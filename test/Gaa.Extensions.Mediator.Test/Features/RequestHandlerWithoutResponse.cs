namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Обработчик запросов.
/// </summary>
public class RequestHandlerWithoutResponse
    : IRequestHandler<ExampleRequest>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestHandlerWithoutResponse"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public RequestHandlerWithoutResponse(IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public void Handle(
        ExampleRequest request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
    }
}