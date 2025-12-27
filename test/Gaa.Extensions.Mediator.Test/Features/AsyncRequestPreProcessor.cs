namespace Gaa.Extensions.Test.Features;

#pragma warning disable CA1812

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
internal sealed class AsyncRequestPreProcessor
    : IAsyncRequestPreProcessor<AsyncRequestWithResponse>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestPreProcessor"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public AsyncRequestPreProcessor(
        IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public Task ProcessAsync(
        AsyncRequestWithResponse request,
        CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        return Task.CompletedTask;
    }
}