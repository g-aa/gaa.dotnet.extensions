namespace Gaa.Extensions.Benchmark.MediatR.Features;

#pragma warning disable CA1849
#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования.
/// </summary>
internal static class WithoutResponse
{
    /// <summary>
    /// Запроса.
    /// </summary>
    internal sealed class Request : global::MediatR.IRequest
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : global::MediatR.IRequestHandler<Request>
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
        public Task Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            _writer.WriteLine(request.Message);

            return Task.CompletedTask;
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
    internal sealed class Request : global::MediatR.IRequest
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from async request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : global::MediatR.IRequestHandler<Request>
    {
        private readonly global::MediatR.IMediator _mediator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="mediator">Медиатор.</param>
        public Handler(global::MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <inheritdoc />
        public Task Handle(
            Request request,
            CancellationToken cancellationToken)
        {
           return _mediator.Send(
                new WithoutResponse.Request() { Message = request.Message },
                cancellationToken);
        }
    }
}