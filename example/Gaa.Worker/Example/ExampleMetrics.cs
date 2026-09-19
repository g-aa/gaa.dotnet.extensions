using System.Diagnostics.Metrics;

using Gaa.Extensions.Observer;

namespace Gaa.Worker.Example;

/// <summary>
/// Пример метрик приложения.
/// </summary>
public sealed class ExampleMetrics
{
    /// <summary>
    /// Наименование набора метрик.
    /// </summary>
    public const string MeterName = "Gaa.Worker.Example";

    private readonly Histogram<double> _histogram;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="InMemoryTransportMetrics"/>.
    /// </summary>
    /// <param name="meterFactory">Фабрика метрик.</param>
    public ExampleMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _histogram = meter.CreateHistogram<double>("gaa.worker.example.processing.time", "us.", "The message processing time.");
    }

    /// <inheritdoc cref="Histogram{T}.Record(T)" />
    public void Record(double processingTime) => _histogram.Record(processingTime);
}