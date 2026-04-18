namespace Gaa.Extensions;

/// <summary>
/// Внутренний обработчик коллекции постпроцессоров вида <see cref="IAsyncRequestPostProcessor{TRequest, TResponse}"/>.
/// </summary>
internal readonly struct AsyncRequestPostProcessorHandler
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestPostProcessorHandler"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public AsyncRequestPostProcessorHandler(IServiceProvider provider)
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
    public async Task<TResponse> HandleAsync<TRequest, TResponse>(
        TRequest request,
        Continuation<TRequest, Task<TResponse>> continuation,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var response = await continuation(_provider, request, cancellationToken);
        var processors = _provider.GetServices<IAsyncRequestPostProcessor<TRequest, TResponse>>();
        for (var i = 0; i < processors.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await processors[i].ProcessAsync(request, response, cancellationToken);
        }

        return response;
    }
}