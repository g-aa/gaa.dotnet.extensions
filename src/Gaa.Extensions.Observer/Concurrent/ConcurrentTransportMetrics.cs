using System.Diagnostics.Metrics;

using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Базовый набор метрик конкурентной шины обмена сообщениями и событиями.
/// </summary>
public sealed class ConcurrentTransportMetrics
{
    /// <summary>
    /// Наименование набора метрик.
    /// </summary>
    public const string MeterName = "Gaa.Extensions.Observer.Transport.Concurrent";

    private readonly List<ConcurrentTransportOptions> _transportOptions;

    private readonly Lazy<List<ConcurrentTransport>> _lazyTransports;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ConcurrentTransportMetrics"/>.
    /// </summary>
    /// <param name="meterFactory">Фабрика метрик.</param>
    /// <param name="transportSelector">Селектор для выбора транспортной шины.</param>
    /// <param name="busOptions">Общие настройки шины.</param>
    public ConcurrentTransportMetrics(
        IMeterFactory meterFactory,
        IOptions<BusOptions> busOptions,
        ITransportSelector transportSelector)
    {
        _transportOptions = [.. busOptions.Value.Transports.Where(o => o is ConcurrentTransportOptions).Cast<ConcurrentTransportOptions>()];
        _lazyTransports = new(() => GetTransports(transportSelector, _transportOptions), isThreadSafe: true);

        var meter = meterFactory.Create(MeterName);

        meter.CreateObservableGauge(
            "gaa.extensions.observer.transport.execution.time.limit",
            () => CreateExecutionTimeLimitList(_transportOptions),
            unit: "s.",
            description: "Maximum message processing time.");

        meter.CreateObservableGauge(
            "gaa.extensions.observer.transport.consumer.count",
            () => CreateConsumerCountMeasurementList(_transportOptions),
            unit: "{consumers}",
            description: "Number of message consumers.");

        meter.CreateObservableGauge(
            "gaa.extensions.observer.transport.message.capacity",
            () => CreateCapacityMeasurementList(_transportOptions),
            unit: "{messages}",
            description: "Message queue capacity.");

        meter.CreateObservableUpDownCounter(
            "gaa.extensions.observer.transport.message.count",
            () => CreateCountMeasurementList(_lazyTransports.Value),
            unit: "{messages}",
            description: "Number of messages in the queue.");
    }

    private static List<ConcurrentTransport> GetTransports(ITransportSelector transportSelector, List<ConcurrentTransportOptions> options)
    {
        var transports = new List<ConcurrentTransport>(options.Count);
        foreach (var transportOptions in options)
        {
            transports.Add(transportSelector.GetTransport<ConcurrentTransport>(transportOptions.Name));
        }

        return transports;
    }

    private static List<Measurement<double>> CreateExecutionTimeLimitList(List<ConcurrentTransportOptions> options)
    {
        return [.. options.Select(static o => new Measurement<double>(o.ExecutionTimeLimit.TotalSeconds, CreateOptionTag(o)))];
    }

    private static List<Measurement<int>> CreateConsumerCountMeasurementList(List<ConcurrentTransportOptions> options)
    {
        return [.. options.Select(static o => new Measurement<int>(o.ConsumerCount, CreateOptionTag(o)))];
    }

    private static List<Measurement<int>> CreateCapacityMeasurementList(List<ConcurrentTransportOptions> options)
    {
        return [.. options.Select(static o => new Measurement<int>(o.Capacity, CreateOptionTag(o)))];
    }

    private static List<Measurement<int>> CreateCountMeasurementList(List<ConcurrentTransport> transports)
    {
        return [.. transports.Select(static t => new Measurement<int>(t.Count, CreateTransportTag(t)))];
    }

    private static KeyValuePair<string, object?> CreateOptionTag(ConcurrentTransportOptions options)
    {
        return new KeyValuePair<string, object?>("transport.name", options.Name);
    }

    private static KeyValuePair<string, object?> CreateTransportTag(ConcurrentTransport transport)
    {
        return new KeyValuePair<string, object?>("transport.name", transport.Name);
    }
}