using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <inheritdoc />
internal sealed class Mediator : IMediator
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
        Continuation<TRequest> func = (p, r, t) => p.GetRequiredService<IRequestHandler<TRequest>>().Handle(r, t);
        new RequestPreProcessorHandler(_provider).Handle(request, func, cancellationToken);
    }

    /// <inheritdoc />
    public TResponse Send<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest<TResponse>
    {
        Continuation<TRequest, TResponse> func = (p, r, t) => p.GetRequiredService<IRequestHandler<TRequest, TResponse>>().Handle(r, t);
        return new RequestPreProcessorHandler(_provider)
            .Handle(request, (p, r, t) => new RequestPostProcessorHandler(p).Handle(r, func, t), cancellationToken);
    }

    /// <inheritdoc />
    public Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest
    {
        Continuation<TRequest, Task> func = (p, r, t) => p.GetRequiredService<IAsyncRequestHandler<TRequest>>().HandleAsync(r, t);
        return new AsyncRequestPreProcessorHandler(_provider).HandleAsync(request, func, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest<TResponse>
    {
        Continuation<TRequest, Task<TResponse>> func = (p, r, t) => p.GetRequiredService<IAsyncRequestHandler<TRequest, TResponse>>().HandleAsync(r, t);
        return new AsyncRequestPreProcessorHandler(_provider)
            .HandleAsync(request, (p, r, t) => new AsyncRequestPostProcessorHandler(p).HandleAsync(r, func, t), cancellationToken);
    }
}