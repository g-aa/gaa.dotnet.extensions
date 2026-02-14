namespace Gaa.Extensions.Benchmark.Custom.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования #1.
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
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="mediator">Медиатор.</param>
        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <inheritdoc />
        public Task HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            return _mediator.SendAsync<AsyncWithoutResponse2.Request>(
                new() { Message = request.Message },
                cancellationToken);
        }
    }
}

/// <summary>
/// Кейс для тестирования #2.
/// </summary>
internal static class AsyncWithoutResponse2
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
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="mediator">Медиатор.</param>
        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <inheritdoc />
        public Task HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            return _mediator.SendAsync<AsyncWithoutResponse3.Request>(
                new() { Message = request.Message },
                cancellationToken);
        }
    }
}

/// <summary>
/// Кейс для тестирования #3.
/// </summary>
internal static class AsyncWithoutResponse3
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
        private readonly TextWriter _writer;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="writer">Ввод вывод данных.</param>
        public Handler(TextWriter writer)
        {
            _writer = writer;
        }

        /// <inheritdoc />
        public Task HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync(request.Message);
        }
    }
}