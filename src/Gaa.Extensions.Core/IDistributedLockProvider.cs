namespace Gaa.Extensions;

/// <summary>
/// Распределенный менеджер блокировок.
/// </summary>
/// <remarks>
/// Реализация данного интерфейса будет отвечать за
/// обеспечение распределенного механизма блокировки для управления рабочими процессами в приложении.
/// </remarks>
public interface IDistributedLockProvider
{
    /// <summary>
    /// Взять блокировку.
    /// </summary>
    /// <param name="lockId">Идентификатор блокировки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task AcquireLock(
        string lockId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Попытаться взять блокировку.
    /// </summary>
    /// <param name="lockId">Идентификатор блокировки.</param>
    /// <param name="timeout">Максимальное время ожидания получения блокировки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат проведения операции.</returns>
    Task<bool> TryAcquireLock(
        string lockId,
        TimeSpan timeout,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Освободить блокировку.
    /// </summary>
    /// <param name="lockId">Идентификатор блокировки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ReleaseLock(
        string lockId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Инициализация провайдера.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task Start(CancellationToken cancellationToken = default);

    /// <summary>
    /// Остановка провайдера.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task Stop(CancellationToken cancellationToken = default);
}