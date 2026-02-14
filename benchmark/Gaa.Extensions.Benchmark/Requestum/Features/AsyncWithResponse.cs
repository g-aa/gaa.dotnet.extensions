using Requestum;
using Requestum.Contract;

namespace Gaa.Extensions.Benchmark.Requestum.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования #1.
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
            return _requestum.HandleAsync<AsyncWithResponse2.Query, Response>(
                new() { Message = query.Message },
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
            return _requestum.HandleAsync<AsyncWithResponse3.Query, Response>(
                new() { Message = query.Message },
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
        public async Task<Response> HandleAsync(
            Query query,
            CancellationToken cancellationToken = default)
        {
            await _writer.WriteLineAsync(query.Message);
            return new()
            {
                Message = "Output message!",
            };
        }
    }
}