namespace Gaa.Extensions;

/// <summary>
/// Шина для обмена сообщениями и событиями.
/// </summary>
internal sealed partial class DefaultBusPublisher : IPublisher
{
    private readonly IBackgroundTaskQueue _taskQueue;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBusPublisher"/>.
    /// </summary>
    /// <param name="taskQueue">Очередь фоновых задач.</param>
    public DefaultBusPublisher(IBackgroundTaskQueue taskQueue)
    {
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
    public Task PublishAsync<TMessage>(
        TMessage message,
        TimeSpan executionTimeLimit,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        return PublishAsync(message, new Dictionary<string, string>(), executionTimeLimit, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        return _taskQueue.QueueTaskAsync(
            new BackgroundTask<TMessage>
            {
                Message = message,
                MessageHeaders = messageHeaders,
                ExecutionTimeLimit = null,
            },
            cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        TimeSpan executionTimeLimit,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        return _taskQueue.QueueTaskAsync(
            new BackgroundTask<TMessage>
            {
                Message = message,
                MessageHeaders = messageHeaders,
                ExecutionTimeLimit = executionTimeLimit,
            },
            cancellationToken);
    }
}