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

    private readonly List<IBackgroundTaskBus> _buses;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBusMetrics"/>.
    /// </summary>
    /// <param name="meterFactory">Фабрика метрик.</param>
    /// <param name="busFactory">Очередь с фоновыми задачами.</param>
    /// <param name="options">Настройки шины сообщений.</param>
    public DefaultBusMetrics(
        IMeterFactory meterFactory,
        IOptions<BusOptions> options,
        IBackgroundTaskBusFactory busFactory)
    {
        var busOprions = options.Value.Options;
        _buses = new(busOprions.Count);
        foreach (var childOptions in busOprions)
        {
            _buses.Add(busFactory.GetOrCreate(childOptions.Name));
        }

        var meter = meterFactory.Create(MeterName);

        meter.CreateObservableUpDownCounter(
            "gaa.extensions.observer.bus.message.count",
            () => CreateCountMeasurementList(_buses),
            unit: "{messages}",
            description: "Number of messages in the queue.");

        meter.CreateObservableCounter(
            "gaa.extensions.observer.bus.message.capacity",
            () => CreateCapacityMeasurementList(_buses),
            unit: "{messages}",
            description: "Message queue capacity.");
    }

    private static List<Measurement<int>> CreateCountMeasurementList(List<IBackgroundTaskBus> buses)
    {
        return [.. buses.Select(static bus => new Measurement<int>(bus.Count, CreateBusTag(bus)))];
    }

    private static List<Measurement<int>> CreateCapacityMeasurementList(List<IBackgroundTaskBus> buses)
    {
        return [.. buses.Select(static bus => new Measurement<int>(bus.Capacity, CreateBusTag(bus)))];
    }

    private static KeyValuePair<string, object?> CreateBusTag(IBackgroundTaskBus bus)
    {
        return new KeyValuePair<string, object?>("bus.name", bus.Name);
    }
}