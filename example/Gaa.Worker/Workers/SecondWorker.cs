using Gaa.Extensions.Observer;
using Gaa.Worker.Messages;
using Microsoft.Extensions.Options;

namespace Gaa.Worker.Workers;

/// <summary>
/// Пример фонового задания публикующего сообщения.
/// </summary>
public sealed partial class SecondWorker : BackgroundService
{
    private readonly ILogger _log;

    private readonly IPublisher _publisher;

    private readonly TimeSpan _delay;

    /// <summary>
    /// Инициализирует новывый экземпляр класса <see cref="SecondWorker"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика для журналов протоколирования собцытий.</param>
    /// <param name="options">Настройки.</param>
    /// <param name="publisher">Шина для публикации сообщений.</param>
    public SecondWorker(ILoggerFactory loggerFactory, IOptions<TimeDelayOptions> options, IPublisher publisher)
    {
        _log = loggerFactory.CreateLogger("Gaa.Worker.Second.Publisher");
        _publisher = publisher;
        _delay = options.Value.SecondWorker;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var number = 1;
        using var timer = new PeriodicTimer(_delay);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    var message = new SecondMessage
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