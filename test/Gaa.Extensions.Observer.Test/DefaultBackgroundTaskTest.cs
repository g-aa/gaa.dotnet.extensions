using Gaa.Extensions.Observer.Test.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer.Test;

/// <summary>
/// Набор тестов для <see cref="DefaultBackgroundTask{TMessage}"/>.
/// </summary>
[TestFixture]
internal sealed class DefaultBackgroundTaskTest
{
    private static readonly Dictionary<string, string> MessageHeaders = [];

    /// <summary>
    /// Успешное выполнение <see cref="DefaultBackgroundTask{TMessage}.ToString()"/>.
    /// </summary>
    [Test]
    public void SuccessfulToString()
    {
        // arrange
        var backgroundTask = new DefaultBackgroundTask<string>("Test message", MessageHeaders);

        // act
        var result = backgroundTask.ToString();

        // assert
        result.Should().Be("Gaa.Extensions.Observer.BackgroundTask<System.String>");
    }

    /// <summary>
    /// Успешное выполнение <see cref="DefaultBackgroundTask{TMessage}.ExecuteAsync(IServiceProvider, CancellationToken)"/>.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task SuccessfulExecuteWithConsumeAsync()
    {
        // arrange
        var provider = new ServiceCollection()
            .AddTransient<IAsyncConsumer<string>, TestConsumer>()
            .AddMessageLogger()
            .BuildServiceProvider();

        var backgroundTask = new DefaultBackgroundTask<string>("Test message", MessageHeaders);

        // act
        var func = () => backgroundTask.ExecuteAsync(provider, CancellationToken.None);

        // assert
        await func.Should().NotThrowAsync();

        provider
            .GetRequiredService<Mock<IMessageLogger>>()
            .Verify(
                l => l.Log(It.Is<string>(m => m == $"Получено сообщение: {backgroundTask.Message}.")),
                Times.Exactly(1));
    }

    /// <summary>
    /// Успешное выполнение <see cref="DefaultBackgroundTask{TMessage}.ExecuteAsync(IServiceProvider, CancellationToken)"/>.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    /// <remarks>Без вызов потребителя сообщения.</remarks>
    [Test]
    public async Task SuccessfulExecuteWithoutConsumeAsync()
    {
        // arrange
        using var provider = new ServiceCollection()
            .AddMessageLogger()
            .BuildServiceProvider();

        var backgroundTask = new DefaultBackgroundTask<string>("Test message", MessageHeaders);

        // act
        var func = () => backgroundTask.ExecuteAsync(provider, CancellationToken.None);

        // assert
        await func.Should().NotThrowAsync();

        provider
            .GetRequiredService<Mock<IMessageLogger>>()
            .Verify(
                l => l.Log(It.Is<string>(m => m == $"Получено сообщение: {backgroundTask.Message}.")),
                Times.Never());
    }
}