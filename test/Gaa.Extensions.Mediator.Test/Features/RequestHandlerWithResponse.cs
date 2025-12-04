namespace Gaa.Extensions.Test.Features;

#pragma warning disable CA1812

/// <summary>
/// Обработчик запросов.
/// </summary>
internal sealed class RequestHandlerWithResponse
    : IRequestHandler<ExampleRequestWithResponse, ExampleResponse>
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
    public ExampleResponse Handle(
        ExampleRequestWithResponse request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        return new ExampleResponse
        {
            Message = "Output message!",
        };
    }
}