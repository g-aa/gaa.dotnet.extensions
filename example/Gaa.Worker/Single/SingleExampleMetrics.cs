using System.Diagnostics.Metrics;

namespace Gaa.Worker.Single;

/// <summary>
/// Пример метрик приложения.
/// </summary>
public sealed class SingleExampleMetrics
{
    /// <summary>
    /// Наименование набора метрик.
    /// </summary>
    public const string MeterName = "Gaa.Worker.Example.Single";

    private readonly Histogram<double> _histogram;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SingleExampleMetrics"/>.
    /// </summary>
    /// <param name="meterFactory">Фабрика метрик.</param>
    public SingleExampleMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _histogram = meter.CreateHistogram<double>("gaa.worker.example.single.processing.time", "us.", "The message processing time.");
    }

    /// <inheritdoc cref="Histogram{T}.Record(T)" />
    public void Record(double processingTime) => _histogram.Record(processingTime);
}