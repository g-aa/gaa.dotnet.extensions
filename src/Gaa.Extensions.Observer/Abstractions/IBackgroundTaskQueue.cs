#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable CA1711  // Identifiers should not have incorrect suffix

namespace Gaa.Extensions.Observer;

/// <summary>
/// Очередь фоновых задач.
/// </summary>
public interface IBackgroundTaskQueue
{
    /// <summary>
    /// Добавляет фоновую задачу в очередь на выполнение.
    /// </summary>
    /// <param name="backgroundTask">Фоновая задача.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task QueueTaskAsync(
        IBackgroundTask backgroundTask,
        CancellationToken cancellationToken);

    /// <summary>
    /// Изымает фоновую задачу из очереди для исполнения.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Фоновая задача.</returns>
    Task<IBackgroundTask> DequeueTaskAsync(
        CancellationToken cancellationToken);
}