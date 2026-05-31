namespace Gaa.Extensions;

/// <summary>
/// Настройки шины сообщений и событий.
/// </summary>
public sealed class BusOptions
{
    /// <summary>
    /// Емкость очереди фоновых задач.
    /// </summary>
    public int BackgroundTaskQueueCapacity { get; set; } = 100;

    /// <summary>
    /// Ограничение по времени выполнения фонового задания.
    /// </summary>
    public TimeSpan BackgroundTaskExecutionTimeLimit { get; set; } = TimeSpan.FromSeconds(30.0);

    /// <summary>
    /// Количество одновременно обрабатываемых задач.
    /// </summary>
    public int ProcessingBackgroundTaskCount { get; set; } = 4;
}