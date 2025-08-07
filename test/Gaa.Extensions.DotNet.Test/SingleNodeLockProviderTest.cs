using FluentAssertions.Extensions;

namespace Gaa.Extensions.DotNet.Test;

/// <summary>
/// Набор тестов для <see cref="SingleNodeLockProvider"/>.
/// </summary>
[TestFixture]
public class SingleNodeLockProviderTest
{
    private SingleNodeLockProvider _provider;

    /// <summary>
    /// Настраивает окружение для тестирования.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _provider = new SingleNodeLockProvider();
        _provider.Start();
    }

    /// <summary>
    /// Очищает тестовое окружение.
    /// </summary>
    [TearDown]
    public virtual void TearDown()
    {
        _provider.Stop();
    }

    /// <summary>
    /// Тест взятия блокировки.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task AcquiresLock()
    {
        // arrange
        const string lock1 = "lock1";
        const string lock2 = "lock2";
        const string lock3 = "lock3";

        // act
        await using var distributedLock1 = await _provider.Lock(lock2, CancellationToken.None);
        var acquired = await _provider.TryAcquireLock(lock1, 3.Seconds());
        await using var distributedLock3 = await _provider.Lock(lock3, CancellationToken.None);

        // assert
        acquired.Should().BeTrue();
    }

    /// <summary>
    /// Проверка недоступности блокированного объекта.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task DoesNotAcquireWhenLocked()
    {
        // arrange
        const string lock1 = "lock1";

        // act
        await using var distributedLock1 = await _provider.Lock(lock1, CancellationToken.None);
        var acquired = await _provider.TryAcquireLock(lock1, 1.Seconds());

        // assert
        acquired.Should().BeFalse();
    }

    /// <summary>
    /// Проверка освобождения блокированного объекта.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task ReleasesLock()
    {
        // arrange
        const string lock1 = "lock1";

        // act
        await _provider.AcquireLock(lock1, CancellationToken.None);
        await _provider.ReleaseLock(lock1);

        // assert
        var available = await _provider.TryAcquireLock(lock1, 3.Seconds());
        available.Should().BeTrue();
    }

    /// <summary>
    /// Проверка освобождения блокированного объекта.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task ReleasesLockWithUsing()
    {
        // arrange
        const string lock1 = "lock1";

        // act
        await using (await _provider.Lock(lock1, CancellationToken.None))
        {
            // do something...
        }

        // assert
        var available = await _provider.TryAcquireLock(lock1, 3.Seconds());
        available.Should().BeTrue();
    }

    /// <summary>
    /// Получает объект до истечения таймаута.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task AcquireBeforeTimeOutEnds()
    {
        // arrange
        const string lock1 = "lock1";
        var delayedTask = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            await _provider.ReleaseLock(lock1);
        });

        // act
        await _provider.AcquireLock(lock1, CancellationToken.None);
        var acquired = await _provider.TryAcquireLock(lock1, 3.Seconds());
        await delayedTask;

        // assert
        acquired.Should().BeTrue();
    }

    /// <summary>
    /// Проверка на выброс исключения при получении блокировки.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task CancellingTokenShouldThrow()
    {
        // arrange
        const string lock1 = "lock1";
        using var cts = new CancellationTokenSource(1.Seconds());
        Func<Task> action = async () => await _provider.AcquireLock(lock1, cts.Token);

        // act
        await _provider.AcquireLock(lock1, CancellationToken.None);

        // assert
        await action.Should().ThrowAsync<TaskCanceledException>();
    }
}