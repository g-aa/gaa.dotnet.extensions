using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable SA1204  // Static elements should appear before instance elements

namespace Gaa.Extensions.Observer;

/// <summary>
/// Hosted сервис транспортной шины в памяти.
/// </summary>
internal sealed partial class InMemoryTransportExecutor : BackgroundService
{
    private readonly ILogger _log;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ITransportSelector _transportSelector;

    private readonly BusOptions _options;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="InMemoryTransportExecutor"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика журналов протоколирования событий.</param>
    /// <param name="scopeFactory">Фабрика сервисов.</param>
    /// <param name="transportSelector">Селектор для выбора транспортной шины.</param>
    /// <param name="options">Общие настройки шины.</param>
    public InMemoryTransportExecutor(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        ITransportSelector transportSelector,
        IOptions<BusOptions> options)
    {
        _log = loggerFactory.CreateLogger(CategoryName.InMemory);
        _scopeFactory = scopeFactory;
        _transportSelector = transportSelector;
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
        var transportOptions = _options.Transports.Where(o => o is InMemoryTransportOptions).Cast<InMemoryTransportOptions>().ToList();
        var transportTasks = new List<Task>(transportOptions.Count);
        foreach (var options in transportOptions)
        {
            var transport = _transportSelector.GetTransport<InMemoryTransport>(options.Name);
            var transportTask = Task.Run(() => transport.RunAsync(_scopeFactory, stoppingToken), stoppingToken);
            transportTasks.Add(transportTask);
        }

        return Task.WhenAll(transportTasks);
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Debug, Message = "Исполняющий сервис транспортных шин в памяти запущен на выполнение...")]
        public static partial void StartMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Debug, Message = "Исполняющий сервис транспортных шин в памяти остановлен.")]
        public static partial void StopMessage(ILogger log);
    }
}