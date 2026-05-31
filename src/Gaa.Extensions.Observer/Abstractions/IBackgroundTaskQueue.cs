#pragma warning disable IDE0130 // Пространство имен (namespace) не соответствует структуре папок.
#pragma warning disable CA1711  // Идентификаторы не должны иметь неправильных суффиксов

namespace Gaa.Extensions;

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
    ValueTask QueueTaskAsync(
        IBackgroundTask backgroundTask,
        CancellationToken cancellationToken);

    /// <summary>
    /// Изымает фоновую задачу из очереди для исполнения.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Фоновая задача.</returns>
    ValueTask<IBackgroundTask> DequeueTaskAsync(
        CancellationToken cancellationToken);
}