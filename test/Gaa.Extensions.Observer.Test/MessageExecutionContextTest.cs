using Gaa.Extensions.Observer.Test.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer.Test;

/// <summary>
/// Набор тестов для <see cref="MessageExecutionContext{TMessage}"/>.
/// </summary>
[TestFixture]
internal sealed class MessageExecutionContextTest
{
    private static readonly Dictionary<string, string> Headers = [];

    /// <summary>
    /// Успешное выполнение <see cref="MessageExecutionContext{TMessage}.ToString()"/>.
    /// </summary>
    [Test]
    public void SuccessfulToString()
    {
        // arrange
        var executionContext = new MessageExecutionContext<string>(Headers, "Test message");

        // act
        var result = executionContext.ToString();

        // assert
        result.Should().Be("Gaa.Extensions.Observer.MessageExecutionContext<System.String>");
    }

    /// <summary>
    /// Успешное выполнение <see cref="MessageExecutionContext{TMessage}.ExecuteAsync(IServiceProvider, CancellationToken)"/>.
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

        var executionContext = new MessageExecutionContext<string>(Headers, "Test message");

        // act
        var func = () => executionContext.ExecuteAsync(provider.GetRequiredService<IServiceScopeFactory>(), CancellationToken.None);

        // assert
        await func.Should().NotThrowAsync();

        provider
            .GetRequiredService<Mock<IMessageLogger>>()
            .Verify(
                l => l.Log(It.Is<string>(m => m == $"Получено сообщение: Test message.")),
                Times.Exactly(1));
    }

    /// <summary>
    /// Успешное выполнение <see cref="MessageExecutionContext{TMessage}.ExecuteAsync(IServiceProvider, CancellationToken)"/>.
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

        var executionContext = new MessageExecutionContext<string>(Headers, "Test message");

        // act
        var func = () => executionContext.ExecuteAsync(provider.GetRequiredService<IServiceScopeFactory>(), CancellationToken.None);

        // assert
        await func.Should().NotThrowAsync();

        provider
            .GetRequiredService<Mock<IMessageLogger>>()
            .Verify(
                l => l.Log(It.Is<string>(m => m == $"Получено сообщение: Test message.")),
                Times.Never());
    }
}