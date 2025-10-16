using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Контекст обработчика запросов.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
public sealed class RequestHandlerConfigurationContext<TRequest>
    : MediatorConfigurationContext
    where TRequest : notnull
{
    /// <summary>
    /// Регистрирует предварительный обработчик вида <see cref="IRequestPreProcessor{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TPreProcessor">Тип предварительного обработчика запросов.</typeparam>
    /// <returns>Контекст обработчиков запросов.</returns>
    /// <remarks>Обработчик регистрируются с временем жизни <see cref="ServiceLifetime.Transient"/>.</remarks>
    public RequestHandlerConfigurationContext<TRequest> AddPreProcessor<TPreProcessor>()
        where TPreProcessor : class, IRequestPreProcessor<TRequest>
    {
        Services.AddTransient<IRequestPreProcessor<TRequest>, TPreProcessor>();
        return this;
    }
}