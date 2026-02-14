using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Внутренний обработчик коллекции препроцессоров вида <see cref="IAsyncRequestPreProcessor{TRequest}"/>.
/// </summary>
internal sealed class AsyncRequestPreProcessorHandler
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestPreProcessorHandler"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public AsyncRequestPreProcessorHandler(IServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Выполнить препроцессоры для запроса.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="continuation">Делегат продолжения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    public async Task HandleAsync<TRequest>(
        TRequest request,
        Continuation<TRequest, Task> continuation,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var processors = (IEnumerable<IAsyncRequestPreProcessor<TRequest>>)_provider.GetRequiredService(typeof(IEnumerable<IAsyncRequestPreProcessor<TRequest>>));
        foreach (var processor in processors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await processor.ProcessAsync(request, cancellationToken);
        }

        await continuation(_provider, request, cancellationToken);
    }

    /// <summary>
    /// Выполнить препроцессоры для запроса.
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
        var processors = (IEnumerable<IAsyncRequestPreProcessor<TRequest>>)_provider.GetRequiredService(typeof(IEnumerable<IAsyncRequestPreProcessor<TRequest>>));
        foreach (var processor in processors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await processor.ProcessAsync(request, cancellationToken);
        }

        return await continuation(_provider, request, cancellationToken);
    }
}