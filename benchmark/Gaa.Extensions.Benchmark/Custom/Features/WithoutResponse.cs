namespace Gaa.Extensions.Benchmark.Custom.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования #1.
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
        public void Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            _mediator.Send<WithoutResponse2.Request>(
                new() { Message = request.Message },
                cancellationToken);
        }
    }
}

/// <summary>
/// Кейс для тестирования #2.
/// </summary>
internal static class WithoutResponse2
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
        public void Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            _mediator.Send<WithoutResponse3.Request>(
                new() { Message = request.Message },
                cancellationToken);
        }
    }
}

/// <summary>
/// Кейс для тестирования #3.
/// </summary>
internal static class WithoutResponse3
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