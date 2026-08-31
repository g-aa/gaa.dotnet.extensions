using System.Threading.Channels;

using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Имплементация <see cref="IChildBus"/> по умолчанию.
/// </summary>
internal sealed partial class DefaultChildBus : IChildBus
{
    private readonly ILogger _log;

    private readonly Channel<IBackgroundTask> _queue;

    private readonly ChannelReader<IBackgroundTask> _reader;

    private readonly ChannelWriter<IBackgroundTask> _writer;

    private readonly string _name;

    private readonly int _taskQueueCapacity;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultChildBus"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика журналов протоколирования событий.</param>
    /// <param name="options">Настройки шины сообщений.</param>
    public DefaultChildBus(ILoggerFactory loggerFactory, ChildBusOptions options)
    {
        _name = options.Name;
        _taskQueueCapacity = options.Capacity;
        var channelOptions = new BoundedChannelOptions(_taskQueueCapacity)
        {
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false,
        };

        _log = loggerFactory.CreateLogger(CategoryName.DefaultBus);
        _queue = Channel.CreateBounded<IBackgroundTask>(channelOptions);
        _reader = _queue.Reader;
        _writer = _queue.Writer;
        Log.QueueCapacityMessage(_log, _taskQueueCapacity);
    }

    /// <summary>
    /// Наименование шины.
    /// </summary>
    public string Name => _name;

    /// <inheritdoc />
    public int Capacity => _taskQueueCapacity;

    /// <inheritdoc />
    public int Count => _reader.Count;

    /// <inheritdoc />
    public async Task QueueTaskAsync(IBackgroundTask backgroundTask, CancellationToken cancellationToken)
    {
        await _writer.WriteAsync(backgroundTask, cancellationToken);
        Log.StopQueueTaskMessage(_log, backgroundTask);
    }

    /// <inheritdoc />
    public async Task<IBackgroundTask> DequeueTaskAsync(CancellationToken cancellationToken)
    {
        var backgroundTask = await _reader.ReadAsync(cancellationToken);
        Log.StopDequeueTaskMessage(_log, backgroundTask);
        return backgroundTask;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Bus name:{Name}; Message capacity:{Capacity}, count:{Count}";
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Trace, Message = "Емкость очереди фоновых задач установлена равной '{Capacity}'.")]
        public static partial void QueueCapacityMessage(ILogger log, int capacity);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Фоновая задача '{BackgroundTask}' добавлена в очередь на исполнение.")]
        public static partial void StopQueueTaskMessage(ILogger log, IBackgroundTask backgroundTask);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Фоновая задача '{BackgroundTask}' излечена для исполнения.")]
        public static partial void StopDequeueTaskMessage(ILogger log, IBackgroundTask backgroundTask);
    }
}