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
    /// Задержка между вызовами для <see cref="Error.ErrorWorker"/>.
    /// </summary>
    public TimeSpan PeriodForErrorWorker { get; set; }

    /// <summary>
    /// Задержка между вызовами для <see cref="Single.SingleExampleWorker"/>.
    /// </summary>
    public TimeSpan PeriodForExampleWorker { get; set; }

    /// <summary>
    /// Задержка между вызовами для <see cref="Multiple.MultipleExampleWorker"/>.
    /// </summary>
    public TimeSpan PeriodForMultipleWorker { get; set; }

    /// <summary>
    /// Количество сообщений отправляемых за один период работы для <see cref="Multiple.MultipleExampleWorker"/>.
    /// </summary>
    public int MessagePerPeriodForMultipleWorker { get; set; }
}