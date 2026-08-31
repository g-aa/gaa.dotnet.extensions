using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable SA1204  // Static elements should appear before instance elements

namespace Gaa.Extensions.Observer;

/// <summary>
/// Hosted сервис очереди с фоновыми задачами.
/// </summary>
internal sealed partial class DefaultBusExecutor : BackgroundService
{
    private readonly ILogger _log;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IChildBusFactory<DefaultChildBus> _busFactory;

    private readonly BusOptions _options;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBusExecutor"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика журналов протоколирования событий.</param>
    /// <param name="scopeFactory">Фабрика сервисов.</param>
    /// <param name="busFactory">Очередь с фоновыми задачами.</param>
    /// <param name="options">Настройки шины сообщений.</param>
    public DefaultBusExecutor(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        IChildBusFactory<DefaultChildBus> busFactory,
        IOptions<BusOptions> options)
    {
        _log = loggerFactory.CreateLogger(CategoryName.DefaultBus);
        _scopeFactory = scopeFactory;
        _busFactory = busFactory;
        _options = options.Value;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.StartMessage(_log);
        await InternalExecuteAsync(stoppingToken);
        Log.StopMessage(_log);
    }

    private Task InternalExecuteAsync(CancellationToken stoppingToken)
    {
        var busTasks = new List<Task>(_options.Options.Count);
        foreach (var busOptions in _options.Options)
        {
            var childBus = _busFactory.GetOrCreate(busOptions.Name);
            var busTask = Task.Run(
                () => BusExecuteAsync(childBus, _options.ExecutionTimeLimit, stoppingToken),
                stoppingToken);

            busTasks.Add(busTask);
        }

        return Task.WhenAll(busTasks);
    }

    private async Task BusExecuteAsync(DefaultChildBus childBus, TimeSpan defaultTimeLimit, CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var backgroundTask = await childBus.DequeueTaskAsync(stoppingToken);
                using var scope = _scopeFactory.CreateScope();
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                cts.CancelAfter(GetTimeLimit(backgroundTask.ExecutionTimeLimit, defaultTimeLimit));
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

    private static TimeSpan GetTimeLimit(TimeSpan? taskTimeLimit, TimeSpan defaultTimeLimit)
    {
        if (taskTimeLimit == null)
        {
            return defaultTimeLimit;
        }

        return taskTimeLimit < defaultTimeLimit ? taskTimeLimit.Value : defaultTimeLimit;
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