using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gaa.Extensions;

/// <summary>
/// Hosted сервис очереди с фоновыми задачами.
/// </summary>
internal sealed partial class BackgroundTaskExecutionService
    : BackgroundService
{
    private readonly ILogger _log;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IBackgroundTaskQueue _taskQueue;

    private readonly BusOptions _options;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="BackgroundTaskExecutionService"/>.
    /// </summary>
    /// <param name="log">Журнал протоколирования событий.</param>
    /// <param name="serviceScopeFactory">Фабрика сервисов.</param>
    /// <param name="taskQueue">Очередь с фоновыми задачами.</param>
    /// <param name="options">Настройки шины сообщений.</param>
    public BackgroundTaskExecutionService(
      ILogger<BackgroundTaskExecutionService> log,
      IServiceScopeFactory serviceScopeFactory,
      IBackgroundTaskQueue taskQueue,
      IOptions<BusOptions> options)
    {
        _log = log;
        _scopeFactory = serviceScopeFactory;
        _taskQueue = taskQueue;
        _options = options.Value;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.StartMessage(_log);
        Log.ExecutionTimeLimitMessage(_log, _options.BackgroundTaskExecutionTimeLimit);

        var count = _options.ProcessingBackgroundTaskCount;
        Log.ProcessingTaskCountMessage(_log, count);

        var tasks = Enumerable
            .Range(1, count)
            .Select(_ => InternalExecuteAsync(stoppingToken))
            .ToList();

        await Task.WhenAll(tasks);
        Log.StopMessage(_log);
    }

    private async Task InternalExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var backgroundTask = await _taskQueue.DequeueTaskAsync(stoppingToken);
                using var scope = _scopeFactory.CreateScope();
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                cts.CancelAfter(_options.BackgroundTaskExecutionTimeLimit);
                await backgroundTask.ExecuteAsync(scope.ServiceProvider, cts.Token);
            }
            catch (OperationCanceledException)
            {
                /* Можно не обрабатывать */
            }
            catch (Exception ex)
            {
                Log.ErrorMessage(_log, ex);
            }
        }
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Trace, Message = "Сервис фоновых задач запущен на выполнение...")]
        public static partial void StartMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Ограничение по времени выполнения фоновой задачи выставлено равным: '{Time}'.")]
        public static partial void ExecutionTimeLimitMessage(ILogger log, TimeSpan time);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Количество одновременно обрабатываемых задач: '{Count}'.")]
        public static partial void ProcessingTaskCountMessage(ILogger log, int count);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Сервис фоновых задач остановлен.")]
        public static partial void StopMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Error, Message = "Сработала ошибка в процессе выполнения фоновой задачи!")]
        public static partial void ErrorMessage(ILogger log, Exception ex);
    }
}