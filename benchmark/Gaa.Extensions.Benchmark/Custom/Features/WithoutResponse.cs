namespace Gaa.Extensions.Benchmark.Custom.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования.
/// </summary>
internal static class WithoutResponse
{
    /// <summary>
    /// Запроса.
    /// </summary>
    internal sealed class Request : IRequest
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IRequestHandler<Request>
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
        public void Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            _writer.WriteLine(request.Message);
        }
    }
}

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
        public string Message { get; init; } = "Test message from async request!";
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
            _mediator.Send<WithoutResponse.Request>(
                new() { Message = request.Message },
                cancellationToken);

            return Task.CompletedTask;
        }
    }
}