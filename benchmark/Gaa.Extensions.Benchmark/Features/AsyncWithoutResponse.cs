namespace Gaa.Extensions.Benchmark.Features;

#pragma warning disable CA1812

/// <summary>
/// Кейс для тестирования.
/// </summary>
internal static class AsyncWithoutResponse
{
    /// <summary>
    /// Запроса.
    /// </summary>
    internal sealed class Request
        : IAsyncRequest
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
        : IAsyncRequestHandler<Request>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="mediator">Медиатор.</param>
        public Handler(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <inheritdoc />
        public Task HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            _mediator.Send<WithoutResponse.Request>(
                new() { Message = request.Message },
                cancellationToken);

            return Task.CompletedTask;
        }
    }
}