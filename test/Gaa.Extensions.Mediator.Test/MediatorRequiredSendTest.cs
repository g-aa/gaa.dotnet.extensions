using Gaa.Extensions.Test.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Test;

/// <summary>
/// Набор тестов для <see cref="Mediator"/>.
/// </summary>
[TestFixture]
internal sealed class MediatorRequiredSendTest
{
    private Mock<IMessageLogger> _mockLog;

    private ServiceProvider _provider;

    /// <summary>
    /// Настраивает тестовое окружение однократно.
    /// </summary>
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _mockLog = new Mock<IMessageLogger>();
        _provider = new ServiceCollection()
            .AddScoped(p => _mockLog.Object)
            .AddMediator()
            .AddHandler<WithoutResponse.Handler, WithoutResponse.Request>()
            .AddHandler<WithResponse.Handler, WithResponse.Request>()
            .AddAsyncHandler<AsyncWithoutResponse.Handler, AsyncWithoutResponse.Request>()
            .AddAsyncHandler<AsyncWithResponse.Handler, AsyncWithResponse.Request>()
            .Services
            .BuildServiceProvider();
    }

    /// <summary>
    /// Настраивает окружение для тестирования.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _mockLog.Setup(l => l.Log(It.IsAny<string>()));
    }

    /// <summary>
    /// Освобождает ресурсы однократно после окончания тестирования.
    /// </summary>
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _provider.Dispose();
    }

    /// <summary>
    /// Успешное выполнение <see cref="IMediator.RequiredSend{TRequest}(TRequest, CancellationToken)"/>.
    /// </summary>
    /// <param name="inputMessage">Входное сообщение.</param>
    [TestCase("Input message!")]
    public void SuccessfulRequiredSendWithoutResponse(string inputMessage)
    {
        // arrange
        using var scope = _provider.CreateScope();
        var mediator = scope.ServiceProvider.GetMediator();
        var func = () =>
        {
            var request = new WithoutResponse.Request { Message = inputMessage };
            mediator.RequiredSend(request, default);
        };

        // act & assert
        func.Should().NotThrow();

        _mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(WithoutResponse.Handler).FullName}: содержимое сообщения {inputMessage}.")),
            Times.Exactly(1));
    }

    /// <summary>
    /// Успешное выполнение <see cref="IMediator.RequiredSendAsync{TRequest}(TRequest, CancellationToken)"/>.
    /// </summary>
    /// <param name="inputMessage">Входное сообщение.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [TestCase("Input message!")]
    public async Task SuccessfulRequiredSendWithoutResponseAsync(string inputMessage)
    {
        // arrange
        using var scope = _provider.CreateScope();
        var mediator = scope.GetMediator();
        var request = new AsyncWithoutResponse.Request { Message = inputMessage };
        var func = () => mediator.RequiredSendAsync(request, default);

        // act & assert
        await func.Should().NotThrowAsync();

        _mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(AsyncWithoutResponse.Handler).FullName}: содержимое сообщения {inputMessage}.")),
            Times.Exactly(1));
    }

    /// <summary>
    /// Успешное выполнение <see cref="IMediator.RequiredSend{TRequest, TResponse}(TRequest, CancellationToken)"/>.
    /// </summary>
    /// <param name="inputMessage">Входное сообщение.</param>
    /// <param name="outputMessage">Выходное сообщение.</param>
    [TestCase("Input message!", "Output message!")]
    public void SuccessfulRequiredSendWithResponse(string inputMessage, string outputMessage)
    {
        // arrange
        using var scope = _provider.CreateScope();
        var mediator = scope.GetMediator();
        var func = () =>
        {
            var request = new WithResponse.Request { Message = inputMessage };
            return mediator.RequiredSend<WithResponse.Request, Response>(request, default);
        };

        // act & assert
        var response = func.Should().NotThrow().Subject;
        response.Should().NotBeNull();
        response.Message.Should().Be(outputMessage);

        _mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(WithResponse.Handler).FullName}: содержимое сообщения {inputMessage}.")),
            Times.Exactly(1));
    }

    /// <summary>
    /// Успешное выполнение <see cref="IMediator.RequiredSendAsync{TRequest, TResponse}(TRequest, CancellationToken)"/>.
    /// </summary>
    /// <param name="inputMessage">Входное сообщение.</param>
    /// <param name="outputMessage">Выходное сообщение.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [TestCase("Input message!", "Output message!")]
    public async Task SuccessfulRequiredSendWithResponseAsync(string inputMessage, string outputMessage)
    {
        // arrange
        using var scope = _provider.CreateScope();
        var mediator = scope.GetMediator();
        var request = new AsyncWithResponse.Request { Message = inputMessage };
        var func = () => mediator.RequiredSendAsync<AsyncWithResponse.Request, Response>(request, default);

        // act & assert
        var response = (await func.Should().NotThrowAsync()).Subject;
        response.Should().NotBeNull();
        response.Message.Should().Be(outputMessage);

        _mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(AsyncWithResponse.Handler).FullName}: содержимое сообщения {inputMessage}.")),
            Times.Exactly(1));
    }
}