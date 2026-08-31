using Gaa.Extensions.Observer;
using Gaa.Worker.Messages;

namespace Gaa.Worker.Consumers;

/// <summary>
/// Второй потребитель сообщений.
/// </summary>
public sealed partial class SecondConsumer : IAsyncConsumer<SecondMessage>
{
    private readonly ILogger _log;

    /// <summary>
    /// Инициализирует новывый экземпляр класса <see cref="SecondConsumer"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика для журналов протоколирования собцытий.</param>
    public SecondConsumer(ILoggerFactory loggerFactory)
    {
        _log = loggerFactory.CreateLogger("Gaa.Worker.Second.Consumer");
    }

    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<SecondMessage> context, CancellationToken cancellationToken)
    {
        var message = context.Message;
        var processingTime = (DateTimeOffset.UtcNow - message.CreationTime).TotalMicroseconds;
        Log.Message(_log, message.Id, message.Text, processingTime);
        return Task.CompletedTask;
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Получено очередное сообщение '{Id}:{Text}', время затраченное на обработку '{Time} us'.")]
        public static partial void Message(ILogger log, Guid id, string text, double time);
    }
}