namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Кейс для тестирования.
/// </summary>
internal static class AsyncWithResponse
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal sealed class Request : IAsyncRequest<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IMessageLogger _log;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="log">Журнал регистрации сообщений.</param>
        public Handler(IMessageLogger log)
        {
            _log = log;
        }

        /// <inheritdoc />
        public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
            return Task.FromResult(new Response()
            {
                Message = "Output message!",
            });
        }
    }
}