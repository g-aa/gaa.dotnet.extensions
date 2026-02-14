namespace Gaa.Extensions.Benchmark.Custom.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования #1.
/// </summary>
internal static class AsyncWithResponse
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal sealed class Request : IAsyncRequest<AsyncResponse>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IAsyncRequestHandler<Request, AsyncResponse>
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
        public Task<AsyncResponse> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            return _mediator.SendAsync<AsyncWithResponse2.Request, AsyncResponse>(
                new() { Message = request.Message },
                cancellationToken);
        }
    }
}

/// <summary>
/// Кейс для тестирования #2.
/// </summary>
internal static class AsyncWithResponse2
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal sealed class Request : IAsyncRequest<AsyncResponse>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IAsyncRequestHandler<Request, AsyncResponse>
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
        public Task<AsyncResponse> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            return _mediator.SendAsync<AsyncWithResponse3.Request, AsyncResponse>(
                new() { Message = request.Message },
                cancellationToken);
        }
    }
}

/// <summary>
/// Кейс для тестирования #3.
/// </summary>
internal static class AsyncWithResponse3
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal sealed class Request : IAsyncRequest<AsyncResponse>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IAsyncRequestHandler<Request, AsyncResponse>
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
        public async Task<AsyncResponse> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            await _writer.WriteLineAsync(request.Message);
            return new()
            {
                Message = "Output message!",
            };
        }
    }
}