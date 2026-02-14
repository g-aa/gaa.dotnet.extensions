using Requestum;
using Requestum.Contract;

namespace Gaa.Extensions.Benchmark.Requestum.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования #1.
/// </summary>
internal static class WithResponse
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal sealed class Query : IQuery<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IQueryHandler<Query, Response>
    {
        private readonly IRequestum _requestum;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="requestum">Медиатор.</param>
        public Handler(IRequestum requestum)
        {
            _requestum = requestum;
        }

        /// <inheritdoc />
        public Response Handle(Query query)
        {
            return _requestum.Handle<WithResponse2.Query, Response>(
                new() { Message = query.Message });
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
    internal sealed class Query : IQuery<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IQueryHandler<Query, Response>
    {
        private readonly IRequestum _requestum;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Handler"/>.
        /// </summary>
        /// <param name="requestum">Медиатор.</param>
        public Handler(IRequestum requestum)
        {
            _requestum = requestum;
        }

        /// <inheritdoc />
        public Response Handle(Query query)
        {
            return _requestum.Handle<WithResponse3.Query, Response>(
                new() { Message = query.Message });
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
    internal sealed class Query : IQuery<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IQueryHandler<Query, Response>
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
        public Response Handle(Query query)
        {
            _writer.WriteLine(query.Message);
            return new()
            {
                Message = "Output message!",
            };
        }
    }
}