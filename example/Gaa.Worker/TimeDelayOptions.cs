namespace Gaa.Worker;

/// <summary>
/// Настройки фоновых задач.
/// </summary>
public sealed class TimeDelayOptions
{
    /// <summary>
    /// Задержка между вызовами для <see cref="Workers.ExampleWorker"/>.
    /// </summary>
    public TimeSpan ExampleWorker { get; set; }

    /// <summary>
    /// Задержка между вызовами для <see cref="Workers.FirstWorker"/>.
    /// </summary>
    public TimeSpan FirstWorker { get; set; }

    /// <summary>
    /// Задержка между вызовами для <see cref="Workers.SecondWorker"/>.
    /// </summary>
    public TimeSpan SecondWorker { get; set; }
}