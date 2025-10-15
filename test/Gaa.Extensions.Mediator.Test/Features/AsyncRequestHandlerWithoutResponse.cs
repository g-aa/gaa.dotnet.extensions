namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Обработчик запросов.
/// </summary>
public class AsyncRequestHandlerWithoutResponse
    : IAsyncRequestHandler<ExampleRequest>
{
    private readonly IMessageLogger _log;

    private readonly Mediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestHandlerWithoutResponse"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    /// <param name="mediator">Медиатор.</param>
    public AsyncRequestHandlerWithoutResponse(IMessageLogger log, Mediator mediator)
    {
        _log = log;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public Task HandleAsync(
        ExampleRequest request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        _mediator.Send(request, cancellationToken);
        return Task.CompletedTask;
    }
}