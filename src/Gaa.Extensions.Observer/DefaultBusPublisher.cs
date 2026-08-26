namespace Gaa.Extensions.Observer;

/// <summary>
/// Шина для обмена сообщениями и событиями.
/// </summary>
internal sealed partial class DefaultBusPublisher : IPublisher
{
    private readonly IBackgroundTaskQueue _taskQueue;

    private readonly IReadOnlyDictionary<string, string> _messageHeaders;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBusPublisher"/>.
    /// </summary>
    /// <param name="taskQueue">Очередь фоновых задач.</param>
    public DefaultBusPublisher(IBackgroundTaskQueue taskQueue)
    {
        _taskQueue = taskQueue;
        _messageHeaders = new Dictionary<string, string>();
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        return PublishAsync(message, _messageHeaders, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        TimeSpan executionTimeLimit,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        return PublishAsync(message, _messageHeaders, executionTimeLimit, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        var backgroundTask = new DefaultBackgroundTask<TMessage>(message, messageHeaders);
        return _taskQueue.QueueTaskAsync(backgroundTask, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        TimeSpan executionTimeLimit,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        var backgroundTask = new DefaultBackgroundTask<TMessage>(message, messageHeaders, executionTimeLimit);
        return _taskQueue.QueueTaskAsync(backgroundTask, cancellationToken);
    }
}