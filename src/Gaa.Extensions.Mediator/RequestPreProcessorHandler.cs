using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Внутренний обработчик коллекции препроцессоров вида <see cref="IRequestPreProcessor{TRequest}"/>.
/// </summary>
internal sealed class RequestPreProcessorHandler
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestPreProcessorHandler"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public RequestPreProcessorHandler(IServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Выполняет препроцессоры для запроса.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="continuation">Делегат продолжения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public void Handle<TRequest>(
        TRequest request,
        Continuation<TRequest> continuation,
        CancellationToken cancellationToken)
        where TRequest : notnull, allows ref struct
    {
        HandleCore(request, cancellationToken);
        continuation(_provider, request, cancellationToken);
    }

    /// <summary>
    /// Выполняет препроцессоры для запроса.
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
        HandleCore(request, cancellationToken);
        return continuation(_provider, request, cancellationToken);
    }

    private void HandleCore<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, allows ref struct
    {
        var processors = (IEnumerable<IRequestPreProcessor<TRequest>>)_provider.GetRequiredService(typeof(IEnumerable<IRequestPreProcessor<TRequest>>));
        foreach (var processor in processors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            processor.Process(request, cancellationToken);
        }
    }
}