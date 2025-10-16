using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public class AsyncRequestHandlerConfigurationContext<TRequest>
    : MediatorConfigurationContext
    where TRequest : notnull
{
    /// <summary>
    /// Регистрирует предварительный обработчик вида <see cref="IAsyncRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TPreProcessor">Тип предварительного обработчика запросов.</typeparam>
    /// <returns>Контекст обработчиков запросов.</returns>
    /// <remarks>Обработчик регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public AsyncRequestHandlerConfigurationContext<TRequest> AddAsyncPreProcessor<TPreProcessor>()
        where TPreProcessor : class, IAsyncRequestPreProcessor<TRequest>
    {
        Services.AddTransient<IAsyncRequestPreProcessor<TRequest>, TPreProcessor>();
        return this;
    }
}