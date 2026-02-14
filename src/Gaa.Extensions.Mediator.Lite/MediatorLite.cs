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
    public void Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest, allows ref struct
    {
        var handler = (IRequestHandler<TRequest>)_provider.GetRequiredService(typeof(IRequestHandler<TRequest>));
        handler.Handle(request, cancellationToken);
    }

    /// <inheritdoc />
    public TResponse Send<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IRequest<TResponse>, allows ref struct
        where TResponse : allows ref struct
    {
        var handler = (IRequestHandler<TRequest, TResponse>)_provider.GetRequiredService(typeof(IRequestHandler<TRequest, TResponse>));
        return handler.Handle(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task SendAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest
    {
        var handler = (IAsyncRequestHandler<TRequest>)_provider.GetRequiredService(typeof(IAsyncRequestHandler<TRequest>));
        return handler.HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : notnull, IAsyncRequest<TResponse>
    {
        var handler = (IAsyncRequestHandler<TRequest, TResponse>)_provider.GetRequiredService(typeof(IAsyncRequestHandler<TRequest, TResponse>));
        return handler.HandleAsync(request, cancellationToken);
    }
}