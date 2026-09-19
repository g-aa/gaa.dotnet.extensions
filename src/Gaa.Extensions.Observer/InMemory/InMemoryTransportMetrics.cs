using System.Diagnostics.Metrics;

using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Базовый набор метрик шины обмена сообщениями и событиями.
/// </summary>
public sealed class InMemoryTransportMetrics
{
    /// <summary>
    /// Наименование набора метрик.
    /// </summary>
    public const string MeterName = "Gaa.Extensions.Observer.Transport.InMemory";

    private readonly List<InMemoryTransportOptions> _transportOptions;

    private readonly List<InMemoryTransport> _transports;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="InMemoryTransportMetrics"/>.
    /// </summary>
    /// <param name="meterFactory">Фабрика метрик.</param>
    /// <param name="transportSelector">Селектор для выбора транспортной шины.</param>
    /// <param name="busOptions">Общие настройки шины.</param>
    public InMemoryTransportMetrics(
        IMeterFactory meterFactory,
        IOptions<BusOptions> busOptions,
        ITransportSelector transportSelector)
    {
        _transportOptions = [.. busOptions.Value.Transports.Where(o => o is InMemoryTransportOptions).Cast<InMemoryTransportOptions>()];
        _transports = new(_transportOptions.Count);
        foreach (var options in _transportOptions)
        {
            _transports.Add(transportSelector.GetTransport<InMemoryTransport>(options.Name));
        }

        var meter = meterFactory.Create(MeterName);

        meter.CreateObservableGauge(
            "gaa.extensions.observer.transport.execution.time.limit",
            () => CreateExecutionTimeLimitList(_transportOptions),
            unit: "s",
            description: "Maximum message processing time.");

        meter.CreateObservableGauge(
            "gaa.extensions.observer.transport.message.capacity",
            () => CreateCapacityMeasurementList(_transportOptions),
            unit: "{messages}",
            description: "Message queue capacity.");

        meter.CreateObservableUpDownCounter(
            "gaa.extensions.observer.transport.message.count",
            () => CreateCountMeasurementList(_transports),
            unit: "{messages}",
            description: "Number of messages in the queue.");
    }

    private static List<Measurement<double>> CreateExecutionTimeLimitList(List<InMemoryTransportOptions> options)
    {
        return [.. options.Select(static o => new Measurement<double>(o.ExecutionTimeLimit.TotalSeconds, CreateOptionTag(o)))];
    }

    private static List<Measurement<int>> CreateCapacityMeasurementList(List<InMemoryTransportOptions> options)
    {
        return [.. options.Select(static o => new Measurement<int>(o.Capacity, CreateOptionTag(o)))];
    }

    private static List<Measurement<int>> CreateCountMeasurementList(List<InMemoryTransport> transports)
    {
        return [.. transports.Select(static t => new Measurement<int>(t.Count, CreateTransportTag(t)))];
    }

    private static KeyValuePair<string, object?> CreateOptionTag(InMemoryTransportOptions options)
    {
        return new KeyValuePair<string, object?>("transport.name", options.Name);
    }

    private static KeyValuePair<string, object?> CreateTransportTag(InMemoryTransport transport)
    {
        return new KeyValuePair<string, object?>("transport.name", transport.Name);
    }
}