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
        var func = () =>
        {
            var services = new ServiceCollection();

            services
                .AddScoped(p => new Mock<IMessageLogger>().Object)
                .AddScopedMediator()
                .AddHandle<RequestHandlerWithoutResponse, ExampleRequest>()
                .AddHandle<RequestHandlerWithoutResponse, ExampleRequest>();

            return services.BuildServiceProvider();
        };

        // act & assert
        func.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Для запроса {typeof(ExampleRequest).Name} можно добавить только один обработчик!");
    }
}