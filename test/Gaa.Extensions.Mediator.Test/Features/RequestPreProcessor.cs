namespace Gaa.Extensions.Test.Features;

#pragma warning disable CA1812

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
internal sealed class RequestPreProcessor
    : IRequestPreProcessor<ExampleRequestWithResponse>
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
        ExampleRequestWithResponse request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
    }
}