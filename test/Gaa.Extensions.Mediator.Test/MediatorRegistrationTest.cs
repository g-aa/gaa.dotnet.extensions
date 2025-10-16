using Gaa.Extensions.Test.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Test;

/// <summary>
/// Набор тестов для <see cref="Mediator"/>.
/// </summary>
[TestFixture]
public class MediatorRegistrationTest
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
            .AddHandler<RequestHandlerWithoutResponse, ExampleRequest>()
            .AddHandler<RequestHandlerWithoutResponse, ExampleRequest>()
            .Services
            .BuildServiceProvider();

        // act & assert
        func.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Для запроса {typeof(ExampleRequest).FullName} можно добавить только один обработчик!");
    }
}