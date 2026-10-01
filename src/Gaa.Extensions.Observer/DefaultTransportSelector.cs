using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Селектор для выбора транспортной шины.
/// </summary>
internal sealed class DefaultTransportSelector : ITransportSelector
{
    private readonly Lock _lock;

    private readonly List<TransportOptions> _transportOptions;

    private readonly List<ITransportFactory> _factories;

    private readonly Dictionary<string, ITransport> _transports;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultTransportSelector"/>.
    /// </summary>
    /// <param name="options">Общие настройки шины.</param>
    /// <param name="factories">Фабрики транспортных шин.</param>
    public DefaultTransportSelector(IOptions<BusOptions> options, IEnumerable<ITransportFactory> factories)
    {
        _lock = new Lock();
        _transportOptions = [.. options.Value.Transports];
        _factories = [.. factories];
        _transports = [];
    }

    /// <inheritdoc />
    public ITransport GetTransport(string transportName)
    {
        return _transports.TryGetValue(transportName, out var transport)
           ? transport
           : Create(transportName);
    }

    /// <inheritdoc />
    public T GetTransport<T>(string transportName)
        where T : ITransport
    {
        return GetTransport(transportName) is T transport
            ? transport
            : Throw<T>(transportName);
    }

    private static T Throw<T>(string transportName)
        where T : ITransport
    {
        var message = $"Не удается получить экземпляр транспортной шины '{typeof(T).FullName}' по наименованию '{transportName}'!";
        throw new InvalidOperationException(message);
    }

    private ITransport Create(string transportName)
    {
        lock (_lock)
        {
            if (_transports.TryGetValue(transportName, out var transport))
            {
                return transport;
            }

            var options = _transportOptions.Single(o => o.Name == transportName);
            var factory = _factories.Single(f => f.Type == options.Type);
            transport = factory.CreateTransport(options);
            _transports.TryAdd(transportName, transport);
            return transport;
        }
    }
}