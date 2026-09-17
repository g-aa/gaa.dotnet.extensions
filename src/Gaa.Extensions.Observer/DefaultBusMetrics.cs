using System.Diagnostics.Metrics;

using Microsoft.Extensions.Options;

namespace Gaa.Extensions.Observer;

/// <summary>
/// Базовый набор метрик шины обмена сообщениями и событиями.
/// </summary>
public sealed class DefaultBusMetrics
{
    /// <summary>
    /// Наименование набора метрик.
    /// </summary>
    public const string MeterName = "Gaa.Extensions.Observer.Default.Bus";

    private readonly List<InMemoryTransport> _transports;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBusMetrics"/>.
    /// </summary>
    /// <param name="meterFactory">Фабрика метрик.</param>
    /// <param name="transportSelector">Селектор для выбора транспортной шины.</param>
    /// <param name="busOptions">Общие настройки шины.</param>
    public DefaultBusMetrics(
        IMeterFactory meterFactory,
        IOptions<BusOptions> busOptions,
        ITransportSelector transportSelector)
    {
        var transportOptions = busOptions.Value.Transports.Where(o => o is InMemoryTransportOptions).Cast<InMemoryTransportOptions>().ToList();
        _transports = new(transportOptions.Count);
        foreach (var options in transportOptions)
        {
            _transports.Add((InMemoryTransport)transportSelector.GetTransport(options.Name));
        }

        var meter = meterFactory.Create(MeterName);

        meter.CreateObservableUpDownCounter(
            "gaa.extensions.observer.bus.message.count",
            () => CreateCountMeasurementList(_transports),
            unit: "{messages}",
            description: "Number of messages in the queue.");

        meter.CreateObservableCounter(
            "gaa.extensions.observer.bus.message.capacity",
            () => CreateCapacityMeasurementList(_transports),
            unit: "{messages}",
            description: "Message queue capacity.");
    }

    private static List<Measurement<int>> CreateCountMeasurementList(List<InMemoryTransport> transports)
    {
        return [.. transports.Select(static t => new Measurement<int>(t.Count, CreateBusTag(t)))];
    }

    private static List<Measurement<int>> CreateCapacityMeasurementList(List<InMemoryTransport> transports)
    {
        return [.. transports.Select(static t => new Measurement<int>(t.Capacity, CreateBusTag(t)))];
    }

    private static KeyValuePair<string, object?> CreateBusTag(InMemoryTransport transport)
    {
        return new KeyValuePair<string, object?>("bus.name", transport.Name);
    }
}