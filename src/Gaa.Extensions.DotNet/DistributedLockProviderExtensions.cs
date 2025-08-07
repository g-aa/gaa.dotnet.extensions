namespace Gaa.Extensions.DotNet;

/// <summary>
/// Методы расширения для <see cref="IDistributedLockProvider"/>.
/// </summary>
public static class DistributedLockProviderExtensions
{
    /// <summary>
    /// Создает блокировку по указанному идентификатору.
    /// </summary>
    /// <param name="provider">Распределенный менеджер блокировок.</param>
    /// <param name="lockId">Идентификатор блокировки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Объект управляющий распределенной блокировкой.</returns>
    public static async Task<IAsyncDisposable> Lock(
        this IDistributedLockProvider provider,
        string lockId,
        CancellationToken cancellationToken = default)
    {
        await provider.AcquireLock(lockId, cancellationToken);

        return new AnonymousAsyncDisposable(async () =>
        {
            await provider.ReleaseLock(lockId, cancellationToken);
        });
    }
}