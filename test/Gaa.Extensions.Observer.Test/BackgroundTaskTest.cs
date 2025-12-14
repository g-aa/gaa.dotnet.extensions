using Gaa.Extensions.Test.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Test;

/// <summary>
/// Набор тестов для <see cref="BackgroundTask{TMessage}"/>.
/// </summary>
[TestFixture]
internal sealed class BackgroundTaskTest
{
    /// <summary>
    /// Успешное выполнение <see cref="BackgroundTask{TMessage}.ToString()"/>.
    /// </summary>
    [Test]
    public void SuccessfulToString()
    {
        // arrange
        var backgroundTask = new BackgroundTask<string>
        {
            Message = "Test message",
            MessageHeaders = new Dictionary<string, string>(),
        };

        // act
        var result = backgroundTask.ToString();

        // assert
        result.Should().Be("BackgroundTask<System.String>");
    }

    /// <summary>
    /// Успешное выполнение <see cref="BackgroundTask{TMessage}.ExecuteAsync(IServiceProvider, CancellationToken)"/>.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Test]
    public async Task SuccessfulExecuteAsync()
    {
        // arrange
        var mockLog = new Mock<IMessageLogger>();
        mockLog.Setup(l => l.Log(It.IsAny<string>()));

        var provider = new ServiceCollection()
            .AddTransient<IAsyncConsumer<string>, TestConsumer>()
            .AddTransient(_ => mockLog.Object)
            .BuildServiceProvider();

        var backgroundTask = new BackgroundTask<string>
        {
            Message = "Test message",
            MessageHeaders = new Dictionary<string, string>(),
        };

        // act
        var func = () => backgroundTask.ExecuteAsync(provider, CancellationToken.None);

        // assert
        await func.Should().NotThrowAsync();

        mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"Получено сообщение: {backgroundTask.Message}.")),
            Times.Exactly(1));
    }
}