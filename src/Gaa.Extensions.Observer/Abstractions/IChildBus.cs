#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Дочерняя шина для <see cref="IBackgroundTask"/>.
/// </summary>
internal interface IChildBus
{
    /// <summary>
    /// Наименование.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Емкость очереди сообщений.
    /// </summary>
    int Capacity { get; }

    /// <summary>
    /// Количество сообщений в очереди.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Добавляет фоновую задачу в очередь на выполнение.
    /// </summary>
    /// <param name="backgroundTask">Фоновая задача.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task QueueTaskAsync(IBackgroundTask backgroundTask, CancellationToken cancellationToken);

    /// <summary>
    /// Изымает фоновую задачу из очереди для исполнения.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Фоновая задача.</returns>
    Task<IBackgroundTask> DequeueTaskAsync(CancellationToken cancellationToken);
}