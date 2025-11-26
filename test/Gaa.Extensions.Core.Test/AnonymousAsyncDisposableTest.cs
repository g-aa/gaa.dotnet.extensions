using System.Collections;

namespace Gaa.Extensions.DotNet.Test;

/// <summary>
/// Набор тестов для <see cref="AnonymousAsyncDisposableTest"/>.
/// </summary>
[TestFixture]
public class AnonymousAsyncDisposableTest
{
    /// <summary>
    /// Успешное анонимное освобождение ресурсов.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task SuccessfulNullDispose()
    {
        // arrange
        var scope = new AnonymousAsyncDisposable(null);

        // act
        var func = async () => await scope.DisposeAsync();

        // assert
        await func.Should().NotThrowAsync();
        scope.IsDisposed.Should().BeTrue();
    }

    /// <summary>
    /// Успешное анонимное освобождение ресурсов.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task SuccessfulDispose()
    {
        // arrange
        var mock = new Mock<IEnumerable>();
        mock.Setup(e => e.GetEnumerator());

        // act
        var func = async () =>
        {
            await using (var scope = new AnonymousAsyncDisposable(() =>
            {
                mock.Object.GetEnumerator();
                return Task.CompletedTask;
            }))
            {
                // do something...
            }
        };

        // assert
        await func.Should().NotThrowAsync();
        mock.Verify(e => e.GetEnumerator(), Times.Once());
    }

    /// <summary>
    /// Успешное анонимное освобождение ресурсов.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task SuccessfulConcurrentDispose()
    {
        // arrange
        var mock = new Mock<IEnumerable>();
        mock.Setup(e => e.GetEnumerator());
        var scope = new AnonymousAsyncDisposable(() =>
        {
            mock.Object.GetEnumerator();
            return Task.CompletedTask;
        });

        // act
        var func = scope.DisposeAsync;
        var tasks = new Task[] { Task.Run(func), Task.Run(func), Task.Run(func), };
        await Task.WhenAll(tasks);

        // assert
        scope.IsDisposed.Should().BeTrue();
        mock.Verify(e => e.GetEnumerator(), Times.Once());
    }
}