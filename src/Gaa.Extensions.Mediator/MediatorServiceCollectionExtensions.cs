using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class MediatorServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует <see cref="IMediator"/> в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="mediatorLifetime">Жизненный цикл <see cref="IMediator"/>.</param>
    /// <param name="handlerLifetime">Жизненный цикл обработчика запросов вида <see cref="IBaseRequestHandler{T}"/> или <see cref="IAsyncBaseRequestHandler{T}"/>.</param>
    /// <param name="processorLifetime">Жизненный цикл <see cref="IRequestPreProcessor{T}"/> или <see cref="IRequestPostProcessor{TRequest, TResponse}"/> или <see cref="IAsyncRequestPreProcessor{T}"/> или <see cref="IAsyncRequestPostProcessor{TRequest, TResponse}"/>.</param>
    /// <returns>Контекст для конфигурирования <see cref="IMediator"/>.</returns>
    public static MediatorConfigurationBuilder AddMediator(
        this IServiceCollection services,
        ServiceLifetime mediatorLifetime = ServiceLifetime.Scoped,
        ServiceLifetime handlerLifetime = ServiceLifetime.Transient,
        ServiceLifetime processorLifetime = ServiceLifetime.Transient)
    {
        return new MediatorConfigurationBuilder(services, mediatorLifetime, handlerLifetime, processorLifetime);
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