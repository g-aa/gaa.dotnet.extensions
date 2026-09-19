using System.Diagnostics;

using Gaa.Extensions.Observer;

namespace Gaa.Worker.Example;

/// <summary>
/// Пример потребителя сообщений.
/// </summary>
public sealed partial class ExampleConsumer : IAsyncConsumer<ExampleMessage>
{
    private readonly ILogger _log;

    private readonly ExampleMetrics _metrics;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ExampleConsumer"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика для журналов протоколирования событий.</param>
    /// <param name="metrics">Пример метрик приложения.</param>
    public ExampleConsumer(ILoggerFactory loggerFactory, ExampleMetrics metrics)
    {
        _log = loggerFactory.CreateLogger("Gaa.Worker.Example.Consumer");
        _metrics = metrics;
    }

    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<ExampleMessage> context, CancellationToken cancellationToken)
    {
        var message = context.Message;
        var processingTime = (double)(Stopwatch.GetTimestamp() - message.CreationTime) / TimeSpan.TicksPerMicrosecond;
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