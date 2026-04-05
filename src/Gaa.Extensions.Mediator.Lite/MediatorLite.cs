using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <inheritdoc />
internal sealed class MediatorLite : IMediator
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MediatorLite"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public MediatorLite(IServiceProvider provider)
    {
        _provider = provider;
    }

    /// <inheritdoc />
    public void RequiredSend<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest, allows ref struct
    {
        var handler = (IRequestHandler<TRequest>)_provider.GetRequiredService(typeof(IBaseRequestHandler<TRequest>));
        handler.Handle(request, cancellationToken);
    }

    /// <inheritdoc />
    public TResponse RequiredSend<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest<TResponse>, allows ref struct
        where TResponse : allows ref struct
    {
        var handler = (IRequestHandler<TRequest, TResponse>)_provider.GetRequiredService(typeof(IBaseRequestHandler<TRequest>));
        return handler.Handle(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task RequiredSendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest
    {
        var handler = (IAsyncRequestHandler<TRequest>)_provider.GetRequiredService(typeof(IAsyncBaseRequestHandler<TRequest>));
        return handler.HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResponse> RequiredSendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest<TResponse>
    {
        var handler = (IAsyncRequestHandler<TRequest, TResponse>)_provider.GetRequiredService(typeof(IAsyncBaseRequestHandler<TRequest>));
        return handler.HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public void Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest, allows ref struct
    {
        var handler = (IRequestHandler<TRequest>?)_provider.GetService(typeof(IBaseRequestHandler<TRequest>));
        handler?.Handle(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest
    {
        var handler = (IAsyncRequestHandler<TRequest>?)_provider.GetService(typeof(IAsyncBaseRequestHandler<TRequest>));
        return handler != null ? handler.HandleAsync(request, cancellationToken) : Task.CompletedTask;
    }
}