namespace Gaa.Extensions.Mediator.Test.Features;

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
internal sealed class AsyncRequestPreProcessor : IAsyncRequestPreProcessor<AsyncWithResponse.Request>
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
    public Task ProcessAsync(AsyncWithResponse.Request request, CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        return Task.CompletedTask;
    }
}

/// <summary>
/// Постпроцессор запросов.
/// </summary>
internal sealed class AsyncRequestPostProcessor : IAsyncRequestPostProcessor<AsyncWithResponse.Request, Response>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestPostProcessor"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public AsyncRequestPostProcessor(IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public Task ProcessAsync(AsyncWithResponse.Request request, Response response, CancellationToken cancellationToken)
    {
        _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        _log.Log($"{GetType().FullName}: содержимое сообщения {response.Message}.");
        return Task.CompletedTask;
    }
}