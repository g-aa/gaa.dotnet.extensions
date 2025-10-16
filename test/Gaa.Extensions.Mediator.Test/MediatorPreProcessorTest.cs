using Gaa.Extensions.Test.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Test;

/// <summary>
/// Набор тестов для <see cref="Mediator"/>.
/// </summary>
[TestFixture]
public class MediatorPreProcessorTest
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
            .AddScopedMediator()
            .AddHandler<RequestHandlerWithResponse, ExampleRequestWithResponse, ExampleResponse>()
            .AddPreProcessor<RequestPreProcessor>()
            .AddAsyncHandler<AsyncRequestHandlerWithResponse, ExampleRequestWithResponse, ExampleResponse>()
            .AddAsyncPreProcessor<AsyncRequestPreProcessor>()
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
    /// Успешное выполнение для <see cref="IAsyncRequestPreProcessor{TRequest},"/>, <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <param name="inputMessage">Входное сообщение.</param>
    /// <param name="outputMessage">Выходное сообщение.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [TestCase("Input message!", "Output message!")]
    public async Task SuccessfulHandleWithResponse(string inputMessage, string outputMessage)
    {
        // arrange
        using var scope = _provider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<Mediator>();
        var request = new ExampleRequestWithResponse { Message = inputMessage };
        var func = () => mediator.SendAsync<ExampleRequestWithResponse, ExampleResponse>(request, default);

        // act & assert
        var response = (await func.Should().NotThrowAsync()).Subject;
        response.Should().NotBeNull();
        response.Message.Should().Be(outputMessage);

        _mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(RequestPreProcessor).FullName}: содержимое сообщения {inputMessage}.")),
            Times.Exactly(1));

        _mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(RequestHandlerWithResponse).FullName}: содержимое сообщения {inputMessage}.")),
            Times.Exactly(1));

        _mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(AsyncRequestPreProcessor).FullName}: содержимое сообщения {inputMessage}.")),
            Times.Exactly(1));

        _mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(AsyncRequestHandlerWithResponse).FullName}: содержимое сообщения {inputMessage}.")),
            Times.Exactly(1));
    }
}