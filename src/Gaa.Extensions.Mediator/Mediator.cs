using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Медиатор, посредник.
/// </summary>
public sealed class Mediator
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Mediator"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Отправить синхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    public void Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest
    {
        PreProcess(request, cancellationToken);
        var handler = _provider.GetRequiredService<IRequestHandler<TRequest>>();
        handler.Handle(request, cancellationToken);
    }

    /// <summary>
    /// Отправить синхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    public TResponse Send<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest<TResponse>
    {
        PreProcess(request, cancellationToken);
        var handler = _provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return handler.Handle(request, cancellationToken);
    }

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    public async Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest
    {
        await AsyncPreProcess(request, cancellationToken);
        var handler = _provider.GetRequiredService<IAsyncRequestHandler<TRequest>>();
        await handler.HandleAsync(request, cancellationToken);
    }

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ на запрос.</returns>
    /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
    public async Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest<TResponse>
    {
        await AsyncPreProcess(request, cancellationToken);
        var handler = _provider.GetRequiredService<IAsyncRequestHandler<TRequest, TResponse>>();
        return await handler.HandleAsync(request, cancellationToken);
    }

    private void PreProcess<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var preProcessors = _provider.GetServices<IRequestPreProcessor<TRequest>>();
        foreach (var preProcessor in preProcessors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            preProcessor.Process(request, cancellationToken);
        }
    }

    private async Task AsyncPreProcess<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var preProcessors = _provider.GetServices<IAsyncRequestPreProcessor<TRequest>>();
        foreach (var preProcessor in preProcessors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await preProcessor.ProcessAsync(request, cancellationToken);
        }
    }
}