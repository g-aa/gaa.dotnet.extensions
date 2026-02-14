using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Внутренний обработчик коллекции постпроцессоров вида <see cref="IRequestPostProcessor{TRequest, TResponse}"/>.
/// </summary>
internal sealed class RequestPostProcessorHandler
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestPostProcessorHandler"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public RequestPostProcessorHandler(IServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Выполнить постпроцессоры для запроса.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="continuation">Делегат продолжения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    public TResponse Handle<TRequest, TResponse>(
        TRequest request,
        Continuation<TRequest, TResponse> continuation,
        CancellationToken cancellationToken)
        where TRequest : notnull, allows ref struct
        where TResponse : allows ref struct
    {
        var response = continuation(_provider, request, cancellationToken);
        var processors = (IEnumerable<IRequestPostProcessor<TRequest, TResponse>>)_provider.GetRequiredService(typeof(IEnumerable<IRequestPostProcessor<TRequest, TResponse>>));
        foreach (var processor in processors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            processor.Process(request, response, cancellationToken);
        }

        return response;
    }
}