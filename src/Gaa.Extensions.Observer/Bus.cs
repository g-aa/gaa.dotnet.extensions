using Microsoft.Extensions.Logging;

namespace Gaa.Extensions;

/// <summary>
/// Шина для обмена сообщениями и событиями.
/// </summary>
internal sealed partial class Bus : IBus
{
    private readonly ILogger _log;

    private readonly IBackgroundTaskQueue _taskQueue;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Bus"/>.
    /// </summary>
    /// <param name="log">Журнал протоколирования событий.</param>
    /// <param name="taskQueue">Очередь фоновых задач.</param>
    public Bus(
        ILogger<Bus> log,
        IBackgroundTaskQueue taskQueue)
    {
        _log = log;
        _taskQueue = taskQueue;
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        return PublishAsync(message, new Dictionary<string, string>(), cancellationToken);
    }

    /// <inheritdoc />
    public async Task PublishAsync<TMessage>(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        try
        {
            Log.StartMessage(_log);
            var backgroundTask = new BackgroundTask<TMessage>
            {
                Message = message,
                MessageHeaders = messageHeaders,
            };

            await _taskQueue.QueueTaskAsync(backgroundTask, cancellationToken);
            Log.StopMessage(_log);
        }
        catch (OperationCanceledException)
        {
            /* Можно не обрабатывать */
        }
        catch (Exception ex)
        {
            Log.ErrorMessage(_log, ex);
        }
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Trace, Message = "Регистрируем новое сообщение в шине.")]
        public static partial void StartMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Сообщение успешно зарегистрировано в шине.")]
        public static partial void StopMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Error, Message = "Сработала ошибка в процессе регистрации сообщения в шине!")]
        public static partial void ErrorMessage(ILogger log, Exception ex);
    }
}