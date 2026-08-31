using System.Diagnostics.Metrics;

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

    private readonly IChildBus _taskQueue;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBusMetrics"/>.
    /// </summary>
    /// <param name="meterFactory">Фабрика метрик.</param>
    /// <param name="taskQueue">Очередь фоновых задач.</param>
    internal DefaultBusMetrics(IMeterFactory meterFactory, IChildBus taskQueue)
    {
        _taskQueue = taskQueue;
        var meter = meterFactory.Create(MeterName);

        meter.CreateObservableUpDownCounter(
            "gaa.extensions.observer.bus.message.count",
            () => _taskQueue.Count,
            unit: "{messages}",
            description: "Number of messages in the queue.");

        meter.CreateObservableCounter(
            "gaa.extensions.observer.bus.message.capacity",
            () => _taskQueue.Capacity,
            unit: "{messages}",
            description: "Message queue capacity.");
    }
}