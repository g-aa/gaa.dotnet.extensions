using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Контекст для конфигурирования <see cref="IMediator"/>.
/// </summary>
public sealed class MediatorLiteConfigurationBuilder
{
    private readonly ServiceLifetime _handlerLifetime;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MediatorLiteConfigurationBuilder"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="mediatorLifetime">Жизненный цикл <see cref="IMediator"/>.</param>
    /// <param name="handlerLifetime">Жизненный цикл обработчика запросов.</param>
    internal MediatorLiteConfigurationBuilder(
        IServiceCollection services,
        ServiceLifetime mediatorLifetime,
        ServiceLifetime handlerLifetime)
    {
        Services = services;
        Services.Add<IMediator, MediatorLite>(mediatorLifetime);
        _handlerLifetime = handlerLifetime;
    }

    /// <summary>
    /// Коллекция сервисов.
    /// </summary>
    public IServiceCollection Services { get; init; }

    /// <summary>
    /// Регистрирует обработчик реализующий интерфейс вида <see cref="IRequestHandler{TRequest}"/> или <see cref="IRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public MediatorLiteConfigurationBuilder AddHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
        where THandler : class, IBaseRequestHandler<TRequest>
        where TRequest : allows ref struct
    {
        return Add<IBaseRequestHandler<TRequest>, THandler, TRequest>(_handlerLifetime);
    }

    /// <summary>
    /// Регистрирует обработчик реализующий интерфейс вида <see cref="IRequestHandler{TRequest}"/> или <see cref="IRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    public MediatorLiteConfigurationBuilder AddHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        ServiceLifetime lifetime)
        where THandler : class, IBaseRequestHandler<TRequest>
        where TRequest : allows ref struct
    {
        return Add<IBaseRequestHandler<TRequest>, THandler, TRequest>(lifetime);
    }

    /// <summary>
    /// Регистрирует обработчик реализующий интерфейс вида <see cref="IAsyncRequestHandler{TRequest}"/> или <see cref="IAsyncRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public MediatorLiteConfigurationBuilder AddAsyncHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
        where THandler : class, IAsyncBaseRequestHandler<TRequest>
    {
        return Add<IAsyncBaseRequestHandler<TRequest>, THandler, TRequest>(_handlerLifetime);
    }

    /// <summary>
    /// Регистрирует обработчик реализующий интерфейс вида <see cref="IAsyncRequestHandler{TRequest}"/> или <see cref="IAsyncRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    public MediatorLiteConfigurationBuilder AddAsyncHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        ServiceLifetime lifetime)
        where THandler : class, IAsyncBaseRequestHandler<TRequest>
    {
        return Add<IAsyncBaseRequestHandler<TRequest>, THandler, TRequest>(lifetime);
    }

    private MediatorLiteConfigurationBuilder Add<TInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        ServiceLifetime lifetime)
        where TInterface : class
        where THandler : class, TInterface
        where TRequest : allows ref struct
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