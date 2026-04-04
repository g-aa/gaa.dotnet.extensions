using Gaa.Extensions.Test.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Test;

/// <summary>
/// Набор тестов для <see cref="Mediator"/>.
/// </summary>
[TestFixture]
internal sealed class MediatorSendTest
{
    /// <summary>
    /// Успешное выполнение <see cref="IMediator.Send{TRequest}(TRequest, CancellationToken)"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    [TestCase("Input message!")]
    public void SuccessfulSendWithHandler(string message)
    {
        // arrange
        var mockLog = new Mock<IMessageLogger>();
        mockLog.Setup(l => l.Log(It.IsAny<string>()));

        using var provider = new ServiceCollection()
            .AddScoped(p => mockLog.Object)
            .AddMediator()
            .AddHandler<WithoutResponse.Handler, WithoutResponse.Request>()
            .Services
            .BuildServiceProvider();

        // act
        using var scope = provider.CreateScope();
        var mediator = scope.GetMediator();

        var func = () =>
        {
            var request = new WithoutResponse.Request { Message = message };
            mediator.Send(request, default);
        };

        // assert
        func.Should().NotThrow();

        mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(WithoutResponse.Handler).FullName}: содержимое сообщения {message}.")),
            Times.Exactly(1));
    }

    /// <summary>
    /// Успешное выполнение <see cref="IMediator.Send{TRequest}(TRequest, CancellationToken)"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    [TestCase("Input message!")]
    public void SuccessfulSendWithoutHandler(string message)
    {
        // arrange
        var mockLog = new Mock<IMessageLogger>();
        mockLog.Setup(l => l.Log(It.IsAny<string>()));

        using var provider = new ServiceCollection()
            .AddScoped(p => mockLog.Object)
            .AddMediator()
            .Services
            .BuildServiceProvider();

        // act
        using var scope = provider.CreateScope();
        var mediator = scope.GetMediator();

        var func = () =>
        {
            var request = new WithoutResponse.Request { Message = message };
            mediator.Send(request, default);
        };

        // assert
        func.Should().NotThrow();

        mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(WithoutResponse.Handler).FullName}: содержимое сообщения {message}.")),
            Times.Exactly(0));
    }

    /// <summary>
    /// Успешное выполнение <see cref="IMediator.SendAsync{TRequest}(TRequest, CancellationToken)"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [TestCase("Input message!")]
    public async Task SuccessfulSendWithHandlerAsync(string message)
    {
        // arrange
        var mockLog = new Mock<IMessageLogger>();
        mockLog.Setup(l => l.Log(It.IsAny<string>()));

        using var provider = new ServiceCollection()
            .AddScoped(p => mockLog.Object)
            .AddMediator()
            .AddAsyncHandler<AsyncWithoutResponse.Handler, AsyncWithoutResponse.Request>()
            .Services
            .BuildServiceProvider();

        // act
        using var scope = provider.CreateScope();
        var mediator = scope.GetMediator();
        var request = new AsyncWithoutResponse.Request { Message = message };
        var func = () => mediator.SendAsync(request, default);

        // assert
        await func.Should().NotThrowAsync();

        mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(AsyncWithoutResponse.Handler).FullName}: содержимое сообщения {message}.")),
            Times.Exactly(1));
    }

    /// <summary>
    /// Успешное выполнение <see cref="IMediator.SendAsync{TRequest}(TRequest, CancellationToken)"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [TestCase("Input message!")]
    public async Task SuccessfulSendWithoutHandlerAsync(string message)
    {
        // arrange
        var mockLog = new Mock<IMessageLogger>();
        mockLog.Setup(l => l.Log(It.IsAny<string>()));

        using var provider = new ServiceCollection()
            .AddScoped(p => mockLog.Object)
            .AddMediator()
            .Services
            .BuildServiceProvider();

        // act
        using var scope = provider.CreateScope();
        var mediator = scope.GetMediator();
        var request = new AsyncWithoutResponse.Request { Message = message };
        var func = () => mediator.SendAsync(request, default);

        // assert
        await func.Should().NotThrowAsync();

        mockLog.Verify(
            l => l.Log(It.Is<string>(m => m == $"{typeof(AsyncWithoutResponse.Handler).FullName}: содержимое сообщения {message}.")),
            Times.Exactly(0));
    }
}