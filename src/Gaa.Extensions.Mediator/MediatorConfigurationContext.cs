using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Контекст <see cref="Mediator"/> для конфигурирования.
/// </summary>
public class MediatorConfigurationContext
{
    /// <summary>
    /// Коллекция сервисов.
    /// </summary>
    public IServiceCollection Services { get; init; } = null!;

    /// <summary>
    /// Регистрирует обработчик вида <see cref="IRequestHandler{TRequest}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    /// <remarks>Обработчик регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public RequestHandlerConfigurationContext<TRequest> AddHandle<THandler, TRequest>()
        where THandler : class, IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        IsSingleUse<IRequestHandler<TRequest>, TRequest>();
        return new RequestHandlerConfigurationContext<TRequest>
        {
            Services = Services.AddTransient<IRequestHandler<TRequest>, THandler>(),
        };
    }

    /// <summary>
    /// Регистрирует обработчик вида <see cref="IRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    /// <remarks>Обработчик регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public RequestHandlerConfigurationContext<TRequest> AddHandle<THandler, TRequest, TResponse>()
        where THandler : class, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        IsSingleUse<IRequestHandler<TRequest, TResponse>, TRequest>();
        return new RequestHandlerConfigurationContext<TRequest>
        {
            Services = Services.AddTransient<IRequestHandler<TRequest, TResponse>, THandler>(),
        };
    }

    /// <summary>
    /// Регистрирует асинхронный обработчик вида <see cref="IAsyncRequestHandler{TRequest}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    /// <remarks>Обработчик регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public AsyncRequestHandlerConfigurationContext<TRequest> AddAsyncHandle<THandler, TRequest>()
        where THandler : class, IAsyncRequestHandler<TRequest>
        where TRequest : IRequest
    {
        IsSingleUse<IAsyncRequestHandler<TRequest>, TRequest>();
        return new AsyncRequestHandlerConfigurationContext<TRequest>
        {
            Services = Services.AddTransient<IAsyncRequestHandler<TRequest>, THandler>(),
        };
    }

    /// <summary>
    /// Регистрирует асинхронный обработчик вида <see cref="IAsyncRequestHandler{TRequest, TResponse}"/> в коллекции сервисов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика запросов.</typeparam>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    /// <remarks>Обработчик регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public AsyncRequestHandlerConfigurationContext<TRequest> AddAsyncHandle<THandler, TRequest, TResponse>()
        where THandler : class, IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        IsSingleUse<IAsyncRequestHandler<TRequest, TResponse>, TRequest>();
        return new AsyncRequestHandlerConfigurationContext<TRequest>
        {
            Services = Services.AddTransient<IAsyncRequestHandler<TRequest, TResponse>, THandler>(),
        };
    }

    private void IsSingleUse<THandler, TRequest>()
        where THandler : class
        where TRequest : notnull
    {
        if (Services.Any(e => e.ServiceType == typeof(THandler)))
        {
            var requestName = typeof(TRequest).Name;
            throw new InvalidOperationException($"Для запроса {requestName} можно добавить только один обработчик!");
        }
    }
}