namespace Gaa.Extensions.Test.Features;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
internal sealed class RequestPreProcessor
    : IRequestPreProcessor<WithResponse.Request>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestPreProcessor"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public RequestPreProcessor(
        IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public void Process(
        WithResponse.Request request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
    }
}

/// <summary>
/// Постпроцессор запросов.
/// </summary>
internal sealed class RequestPostProcessor
    : IRequestPostProcessor<WithResponse.Request, Response>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestPostProcessor"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public RequestPostProcessor(
        IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public void Process(
        WithResponse.Request request,
        Response response,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        _log.Log($"{GetType().FullName}: содержимое сообщения {response.Message}.");
    }
}