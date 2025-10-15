namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
public class AsyncRequestPreProcessor
    : IAsyncRequestPreProcessor<ExampleRequestWithResponse>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestPreProcessor"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public AsyncRequestPreProcessor(IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public Task ProcessAsync(
        ExampleRequestWithResponse request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        return Task.CompletedTask;
    }
}