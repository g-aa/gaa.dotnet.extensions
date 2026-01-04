using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public class RequestHandlerConfigurationContext<TRequest>
    : MediatorConfigurationContext
    where TRequest : notnull
{
    /// <summary>
    /// Регистрирует препроцессор вида <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TPreProcessor">Тип препроцессора запросов.</typeparam>
    /// <returns>Контекст обработчиков запросов.</returns>
    /// <remarks>Препроцессор регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public RequestHandlerConfigurationContext<TRequest> AddPreProcessor<TPreProcessor>()
        where TPreProcessor : class, IRequestPreProcessor<TRequest>
    {
        Services.AddTransient<IRequestPreProcessor<TRequest>, TPreProcessor>();
        return this;
    }
}

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public class RequestHandlerConfigurationContext<TRequest, TResponse>
    : MediatorConfigurationContext
    where TRequest : notnull
{
    /// <summary>
    /// Регистрирует препроцессор вида <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TPreProcessor">Тип препроцессора запросов.</typeparam>
    /// <returns>Контекст обработчиков запросов.</returns>
    /// <remarks>Препроцессор регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public RequestHandlerConfigurationContext<TRequest, TResponse> AddPreProcessor<TPreProcessor>()
        where TPreProcessor : class, IRequestPreProcessor<TRequest>
    {
        Services.AddTransient<IRequestPreProcessor<TRequest>, TPreProcessor>();
        return this;
    }

    /// <summary>
    /// Регистрирует постпроцессор вида <see cref="IRequestPostProcessor{TRequest, TResponse}"/>.
    /// </summary>
    /// <typeparam name="TPostProcessor">Тип постпроцессора запросов.</typeparam>
    /// <returns>Контекст обработчиков запросов.</returns>
    /// <remarks>Постпроцессор регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public RequestHandlerConfigurationContext<TRequest, TResponse> AddPostProcessor<TPostProcessor>()
        where TPostProcessor : class, IRequestPostProcessor<TRequest, TResponse>
    {
        Services.AddTransient<IRequestPostProcessor<TRequest, TResponse>, TPostProcessor>();
        return this;
    }
}