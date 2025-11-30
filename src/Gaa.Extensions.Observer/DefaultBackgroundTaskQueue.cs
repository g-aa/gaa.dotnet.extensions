using System.Threading.Channels;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gaa.Extensions;

/// <summary>
/// Имплементация <see cref="IBackgroundTaskQueue"/> по умолчанию.
/// </summary>
internal sealed partial class DefaultBackgroundTaskQueue
    : IBackgroundTaskQueue
{
    private readonly ILogger _log;

    private readonly Channel<IBackgroundTask> _queue;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBackgroundTaskQueue"/>.
    /// </summary>
    /// <param name="log">Журнал протоколирования событий.</param>
    /// <param name="options">Настройки шины сообщений.</param>
    public DefaultBackgroundTaskQueue(
      ILogger<DefaultBackgroundTaskQueue> log,
      IOptions<BusOptions> options)
    {
        var taskQueueCapacity = options.Value.BackgroundTaskQueueCapacity;
        var boundedChannelOptions = new BoundedChannelOptions(taskQueueCapacity)
        {
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false,
        };

        _log = log;
        _queue = Channel.CreateBounded<IBackgroundTask>(boundedChannelOptions);
        Log.QueueCapacityMessage(_log, taskQueueCapacity);
    }

    /// <inheritdoc />
    public async ValueTask QueueTaskAsync(
      IBackgroundTask backgroundTask,
      CancellationToken cancellationToken)
    {
        Log.StartQueueTaskMessage(_log, backgroundTask);
        await _queue.Writer.WriteAsync(backgroundTask, cancellationToken);
        Log.StopQueueTaskMessage(_log, backgroundTask);
    }

    /// <inheritdoc />
    public async ValueTask<IBackgroundTask> DequeueTaskAsync(
        CancellationToken cancellationToken)
    {
        Log.StartDequeueTaskMessage(_log);
        var backgroundTask = await _queue.Reader.ReadAsync(cancellationToken);
        Log.StopDequeueTaskMessage(_log, backgroundTask);
        Log.QueueTaskCountMessage(_log, _queue.Reader.Count);
        return backgroundTask;
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Trace, Message = "Емкость очереди фоновых задач: '{Capacity}'.")]
        public static partial void QueueCapacityMessage(ILogger log, int capacity);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Инициировано добавление фоновой задачи '{BackgroundTask}' в очередь.")]
        public static partial void StartQueueTaskMessage(ILogger log, IBackgroundTask backgroundTask);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Фоновая задача '{BackgroundTask}' успешно добавлена в очередь на исполнение.")]
        public static partial void StopQueueTaskMessage(ILogger log, IBackgroundTask backgroundTask);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Инициировано извлечение фоновой задачи из очереди для исполнения.")]
        public static partial void StartDequeueTaskMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Фоновая задача '{BackgroundTask}' успешно излечена для исполнения.")]
        public static partial void StopDequeueTaskMessage(ILogger log, IBackgroundTask backgroundTask);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Количество фоновых задач в очереди: '{Count}' штук.")]
        public static partial void QueueTaskCountMessage(ILogger log, int count);
    }
}