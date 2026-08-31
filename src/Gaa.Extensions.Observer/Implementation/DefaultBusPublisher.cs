#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Шина для обмена сообщениями и событиями.
/// </summary>
internal sealed partial class DefaultBusPublisher : IPublisher
{
    private readonly IChildBusSelector _busSelector;

    private readonly IChildBusFactory<DefaultChildBus> _busFactory;

    private readonly IReadOnlyDictionary<string, string> _messageHeaders;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBusPublisher"/>.
    /// </summary>
    /// <param name="busSelector">Селектро дочерних шин.</param>
    /// <param name="busFactory">Фабрика дочерних шин.</param>
    public DefaultBusPublisher(
        IChildBusSelector busSelector,
        IChildBusFactory<DefaultChildBus> busFactory)
    {
        _busSelector = busSelector;
        _busFactory = busFactory;
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
        var busName = _busSelector.GetBusName<TMessage>();
        return busName != null
            ? PublishAsync(busName, message, messageHeaders, cancellationToken)
            : Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        TimeSpan executionTimeLimit,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        var busName = _busSelector.GetBusName<TMessage>();
        return busName != null
            ? PublishAsync(busName, message, messageHeaders, executionTimeLimit, cancellationToken)
            : Task.CompletedTask;
    }

    private Task PublishAsync<TMessage>(
        string busName,
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        var backgroundTask = new DefaultBackgroundTask<TMessage>(message, messageHeaders);
        return _busFactory.GetOrCreate(busName).QueueTaskAsync(backgroundTask, cancellationToken);
    }

    private Task PublishAsync<TMessage>(
        string busName,
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        TimeSpan executionTimeLimit,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        var backgroundTask = new DefaultBackgroundTask<TMessage>(message, messageHeaders, executionTimeLimit);
        return _busFactory.GetOrCreate(busName).QueueTaskAsync(backgroundTask, cancellationToken);
    }
}