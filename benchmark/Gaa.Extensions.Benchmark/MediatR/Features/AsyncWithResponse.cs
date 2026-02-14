namespace Gaa.Extensions.Benchmark.MediatR.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования #1.
/// </summary>
internal static class AsyncWithResponse
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal sealed class Request : global::MediatR.IRequest<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from async request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : global::MediatR.IRequestHandler<Request, Response>
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
        public Task<Response> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            return _mediator.Send(
                new AsyncWithResponse2.Request { Message = request.Message },
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
    internal sealed class Request : global::MediatR.IRequest<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from async request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : global::MediatR.IRequestHandler<Request, Response>
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
        public Task<Response> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            return _mediator.Send(
                new AsyncWithResponse3.Request { Message = request.Message },
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
    internal sealed class Request : global::MediatR.IRequest<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : global::MediatR.IRequestHandler<Request, Response>
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
        public async Task<Response> Handle(
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