using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Внутренний обработчик коллекции постпроцессоров вида <see cref="IAsyncRequestPostProcessor{TRequest, TResponse}"/>.
/// </summary>
internal class AsyncRequestPostProcessorHandler
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
        var processors = (IEnumerable<IAsyncRequestPostProcessor<TRequest, TResponse>>)_provider.GetRequiredService(typeof(IEnumerable<IAsyncRequestPostProcessor<TRequest, TResponse>>));
        foreach (var processor in processors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await processor.ProcessAsync(request, response, cancellationToken);
        }

        return response;
    }
}