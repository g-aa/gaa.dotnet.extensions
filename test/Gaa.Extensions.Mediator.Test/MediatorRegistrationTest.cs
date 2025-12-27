using Gaa.Extensions.Test.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Test;

/// <summary>
/// Набор тестов для <see cref="Mediator"/>.
/// </summary>
[TestFixture]
internal sealed class MediatorRegistrationTest
{
    /// <summary>
    /// Настраивает тестовое окружение однократно.
    /// </summary>
    [Test]
    public void UnsuccessfulSingleUseRegistration()
    {
        // arrange
        var func = () => new ServiceCollection()
            .AddScoped(p => new Mock<IMessageLogger>().Object)
            .AddScopedMediator()
            .AddHandler<RequestHandlerWithoutResponse, RequestWithoutResponse>()
            .AddHandler<RequestHandlerWithoutResponse, RequestWithoutResponse>()
            .Services
            .BuildServiceProvider();

        // act & assert
        func.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Для запроса {typeof(RequestWithoutResponse).FullName} можно добавить только один обработчик!");
    }
}