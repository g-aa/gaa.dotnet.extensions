using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class MediatorExtensions
{
    /// <summary>
    /// Регистрирует <see cref="Mediator"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Контекст <see cref="Mediator"/> для конфигурирования.</returns>
    /// <remarks>Жизненный цикл <see cref="ServiceLifetime.Scoped"/>.</remarks>
    public static MediatorConfigurationContext AddScopedMediator(this IServiceCollection services)
    {
        return new MediatorConfigurationContext
        {
            Services = services.AddScoped<IMediator, Mediator>(),
        };
    }
}