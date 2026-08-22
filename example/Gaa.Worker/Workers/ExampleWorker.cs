using Gaa.Extensions.Observer;
using Gaa.Worker.Messages;

namespace Gaa.Worker.Workers;

/// <summary>
/// Пример фонового задания публикующего сообщения.
/// </summary>
public sealed partial class ExampleWorker : BackgroundService
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(2);

    private readonly ILogger _log;

    private readonly IPublisher _publisher;

    /// <summary>
    /// Инициализирует новывый экземпляр класса <see cref="ExampleWorker"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика для журналов протоколирования собцытий.</param>
    /// <param name="publisher">Шина для публикации сообщений.</param>
    public ExampleWorker(ILoggerFactory loggerFactory, IPublisher publisher)
    {
        _log = loggerFactory.CreateLogger("Gaa.Worker.Example.Publisher");
        _publisher = publisher;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var number = 1;
        using var timer = new PeriodicTimer(Delay);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    var message = new ExampleMessage
                    {
                        Id = Guid.NewGuid(),
                        Text = $"Сообщение #{number}",
                        CreationTime = DateTimeOffset.UtcNow,
                    };

                    await _publisher.PublishAsync(message, stoppingToken);
                    Log.Message(_log, number);
                    number++;
                }
                catch (Exception ex)
                {
                    Log.ErrorMessage(_log, ex);
                }
            }
        }
        catch (OperationCanceledException)
        {
            /* Можно не регичстрировать */
        }
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Debug, Message = "В шину было отправлено сообщение '#{Number}'.")]
        public static partial void Message(ILogger log, int number);

        [LoggerMessage(Level = LogLevel.Warning, Message = "В процессе работы фонового задания сработала необработанное исключение!")]
        public static partial void ErrorMessage(ILogger log, Exception ex);
    }
}