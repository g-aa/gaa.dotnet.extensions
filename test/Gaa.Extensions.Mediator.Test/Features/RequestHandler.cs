namespace Gaa.Extensions.Test.Features;

#pragma warning disable CA1812
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Обработчик запросов.
/// </summary>
internal sealed class RequestHandlerWithoutResponse
    : IRequestHandler<RequestWithoutResponse>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestHandlerWithoutResponse"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public RequestHandlerWithoutResponse(
        IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public void Handle(
        RequestWithoutResponse request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
    }
}

/// <summary>
/// Обработчик запросов.
/// </summary>
internal sealed class RequestHandlerWithResponse
    : IRequestHandler<RequestWithResponse, Response>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestHandlerWithResponse"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public RequestHandlerWithResponse(
        IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public Response Handle(
        RequestWithResponse request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        return new Response
        {
            Message = "Output message!",
        };
    }
}