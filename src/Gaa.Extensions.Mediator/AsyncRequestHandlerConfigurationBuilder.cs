using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public class AsyncRequestHandlerConfigurationBuilder<TRequest>
    : MediatorConfigurationBuilder
    where TRequest : notnull
{
    /// <summary>
    /// Регистрирует препроцессор вида <see cref="IAsyncRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TPreProcessor">Тип препроцессора запросов.</typeparam>
    /// <returns>Контекст обработчиков запросов.</returns>
    /// <remarks>Препроцессор регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public AsyncRequestHandlerConfigurationBuilder<TRequest> AddAsyncPreProcessor<TPreProcessor>()
        where TPreProcessor : class, IAsyncRequestPreProcessor<TRequest>
    {
        Services.AddTransient<IAsyncRequestPreProcessor<TRequest>, TPreProcessor>();
        return this;
    }
}

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
public class AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse>
    : MediatorConfigurationBuilder
    where TRequest : notnull
{
    /// <summary>
    /// Регистрирует препроцессор вида <see cref="IAsyncRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TPreProcessor">Тип препроцессора запросов.</typeparam>
    /// <returns>Контекст обработчиков запросов.</returns>
    /// <remarks>Препроцессор регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse> AddAsyncPreProcessor<TPreProcessor>()
        where TPreProcessor : class, IAsyncRequestPreProcessor<TRequest>
    {
        Services.AddTransient<IAsyncRequestPreProcessor<TRequest>, TPreProcessor>();
        return this;
    }

    /// <summary>
    /// Регистрирует постпроцессор вида <see cref="IAsyncRequestPostProcessor{TRequest, TResponse}"/>.
    /// </summary>
    /// <typeparam name="TPostProcessor">Тип постпроцессора запросов.</typeparam>
    /// <returns>Контекст обработчиков запросов.</returns>
    /// <remarks>Постпроцессор регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public AsyncRequestHandlerConfigurationBuilder<TRequest, TResponse> AddAsyncPostProcessor<TPostProcessor>()
        where TPostProcessor : class, IAsyncRequestPostProcessor<TRequest, TResponse>
    {
        Services.AddTransient<IAsyncRequestPostProcessor<TRequest, TResponse>, TPostProcessor>();
        return this;
    }
}