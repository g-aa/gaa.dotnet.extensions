using System.Diagnostics;

using Gaa.Extensions.Observer;

namespace Gaa.Worker.Single;

/// <summary>
/// Пример потребителя сообщений.
/// </summary>
public sealed partial class SingleExampleConsumer : IAsyncConsumer<SingleExampleMessage>
{
    private readonly ILogger _log;

    private readonly SingleExampleMetrics _metrics;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SingleExampleConsumer"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика для журналов протоколирования событий.</param>
    /// <param name="metrics">Пример метрик приложения.</param>
    public SingleExampleConsumer(ILoggerFactory loggerFactory, SingleExampleMetrics metrics)
    {
        _log = loggerFactory.CreateLogger("Gaa.Worker.Example.Single.Consumer");
        _metrics = metrics;
    }

    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<SingleExampleMessage> context, CancellationToken cancellationToken)
    {
        var message = context.Message;
        var processingTime = Stopwatch.GetElapsedTime(message.CreationTime).TotalMicroseconds;
        Log.Message(_log,  message.Text, processingTime);
        _metrics.Record(processingTime);
        return Task.CompletedTask;
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Получено очередное сообщение '{Text}', время затраченное на обработку '{Time} us'.")]
        public static partial void Message(ILogger log, string text, double time);
    }
}