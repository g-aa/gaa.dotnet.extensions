using Requestum;
using Requestum.Contract;

namespace Gaa.Extensions.Benchmark.Requestum.Features;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Кейс для тестирования.
/// </summary>
internal static class WithoutResponse
{
    /// <summary>
    /// Запроса.
    /// </summary>
    internal sealed class Command : ICommand
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from request!";
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : ICommandHandler<Command>
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
        public void Execute(Command command)
        {
            _writer.WriteLine(command.Message);
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
    internal sealed class Command : ICommand
    {
        /// <summary>
        /// Текст с сообщением.
        /// </summary>
        public string Message { get; init; } = "Test message from async request!";
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
            _requestum.Execute<WithoutResponse.Command>(
                new() { Message = command.Message });
            return Task.CompletedTask;
        }
    }
}