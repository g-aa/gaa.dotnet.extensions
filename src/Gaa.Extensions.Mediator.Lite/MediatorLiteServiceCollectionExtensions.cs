using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class MediatorLiteServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует <see cref="IMediator"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Контекст <see cref="IMediator"/> для конфигурирования.</returns>
    public static MediatorLiteConfigurationContext AddMediatorLite(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        return new MediatorLiteConfigurationContext(services, lifetime);
    }

    /// <summary>
    /// Регистрирует сервис <typeparamref name="TService"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип имплементации сервиса.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="serviceKey">Ключ сервиса вида <see cref="ServiceDescriptor.ServiceKey"/>.</param>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    internal static IServiceCollection AddKeyed<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this IServiceCollection services,
        object? serviceKey,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);

        var descriptor = new ServiceDescriptor(typeof(TService), serviceKey, typeof(TImplementation), lifetime);
        services.Add(descriptor);
        return services;
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