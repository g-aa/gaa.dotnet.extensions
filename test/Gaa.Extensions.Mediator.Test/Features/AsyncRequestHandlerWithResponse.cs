namespace Gaa.Extensions.Test.Features;

#pragma warning disable CA1812

/// <summary>
/// Обработчик запросов.
/// </summary>
internal sealed class AsyncRequestHandlerWithResponse
    : IAsyncRequestHandler<ExampleRequestWithResponse, ExampleResponse>
{
    private readonly IMessageLogger _log;

    private readonly Mediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestHandlerWithResponse"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    /// <param name="mediator">Медиатор.</param>
    public AsyncRequestHandlerWithResponse(
        IMessageLogger log,
        Mediator mediator)
    {
        _log = log;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public Task<ExampleResponse> HandleAsync(
        ExampleRequestWithResponse request,
        CancellationToken cancellationToken)
    {
        var e = GetType().FullName;

        _log.Log($"{e}: содержимое сообщения {request.Message}.");
        var response = _mediator.Send<ExampleRequestWithResponse, ExampleResponse>(request, cancellationToken);
        return Task.FromResult(response);
    }
}