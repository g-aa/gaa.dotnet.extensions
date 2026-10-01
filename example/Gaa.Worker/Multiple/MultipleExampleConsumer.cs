using System.Diagnostics;

using Gaa.Extensions.Observer;

namespace Gaa.Worker.Multiple;

/// <summary>
/// Пример потребителя сообщений.
/// </summary>
public sealed partial class MultipleExampleConsumer : IAsyncConsumer<MultipleExampleMessage>
{
    private readonly ILogger _log;

    private readonly MultipleExampleMetrics _metrics;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MultipleExampleConsumer"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика для журналов протоколирования событий.</param>
    /// <param name="metrics">Пример метрик приложения.</param>
    public MultipleExampleConsumer(ILoggerFactory loggerFactory, MultipleExampleMetrics metrics)
    {
        _log = loggerFactory.CreateLogger("Gaa.Worker.Example.Multiple.Consumer");
        _metrics = metrics;
    }

    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<MultipleExampleMessage> context, CancellationToken cancellationToken)
    {
        var message = context.Message;
        var processingTime = Stopwatch.GetElapsedTime(message.CreationTime).TotalMicroseconds;
        Log.Message(_log, message.Text, processingTime);
        _metrics.Record(processingTime);
        return Task.CompletedTask;
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Получено очередное сообщение '{Text}', время затраченное на обработку '{Time} us'.")]
        public static partial void Message(ILogger log, string text, double time);
    }
}