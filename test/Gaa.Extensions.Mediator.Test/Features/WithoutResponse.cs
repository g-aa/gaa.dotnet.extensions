namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Кейс для тестирования.
/// </summary>
internal static class WithoutResponse
{
    /// <summary>
    /// Запроса.
    /// </summary>
    internal sealed class Request
        : IRequest
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler
        : IRequestHandler<Request>
    {
        private readonly IMessageLogger _log;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="log">Журнал регистрации сообщений.</param>
        public Handler(
            IMessageLogger log)
        {
            _log = log;
        }

        /// <inheritdoc />
        public void Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            _log.Log($"{GetType().FullName}: содержимое сообщения {request.Message}.");
        }
    }
}