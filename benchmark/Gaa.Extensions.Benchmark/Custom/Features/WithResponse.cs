namespace Gaa.Extensions.Benchmark.Custom.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования #1.
/// </summary>
internal static class WithResponse
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal readonly ref struct Request : IRequest<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required ReadOnlySpan<char> Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IRequestHandler<Request, Response>
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
        public Response Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            return _mediator.Send<WithResponse2.Request, Response>(
                new() { Message = request.Message },
                cancellationToken);
        }
    }
}

/// <summary>
/// Кейс для тестирования #2.
/// </summary>
internal static class WithResponse2
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal readonly ref struct Request : IRequest<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required ReadOnlySpan<char> Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IRequestHandler<Request, Response>
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
        public Response Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            return _mediator.Send<WithResponse3.Request, Response>(
                new() { Message = request.Message },
                cancellationToken);
        }
    }
}

/// <summary>
/// Кейс для тестирования #3.
/// </summary>
internal static class WithResponse3
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal readonly ref struct Request : IRequest<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required ReadOnlySpan<char> Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IRequestHandler<Request, Response>
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
        public Response Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            _writer.WriteLine(request.Message);
            return new()
            {
                Message = "Output message!",
            };
        }
    }
}