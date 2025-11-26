namespace Gaa.Extensions;

/// <summary>
/// Менеджер блокировок для приложения запускаемого в единичном экземпляре.
/// </summary>
public sealed class SingleNodeLockProvider
    : IDistributedLockProvider
{
    /// <summary>
    /// Задержка времени между попытками взятия блокировки.
    /// </summary>
    private readonly TimeSpan _lockDelay;

    /// <summary>
    /// Список заблокированных объектов.
    /// </summary>
    private readonly HashSet<string> _locks;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SingleNodeLockProvider"/>.
    /// </summary>
    public SingleNodeLockProvider()
        : this(TimeSpan.FromSeconds(0.1))
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SingleNodeLockProvider"/>.
    /// </summary>
    /// <param name="lockDelay">Задержка времени между попытками взять блокировку.</param>
    public SingleNodeLockProvider(TimeSpan lockDelay)
    {
        _locks = new HashSet<string>();
        _lockDelay = lockDelay;
    }

    /// <inheritdoc />
    public async Task AcquireLock(
        string lockId,
        CancellationToken cancellationToken = default)
    {
        while (true)
        {
            lock (_locks)
            {
                if (_locks.Add(lockId))
                {
                    return;
                }
            }

            await Task.Delay(_lockDelay, cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task<bool> TryAcquireLock(
        string lockId,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);
        try
        {
            await AcquireLock(lockId, cts.Token);
            return true;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public Task ReleaseLock(
        string lockId,
        CancellationToken cancellationToken = default)
    {
        lock (_locks)
        {
            _locks.Remove(lockId);

            return Task.CompletedTask;
        }
    }

    /// <inheritdoc />
    public Task Start(CancellationToken cancellationToken = default) => Task.CompletedTask;

    /// <inheritdoc />
    public Task Stop(CancellationToken cancellationToken = default) => Task.CompletedTask;
}