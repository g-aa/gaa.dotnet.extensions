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
        _provider
            .GetRequiredService<IRequestHandler<TRequest>, IBaseRequestHandler<TRequest>>()
            .Handle(request, cancellationToken);
    }

    /// <inheritdoc />
    public TResponse RequiredSend<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest<TResponse>, allows ref struct
        where TResponse : allows ref struct
    {
        return _provider
            .GetRequiredService<IRequestHandler<TRequest, TResponse>, IBaseRequestHandler<TRequest>>()
            .Handle(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task RequiredSendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest
    {
        return _provider
            .GetRequiredService<IAsyncRequestHandler<TRequest>, IAsyncBaseRequestHandler<TRequest>>()
            .HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResponse> RequiredSendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest<TResponse>
    {
        return _provider
            .GetRequiredService<IAsyncRequestHandler<TRequest, TResponse>, IAsyncBaseRequestHandler<TRequest>>()
            .HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public void Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest, allows ref struct
    {
        var handler = _provider.GetService<IRequestHandler<TRequest>, IBaseRequestHandler<TRequest>>();
        handler?.Handle(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest
    {
        var handler = _provider.GetService<IAsyncRequestHandler<TRequest>, IAsyncBaseRequestHandler<TRequest>>();
        return handler != null ? handler.HandleAsync(request, cancellationToken) : Task.CompletedTask;
    }
}