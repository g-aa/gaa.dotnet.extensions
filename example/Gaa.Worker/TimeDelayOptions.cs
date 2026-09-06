namespace Gaa.Worker;

/// <summary>
/// Настройки фоновых задач.
/// </summary>
public sealed class TimeDelayOptions
{
    /// <summary>
    /// Задержка между вызовами для <see cref="Example.ExampleWorker"/>.
    /// </summary>
    public TimeSpan ExampleWorker { get; set; }

    /// <summary>
    /// Задержка между вызовами для <see cref="Error.ErrorWorker"/>.
    /// </summary>
    public TimeSpan ErrorWorker { get; set; }
}