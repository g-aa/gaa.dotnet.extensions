using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Hosted сервис конкурентной транспортной шины в памяти.
/// </summary>
internal sealed partial class ConcurrentTransportExecutor : BackgroundService
{
    private readonly ILogger _log;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ITransportSelector _transportSelector;

    private readonly List<string> _transportNames;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ConcurrentTransportExecutor"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика журналов протоколирования событий.</param>
    /// <param name="scopeFactory">Фабрика сервисов.</param>
    /// <param name="transportSelector">Селектор для выбора транспортной шины.</param>
    /// <param name="options">Общие настройки шины.</param>
    public ConcurrentTransportExecutor(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        ITransportSelector transportSelector,
        IOptions<BusOptions> options)
    {
        _log = loggerFactory.CreateLogger(CategoryName.Concurrent);
        _scopeFactory = scopeFactory;
        _transportSelector = transportSelector;
        _transportNames = [.. options.Value.Transports.Where(o => o is ConcurrentTransportOptions).Select(o => o.Name)];
    }

    /// <inheritdoc />
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var transportName in _transportNames)
        {
            _transportSelector.GetTransport<ConcurrentTransport>(transportName).Close(CancellationToken.None);
        }

        return base.StopAsync(cancellationToken);
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
        var transportTasks = new List<Task>(_transportNames.Count);
        foreach (var transportName in _transportNames)
        {
            var transport = _transportSelector.GetTransport<ConcurrentTransport>(transportName);
            var transportTask = Task.Run(() => transport.RunAsync(_scopeFactory, stoppingToken), stoppingToken);
            transportTasks.Add(transportTask);
        }

        return Task.WhenAll(transportTasks);
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Debug, Message = "Исполняющий сервис конкурентных транспортных шин в памяти запущен на выполнение...")]
        public static partial void StartMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Debug, Message = "Исполняющий сервис конкурентных транспортных шин в памяти остановлен.")]
        public static partial void StopMessage(ILogger log);
    }
}