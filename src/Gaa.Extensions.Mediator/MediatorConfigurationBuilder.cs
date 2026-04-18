using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Контекст <see cref="IMediator"/> для конфигурирования.
/// </summary>
public sealed class MediatorConfigurationBuilder
{
    private readonly ServiceLifetime _handlerLifetime;

    private readonly ServiceLifetime _processorLifetime;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MediatorConfigurationBuilder"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="mediatorLifetime">Жизненный цикл <see cref="IMediator"/>.</param>
    /// <param name="handlerLifetime">Жизненный цикл обработчика запросов.</param>
    /// <param name="processorLifetime">Жизненный цикл пред. и пост обработчиков запроса.</param>
    internal MediatorConfigurationBuilder(
        IServiceCollection services,
        ServiceLifetime mediatorLifetime,
        ServiceLifetime handlerLifetime,
        ServiceLifetime processorLifetime)
    {
        Services = services;
        Services.Add<IMediator, Mediator>(mediatorLifetime);
        _handlerLifetime = handlerLifetime;
        _processorLifetime = processorLifetime;
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
    /// <param name="configure">Конфигурирует обработчик запроса.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public MediatorConfigurationBuilder AddHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        Action<RequestHandlerConfigurationBuilder<TRequest>>? configure = null)
        where THandler : class, IBaseRequestHandler<TRequest>
        where TRequest : notnull, allows ref struct
    {
        Add<IBaseRequestHandler<TRequest>, THandler, TRequest>(_handlerLifetime);
        Configure(this, configure);
        return this;
    }

    /// <summary>
    /// Регистрирует обработчик реализующий интерфейс вида <see cref="IRequestHandler{TRequest}"/> или <see cref="IRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <param name="configure">Конфигурирует обработчик запроса.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    public MediatorConfigurationBuilder AddHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        ServiceLifetime lifetime,
        Action<RequestHandlerConfigurationBuilder<TRequest>>? configure = null)
        where THandler : class, IBaseRequestHandler<TRequest>
        where TRequest : notnull, allows ref struct
    {
        Add<IBaseRequestHandler<TRequest>, THandler, TRequest>(lifetime);
        Configure(this, configure);
        return this;
    }

    /// <summary>
    /// Регистрирует обработчик вида <see cref="IRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="configure">Конфигурирует обработчик запроса.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public MediatorConfigurationBuilder AddHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>(
        Action<RequestHandlerConfigurationBuilder<TRequest, TResponse>> configure)
        where THandler : class, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, allows ref struct
        where TResponse : notnull, allows ref struct
    {
        Add<IBaseRequestHandler<TRequest>, THandler, TRequest>(_handlerLifetime);
        Configure(this, configure);
        return this;
    }

    /// <summary>
    /// Регистрирует обработчик вида <see cref="IRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <param name="configure">Конфигурирует обработчик запроса.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    public MediatorConfigurationBuilder AddHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>(
        ServiceLifetime lifetime,
        Action<RequestHandlerConfigurationBuilder<TRequest, TResponse>> configure)
        where THandler : class, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, allows ref struct
        where TResponse : notnull, allows ref struct
    {
        Add<IBaseRequestHandler<TRequest>, THandler, TRequest>(lifetime);
        Configure(this, configure);
        return this;
    }

    /// <summary>
    /// Регистрирует обработчик реализующий интерфейс вида <see cref="IAsyncRequestHandler{TRequest}"/> или <see cref="IAsyncRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="configure">Конфигурирует обработчик запроса.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    public MediatorConfigurationBuilder AddAsyncHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        Action<AsyncRequestHandlerConfigurationBuilder<TRequest>>? configure = null)
        where THandler : class, IAsyncBaseRequestHandler<TRequest>
        where TRequest : notnull
    {
        Add<IAsyncBaseRequestHandler<TRequest>, THandler, TRequest>(_handlerLifetime);
        Configure(this, configure);
        return this;
    }

    /// <summary>
    /// Регистрирует обработчик реализующий интерфейс вида <see cref="IAsyncRequestHandler{TRequest}"/> или <see cref="IAsyncRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <param name="configure">Конфигурирует обработчик запроса.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    public MediatorConfigurationBuilder AddAsyncHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        ServiceLifetime lifetime,
        Action<AsyncRequestHandlerConfigurationBuilder<TRequest>>? configure = null)
        where THandler : class, IAsyncBaseRequestHandler<TRequest>
        where TRequest : notnull
    {
        Add<IAsyncBaseRequestHandler<TRequest>, THandler, TRequest>(lifetime);
        Configure(this, configure);
        return this;
    }

    /// <summary>
    /// Регистрирует обработчик вида <see cref="IAsyncRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="configure">Конфигурирует обработчик запроса.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    /// <remarks>Жизненный цикл обработчика запросов берется из настроек <see cref="IMediator"/>.</remarks>
    public MediatorConfigurationBuilder AddAsyncHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>(
        Action<AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse>> configure)
        where THandler : class, IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        Add<IAsyncBaseRequestHandler<TRequest>, THandler, TRequest>(_handlerLifetime);
        Configure(this, configure);
        return this;
    }

    /// <summary>
    /// Регистрирует обработчик вида <see cref="IAsyncRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="lifetime">Жизненный цикл.</param>
    /// <param name="configure">Конфигурирует обработчик запроса.</param>
    /// <returns>Модифицированный контекст конфигурации.</returns>
    public MediatorConfigurationBuilder AddAsyncHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>(
        ServiceLifetime lifetime,
        Action<AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse>> configure)
        where THandler : class, IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        Add<IAsyncBaseRequestHandler<TRequest>, THandler, TRequest>(lifetime);
        Configure(this, configure);
        return this;
    }

    private static void Configure<TRequest>(
        MediatorConfigurationBuilder builder,
        Action<RequestHandlerConfigurationBuilder<TRequest>>? configure)
        where TRequest : notnull, allows ref struct
    {
        if (configure != null)
        {
            var subBuilder = new RequestHandlerConfigurationBuilder<TRequest>(builder.Services, builder._processorLifetime);
            configure(subBuilder);
        }
    }

    private static void Configure<TRequest, TResponse>(
        MediatorConfigurationBuilder builder,
        Action<RequestHandlerConfigurationBuilder<TRequest, TResponse>>? configure)
        where TRequest : notnull, allows ref struct
        where TResponse : notnull, allows ref struct
    {
        if (configure != null)
        {
            var subBuilder = new RequestHandlerConfigurationBuilder<TRequest, TResponse>(builder.Services, builder._processorLifetime);
            configure(subBuilder);
        }
    }

    private static void Configure<TRequest>(
        MediatorConfigurationBuilder builder,
        Action<AsyncRequestHandlerConfigurationBuilder<TRequest>>? configure)
        where TRequest : notnull
    {
        if (configure != null)
        {
            var subBuilder = new AsyncRequestHandlerConfigurationBuilder<TRequest>(builder.Services, builder._processorLifetime);
            configure(subBuilder);
        }
    }

    private static void Configure<TRequest, TResponse>(
        MediatorConfigurationBuilder builder,
        Action<AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse>>? configure)
        where TRequest : notnull
    {
        if (configure != null)
        {
            var subBuilder = new AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse>(builder.Services, builder._processorLifetime);
            configure(subBuilder);
        }
    }

    private void Add<TInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
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
    }
}