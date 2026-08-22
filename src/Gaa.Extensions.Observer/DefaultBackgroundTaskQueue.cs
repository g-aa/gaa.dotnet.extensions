using System.Threading.Channels;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gaa.Extensions.Observer;

/// <summary>
/// Имплементация <see cref="IBackgroundTaskQueue"/> по умолчанию.
/// </summary>
internal sealed partial class DefaultBackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly ILogger _log;

    private readonly Channel<IBackgroundTask> _queue;

    private readonly ChannelReader<IBackgroundTask> _reader;

    private readonly ChannelWriter<IBackgroundTask> _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBackgroundTaskQueue"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика журналов протоколирования событий.</param>
    /// <param name="options">Настройки шины сообщений.</param>
    public DefaultBackgroundTaskQueue(
        ILoggerFactory loggerFactory,
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

        _log = loggerFactory.CreateLogger(CategoryName.DefaultBus);
        _queue = Channel.CreateBounded<IBackgroundTask>(boundedChannelOptions);
        _reader = _queue.Reader;
        _writer = _queue.Writer;
        Log.QueueCapacityMessage(_log, taskQueueCapacity);
    }

    /// <inheritdoc />
    public async Task QueueTaskAsync(
        IBackgroundTask backgroundTask,
        CancellationToken cancellationToken)
    {
        await _writer.WriteAsync(backgroundTask, cancellationToken);
        Log.StopQueueTaskMessage(_log, backgroundTask);
    }

    /// <inheritdoc />
    public async Task<IBackgroundTask> DequeueTaskAsync(CancellationToken cancellationToken)
    {
        var backgroundTask = await _reader.ReadAsync(cancellationToken);
        Log.StopDequeueTaskMessage(_log, backgroundTask, _reader.Count);
        return backgroundTask;
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Trace, Message = "Емкость очереди фоновых задач установлена равной '{Capacity}'.")]
        public static partial void QueueCapacityMessage(ILogger log, int capacity);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Фоновая задача '{BackgroundTask}' успешно добавлена в очередь на исполнение.")]
        public static partial void StopQueueTaskMessage(ILogger log, IBackgroundTask backgroundTask);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Фоновая задача '{BackgroundTask}' успешно излечена для исполнения, количество фоновых задач в очереди '{Count}' штук.")]
        public static partial void StopDequeueTaskMessage(ILogger log, IBackgroundTask backgroundTask, int count);
    }
}