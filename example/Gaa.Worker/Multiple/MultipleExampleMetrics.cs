using System.Diagnostics.Metrics;

namespace Gaa.Worker.Multiple;

/// <summary>
/// Пример метрик приложения.
/// </summary>
public sealed class MultipleExampleMetrics
{
    /// <summary>
    /// Наименование набора метрик.
    /// </summary>
    public const string MeterName = "Gaa.Worker.Example.Multiple";

    private readonly Histogram<double> _histogram;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MultipleExampleMetrics"/>.
    /// </summary>
    /// <param name="meterFactory">Фабрика метрик.</param>
    public MultipleExampleMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _histogram = meter.CreateHistogram<double>("gaa.worker.example.multiple.processing.time", "us.", "The message processing time.");
    }

    /// <inheritdoc cref="Histogram{T}.Record(T)" />
    public void Record(double processingTime) => _histogram.Record(processingTime);
}