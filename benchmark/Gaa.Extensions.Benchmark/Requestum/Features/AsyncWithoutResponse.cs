using Requestum;
using Requestum.Contract;

namespace Gaa.Extensions.Benchmark.Requestum.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования #1.
/// </summary>
internal static class AsyncWithoutResponse
{
    /// <summary>
    /// Запроса.
    /// </summary>
    internal sealed class Command : ICommand
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IAsyncCommandHandler<Command>
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
        public Task ExecuteAsync(
            Command command,
            CancellationToken cancellationToken = default)
        {
            return _requestum.ExecuteAsync<AsyncWithoutResponse2.Command>(
                new() { Message = command.Message },
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
    internal sealed class Command : ICommand
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IAsyncCommandHandler<Command>
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
        public Task ExecuteAsync(
            Command command,
            CancellationToken cancellationToken = default)
        {
            return _requestum.ExecuteAsync<AsyncWithoutResponse3.Command>(
                new() { Message = command.Message },
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
    internal sealed class Command : ICommand
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : IAsyncCommandHandler<Command>
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
        public Task ExecuteAsync(
            Command command,
            CancellationToken cancellationToken = default)
        {
            return _writer.WriteLineAsync(command.Message);
        }
    }
}