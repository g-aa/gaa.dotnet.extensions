namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Кейс для тестирования.
/// </summary>
internal static class AsyncWithoutResponse
{
    /// <summary>
    /// Запроса.
    /// </summary>
    internal sealed class Request : IAsyncRequest
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IAsyncRequestHandler<Request>
    {
        private readonly IMessageLogger _log;

        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="log">Журнал регистрации сообщений.</param>
        /// <param name="mediator">Медиатор.</param>
        public Handler(
            IMessageLogger log,
            IMediator mediator)
        {
            _log = log;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public Task HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
            _mediator.Send<WithoutResponse.Request>(
                new() { Message = request.Message },
                cancellationToken);

            return Task.CompletedTask;
        }
    }
}