using Requestum;
using Requestum.Contract;

namespace Gaa.Extensions.Benchmark.Requestum.Features;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Кейс для тестирования.
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
        public string Message { get; init; } = "Test message from request!";
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
            return new Response
            {
                Message = "Output message!",
            };
        }
    }
}

/// <summary>
/// Кейс для тестирования.
/// </summary>
internal static class AsyncWithResponse
{
    /// <summary>
    /// Пример запроса.
    /// </summary>
    internal sealed class Query : IQuery<Response>
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from async request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IAsyncQueryHandler<Query, Response>
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
        public Task<Response> HandleAsync(
            Query query,
            CancellationToken cancellationToken = default)
        {
            var response = _requestum.Handle<WithResponse.Query, Response>(
                new() { Message = query.Message });
            return Task.FromResult(response);
        }
    }
}