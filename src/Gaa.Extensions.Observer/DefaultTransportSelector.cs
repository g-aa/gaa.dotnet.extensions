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

    private readonly Dictionary<string, ITransport> _transports;

    private readonly ITransportFactory _factory;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultTransportSelector"/>.
    /// </summary>
    /// <param name="options">Общие настройки шины.</param>
    /// <param name="factory">Фабрика транспортных шин.</param>
    public DefaultTransportSelector(IOptions<BusOptions> options, ITransportFactory factory)
    {
        _lock = new Lock();
        _transportOptions = [.. options.Value.Transports];
        _transports = [];
        _factory = factory;
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

            var options = _transportOptions.FirstOrDefault(o => o.Name == transportName);
            transport = _factory.CreateTransport(options);
            _transports.TryAdd(transportName, transport);
            return transport;
        }
    }
}