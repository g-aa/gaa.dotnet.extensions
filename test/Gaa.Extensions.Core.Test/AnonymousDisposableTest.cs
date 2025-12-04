using System.Collections;

namespace Gaa.Extensions.DotNet.Test;

/// <summary>
/// Набор тестов для <see cref="AnonymousDisposable"/>.
/// </summary>
[TestFixture]
internal sealed class AnonymousDisposableTest
{
    /// <summary>
    /// Успешное анонимное освобождение ресурсов.
    /// </summary>
    [Test]
    public void SuccessfulNullDispose()
    {
        // arrange
        var scope = new AnonymousDisposable(null);

        // act
        var action = scope.Dispose;

        // assert
        action.Should().NotThrow();
        scope.IsDisposed.Should().BeTrue();
    }

    /// <summary>
    /// Успешное анонимное освобождение ресурсов.
    /// </summary>
    [Test]
    public void SuccessfulDispose()
    {
        // arrange
        var mock = new Mock<IEnumerable>();
        mock.Setup(e => e.GetEnumerator());

        // act
        var action = () =>
        {
            using (var scope = new AnonymousDisposable(() => mock.Object.GetEnumerator()))
            {
                // do something...
            }
        };

        // assert
        action.Should().NotThrow();
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
        var scope = new AnonymousDisposable(() => mock.Object.GetEnumerator());

        // act
        var action = scope.Dispose;
        var tasks = new Task[] { Task.Run(action), Task.Run(action), Task.Run(action), };
        await Task.WhenAll(tasks);

        // assert
        scope.IsDisposed.Should().BeTrue();
        mock.Verify(e => e.GetEnumerator(), Times.Once());
    }
}