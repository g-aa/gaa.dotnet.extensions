using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <inheritdoc />
internal sealed class Mediator
    : IMediator
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

    /// <inheritdoc />
    public void Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest
    {
        PreProcess(request, cancellationToken);
        var handler = _provider.GetRequiredService<IRequestHandler<TRequest>>();
        handler.Handle(request, cancellationToken);
    }

    /// <inheritdoc />
    public TResponse Send<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest<TResponse>
    {
        PreProcess(request, cancellationToken);
        var handler = _provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        var response = handler.Handle(request, cancellationToken);
        PostProcess(request, response, cancellationToken);
        return response;
    }

    /// <inheritdoc />
    public async Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest
    {
        await AsyncPreProcess(request, cancellationToken);
        var handler = _provider.GetRequiredService<IAsyncRequestHandler<TRequest>>();
        await handler.HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest<TResponse>
    {
        await AsyncPreProcess(request, cancellationToken);
        var handler = _provider.GetRequiredService<IAsyncRequestHandler<TRequest, TResponse>>();
        var response = await handler.HandleAsync(request, cancellationToken);
        await AsyncPostProcess(request, response, cancellationToken);
        return response;
    }

    private void PreProcess<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var preProcessors = _provider.GetServices<IRequestPreProcessor<TRequest>>();
        foreach (var preProcessor in preProcessors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            preProcessor.Process(request, cancellationToken);
        }
    }

    private void PostProcess<TRequest, TResponse>(
        TRequest request,
        TResponse response,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var postProcessors = _provider.GetServices<IRequestPostProcessor<TRequest, TResponse>>();
        foreach (var postProcessor in postProcessors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            postProcessor.Process(request, response, cancellationToken);
        }
    }

    private async Task AsyncPreProcess<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var preProcessors = _provider.GetServices<IAsyncRequestPreProcessor<TRequest>>();
        foreach (var preProcessor in preProcessors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await preProcessor.ProcessAsync(request, cancellationToken);
        }
    }

    private async Task AsyncPostProcess<TRequest, TResponse>(
        TRequest request,
        TResponse response,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var postProcessors = _provider.GetServices<IAsyncRequestPostProcessor<TRequest, TResponse>>();
        foreach (var postProcessor in postProcessors)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await postProcessor.ProcessAsync(request, response, cancellationToken);
        }
    }
}