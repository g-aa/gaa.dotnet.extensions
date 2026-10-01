using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class BusServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует компоненты <see cref="IPublisher"/> и <see cref="ITransport"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configureOptions">Настройки шины.</param>
    /// <returns>Контекст конфигурирования.</returns>
    public static BusConfigurationBuilder AddBus(
        this IServiceCollection services,
        Action<BusOptions> configureOptions)
    {
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        services
            .AddSingleton<ITransportSelector, DefaultTransportSelector>()
            .AddSingleton<ITransportNameSelector, DefaultTransportNameSelector>()
            .AddSingleton<IPublisher, DefaultPublisher>();

        return new(services);
    }

    /// <summary>
    /// Регистрирует компоненты транспортной шины в памяти в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <param name="configureOptions">Настройки транспортной шины.</param>
    /// <returns>Контекст конфигурирования.</returns>
    internal static TransportConfigurationBuilder AddInMemoryTransport(
        this IServiceCollection services,
        string transportName,
        Action<InMemoryTransportOptions> configureOptions)
    {
        var transportOptions = new InMemoryTransportOptions
        {
            Type = typeof(InMemoryTransport),
            Name = transportName,
        };

        configureOptions.Invoke(transportOptions);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ITransportFactory, InMemoryTransportFactory>());
        services
            .AddHostedService<InMemoryTransportExecutor>()
            .Configure<BusOptions>(options =>
            {
                options.Transports.Add(transportOptions);
                options.Subscriptions.Add(transportName, new HashSet<Type>());
            });

        return new(transportName, services);
    }

    /// <summary>
    /// Регистрирует компоненты конкурентной транспортной шины в памяти в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <param name="configureOptions">Настройки транспортной шины.</param>
    /// <returns>Контекст конфигурирования.</returns>
    internal static TransportConfigurationBuilder AddConcurrentTransport(
        this IServiceCollection services,
        string transportName,
        Action<ConcurrentTransportOptions> configureOptions)
    {
        var transportOptions = new ConcurrentTransportOptions
        {
            Type = typeof(ConcurrentTransport),
            Name = transportName,
        };

        configureOptions.Invoke(transportOptions);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ITransportFactory, ConcurrentTransportFactory>());
        services
            .AddHostedService<ConcurrentTransportExecutor>()
            .Configure<BusOptions>(options =>
            {
                options.Transports.Add(transportOptions);
                options.Subscriptions.Add(transportName, new HashSet<Type>());
            });

        return new(transportName, services);
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