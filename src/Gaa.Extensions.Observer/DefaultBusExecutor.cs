using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gaa.Extensions;

/// <summary>
/// Hosted сервис очереди с фоновыми задачами.
/// </summary>
internal sealed partial class DefaultBusExecutor : BackgroundService
{
    private readonly ILogger _log;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IBackgroundTaskQueue _taskQueue;

    private readonly IOptions<BusOptions> _options;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBusExecutor"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика журналов протоколирования событий.</param>
    /// <param name="scopeFactory">Фабрика сервисов.</param>
    /// <param name="taskQueue">Очередь с фоновыми задачами.</param>
    /// <param name="options">Настройки шины сообщений.</param>
    public DefaultBusExecutor(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        IBackgroundTaskQueue taskQueue,
        IOptions<BusOptions> options)
    {
        _log = loggerFactory.CreateLogger(CategoryName.DefaultBus);
        _scopeFactory = scopeFactory;
        _taskQueue = taskQueue;
        _options = options;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.StartMessage(_log);
        await InternalExecuteAsync(stoppingToken);
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
                cts.CancelAfter(backgroundTask.ExecutionTimeLimit ?? _options.Value.BackgroundTaskExecutionTimeLimit);
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
        [LoggerMessage(Level = LogLevel.Debug, Message = "Сервис фоновых задач запущен на выполнение...")]
        public static partial void StartMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Debug, Message = "Сервис фоновых задач остановлен.")]
        public static partial void StopMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Error, Message = "Сработала необработанное исключение в процессе выполнения фоновой задачи!")]
        public static partial void ErrorMessage(ILogger log, Exception ex);
    }
}