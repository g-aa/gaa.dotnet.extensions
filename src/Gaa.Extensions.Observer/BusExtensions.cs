using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class BusExtensions
{
    /// <summary>
    /// Регистрирует компоненты <see cref="IBus"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Настройки конфигурации.</param>
    /// <returns>Контекст <see cref="IBus"/> для конфигурирования.</returns>
    /// <remarks>Жизненный цикл <see cref="ServiceLifetime.Scoped"/>.</remarks>
    public static BusConfigurationContext AddScopedBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .Configure<BusOptions>(BusOptions.Section, configuration.GetRequiredSection(BusOptions.Section))
            .AddScoped<IBus, Bus>()
            .AddSingleton<IBackgroundTaskQueue, DefaultBackgroundTaskQueue>()
            .AddHostedService<BackgroundTaskExecutionService>();

        return new()
        {
            Services = services,
        };
    }
}