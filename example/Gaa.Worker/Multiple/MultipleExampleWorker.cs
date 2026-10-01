using System.Diagnostics;

using Gaa.Extensions.Observer;
using Microsoft.Extensions.Options;

namespace Gaa.Worker.Multiple;

/// <summary>
/// Пример фонового задания публикующего сообщение.
/// </summary>
public sealed partial class MultipleExampleWorker : BackgroundService
{
    private readonly ILogger _log;

    private readonly IPublisher _publisher;

    private readonly TimeSpan _period;

    private readonly int _messagePerPeriod;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MultipleExampleWorker"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика для журналов протоколирования событий.</param>
    /// <param name="publisher">Шина для публикации сообщений.</param>
    /// <param name="options">Настройки.</param>
    public MultipleExampleWorker(ILoggerFactory loggerFactory, IPublisher publisher, IOptions<TimeOptions> options)
    {
        _log = loggerFactory.CreateLogger("Gaa.Worker.Example.Multiple.Publisher");
        _publisher = publisher;
        _period = options.Value.PeriodForMultipleWorker;
        _messagePerPeriod = options.Value.MessagePerPeriodForMultipleWorker;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var number = 1;
        using var timer = new PeriodicTimer(_period);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    for (var i = 0; i < _messagePerPeriod; i++)
                    {
                        var message = new MultipleExampleMessage
                        {
                            Id = Guid.NewGuid(),
                            Text = $"Сообщение #{number}",
                            CreationTime = Stopwatch.GetTimestamp(),
                        };

                        await _publisher.PublishAsync(message, stoppingToken);
                        Log.Message(_log, number);
                        number++;
                    }
                }
                catch (Exception ex)
                {
                    Log.ErrorMessage(_log, ex);
                }
            }
        }
        catch (OperationCanceledException)
        {
            /* Можно не обрабатывать */
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