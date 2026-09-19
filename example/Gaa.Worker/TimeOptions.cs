namespace Gaa.Worker;

/// <summary>
/// Настройки фоновых задач.
/// </summary>
public sealed class TimeOptions
{
    /// <summary>
    /// Время до полной остановки <see cref="IHost.StartAsync(CancellationToken)"/>.
    /// </summary>
    public TimeSpan HostExecutionTimeLimit { get; set; }

    /// <summary>
    /// Задержка между вызовами для <see cref="Example.ExampleWorker"/>.
    /// </summary>
    public TimeSpan PeriodForExampleWorker { get; set; }

    /// <summary>
    /// Задержка между вызовами для <see cref="Error.ErrorWorker"/>.
    /// </summary>
    public TimeSpan PeriodForErrorWorker { get; set; }
}