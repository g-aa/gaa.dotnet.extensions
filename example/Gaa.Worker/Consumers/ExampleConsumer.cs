using Gaa.Extensions.Observer;
using Gaa.Worker.Messages;

namespace Gaa.Worker.Consumers;

/// <summary>
/// Пример потребителя сообщений.
/// </summary>
public sealed partial class ExampleConsumer : IAsyncConsumer<ExampleMessage>
{
    private readonly ILogger _log;

    /// <summary>
    /// Инициализирует новывый экземпляр класса <see cref="ExampleConsumer"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика для журналов протоколирования собцытий.</param>
    public ExampleConsumer(ILoggerFactory loggerFactory)
    {
        _log = loggerFactory.CreateLogger("Gaa.Worker.Example.Consumer");
    }

    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<ExampleMessage> context, CancellationToken cancellationToken)
    {
        var message = context.Message;
        var processingTime = DateTimeOffset.UtcNow - message.CreationTime;
        Log.Message(_log, message.Id, message.Text, processingTime);
        return Task.CompletedTask;
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Получено очередное сообщение '{Id}:{Text}', время затраченное на обработку '{Time}'.")]
        public static partial void Message(ILogger log, Guid id, string text, TimeSpan time);
    }
}