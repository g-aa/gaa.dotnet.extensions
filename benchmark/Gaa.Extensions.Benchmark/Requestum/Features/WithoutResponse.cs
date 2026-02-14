using Requestum;
using Requestum.Contract;

namespace Gaa.Extensions.Benchmark.Requestum.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Кейс для тестирования #1.
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
        public required string Message { get; init; }
    }

    /// <summary>
    /// Обработчик запросов.
    /// </summary>
    internal sealed class Handler : ICommandHandler<Command>
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
        public void Execute(Command command)
        {
            _requestum.Execute<WithoutResponse2.Command>(
                new() { Message = command.Message });
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
    internal sealed class Handler : ICommandHandler<Command>
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
        public void Execute(Command command)
        {
            _requestum.Execute<WithoutResponse3.Command>(
                new() { Message = command.Message });
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