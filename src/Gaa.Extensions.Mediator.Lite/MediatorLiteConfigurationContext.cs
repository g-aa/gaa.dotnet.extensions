using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Контекст <see cref="IMediator"/> для конфигурирования.
/// </summary>
public sealed class MediatorLiteConfigurationContext
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MediatorLiteConfigurationContext"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="lifetime">Жизненный цикл.</param>
    public MediatorLiteConfigurationContext(
        IServiceCollection services,
        ServiceLifetime lifetime)
    {
        Services = services;
        Services.Add<IMediator, MediatorLite>(lifetime);
    }

    /// <summary>
    /// Коллекция сервисов.
    /// </summary>
    public IServiceCollection Services { get; init; }

    /// <summary>
    /// Регистрирует обработчик вида <see cref="IRequestHandler{TRequest}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    public MediatorLiteConfigurationContext AddHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where THandler : class, IRequestHandler<TRequest>
        where TRequest : IRequest, allows ref struct
    {
        return Add<IRequestHandler<TRequest>, THandler, TRequest>(lifetime);
    }

    /// <summary>
    /// Регистрирует обработчик вида <see cref="IRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    public MediatorLiteConfigurationContext AddHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>(
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where THandler : class, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, allows ref struct
        where TResponse : allows ref struct
    {
        return Add<IRequestHandler<TRequest, TResponse>, THandler, TRequest>(lifetime);
    }

    /// <summary>
    /// Регистрирует асинхронный обработчик вида <see cref="IAsyncRequestHandler{TRequest}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    public MediatorLiteConfigurationContext AddAsyncHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where THandler : class, IAsyncRequestHandler<TRequest>
        where TRequest : IAsyncRequest
    {
        return Add<IAsyncRequestHandler<TRequest>, THandler, TRequest>(lifetime);
    }

    /// <summary>
    /// Регистрирует асинхронный обработчик вида <see cref="IAsyncRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    public MediatorLiteConfigurationContext AddAsyncHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>(
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where THandler : class, IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        return Add<IAsyncRequestHandler<TRequest, TResponse>, THandler, TRequest>(lifetime);
    }

    private MediatorLiteConfigurationContext Add<TInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        ServiceLifetime lifetime)
        where TInterface : class
        where THandler : class, TInterface
        where TRequest : notnull, allows ref struct
    {
        if (Services.Any(e => e.ServiceType == typeof(TInterface)))
        {
            var requestName = typeof(TRequest).FullName;
            throw new InvalidOperationException($"Для запроса '{requestName}' можно добавить только один обработчик!");
        }

        Services.Add<TInterface, THandler>(lifetime);
        return this;
    }
}