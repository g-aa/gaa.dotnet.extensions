#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <inheritdoc cref="IPublisher"/>
internal sealed class DefaultPublisher : IPublisher
{
    private readonly IReadOnlyDictionary<string, string> _defaultHeaders;

    private readonly ITransportNameSelector _transportNameSelector;

    private readonly ITransportSelector _transportSelector;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultPublisher"/>.
    /// </summary>
    /// <param name="transportNameSelector">Селектор для выбора наименования транспортной шины.</param>
    /// <param name="transportSelector">Селектор для выбора транспортной шины.</param>
    public DefaultPublisher(
        ITransportNameSelector transportNameSelector,
        ITransportSelector transportSelector)
    {
        _defaultHeaders = new Dictionary<string, string>();
        _transportNameSelector = transportNameSelector;
        _transportSelector = transportSelector;
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        return PublishAsync(message, _defaultHeaders, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        var transportName = _transportNameSelector.GetTransportName<TMessage>();
        return transportName != null
            ? PublishAsync(transportName, message, messageHeaders, cancellationToken)
            : Task.CompletedTask;
    }

    private Task PublishAsync<TMessage>(
        string transportName,
        TMessage message,
        IReadOnlyDictionary<string, string> headers,
        CancellationToken cancellationToken)
        where TMessage : notnull
    {
        var context = new MessageContext<TMessage>(headers, message);
        return _transportSelector.GetTransport(transportName).PublishAsync(context, cancellationToken);
    }
}