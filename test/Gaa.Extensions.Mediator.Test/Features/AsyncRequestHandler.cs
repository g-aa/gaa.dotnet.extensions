namespace Gaa.Extensions.Test.Features;

#pragma warning disable CA1812
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Обработчик запросов.
/// </summary>
internal sealed class AsyncRequestHandlerWithoutResponse
    : IAsyncRequestHandler<AsyncRequestWithoutResponse>
{
    private readonly IMessageLogger _log;

    private readonly IMediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestHandlerWithoutResponse"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    /// <param name="mediator">Медиатор.</param>
    public AsyncRequestHandlerWithoutResponse(
        IMessageLogger log,
        IMediator mediator)
    {
        _log = log;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public Task HandleAsync(
        AsyncRequestWithoutResponse request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        _mediator.Send<RequestWithoutResponse>(
            new() { Message = request.Message },
            cancellationToken);

        return Task.CompletedTask;
    }
}

/// <summary>
/// Обработчик запросов.
/// </summary>
internal sealed class AsyncRequestHandlerWithResponse
    : IAsyncRequestHandler<AsyncRequestWithResponse, Response>
{
    private readonly IMessageLogger _log;

    private readonly IMediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestHandlerWithResponse"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    /// <param name="mediator">Медиатор.</param>
    public AsyncRequestHandlerWithResponse(
        IMessageLogger log,
        IMediator mediator)
    {
        _log = log;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public Task<Response> HandleAsync(
        AsyncRequestWithResponse request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        var response = _mediator.Send<RequestWithResponse, Response>(
            new() { Message = request.Message },
            cancellationToken);

        return Task.FromResult(response);
    }
}