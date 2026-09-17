#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Общие настройки шины сообщений и событий.
/// </summary>
public sealed class BusOptions
{
    /// <summary>
    /// Коллекция настроек транспортных шин.
    /// </summary>
    public ICollection<TransportOptions> Transports { get; private set; } = new HashSet<TransportOptions>();

    /// <summary>
    /// Подписка транспортных шин на сообщения.
    /// </summary>
    public IDictionary<string, ICollection<Type>> Subscriptions { get; private set; } = new Dictionary<string, ICollection<Type>>();
}