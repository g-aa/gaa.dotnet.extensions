namespace Gaa.Extensions.Benchmark.Features;

#pragma warning disable CA1812

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
        public Task<Response> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var response = _mediator.Send<WithResponse.Request, Response>(
                new() { Message = request.Message },
                cancellationToken);

            return Task.FromResult(response);
        }
    }
}