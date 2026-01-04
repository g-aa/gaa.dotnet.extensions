namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Кейс для тестирования.
/// </summary>
internal static class AsyncWithResponse
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal sealed class Request
        : IAsyncRequest<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from async request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler
        : IAsyncRequestHandler<Request, Response>
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
        public Task<Response> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
            var response = _mediator.Send<WithResponse.Request, Response>(
                new() { Message = request.Message },
                cancellationToken);

            return Task.FromResult(response);
        }
    }
}