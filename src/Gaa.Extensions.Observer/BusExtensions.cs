using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class BusExtensions
{
    /// <summary>
    /// Регистрирует компоненты <see cref="IPublisher"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configureOptions">Настройки конфигурации.</param>
    /// <returns>Контекст <see cref="IPublisher"/> для конфигурирования.</returns>
    public static BusConfigurationBuilder AddBus(
        this IServiceCollection services,
        Action<BusOptions> configureOptions)
    {
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        services
            .AddHostedService<DefaultBusExecutor>()
            .AddSingleton<IPublisher, DefaultBusPublisher>()
            .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

        return new()
        {
            Services = services,
        };
    }

    /// <summary>
    /// Регистрирует сервис <typeparamref name="TService"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип имплементации сервиса.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    internal static IServiceCollection Add<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);

        var descriptor = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);
        services.Add(descriptor);
        return services;
    }
}