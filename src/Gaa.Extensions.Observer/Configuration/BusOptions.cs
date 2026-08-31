#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Общие настройки шины сообщений и событий.
/// </summary>
public sealed class BusOptions
{
    /// <summary>
    /// Ограничение по времени выполнения обработки одного сообщения.
    /// </summary>
    public TimeSpan ExecutionTimeLimit { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Коллекция настроек дочерних шин.
    /// </summary>
    public ICollection<ChildBusOptions> Options { get; private set; } = new List<ChildBusOptions>();

    /// <summary>
    /// Подписка шин на сообщения.
    /// </summary>
    public IDictionary<string, ICollection<Type>> Subscriptions { get; private set; } = new Dictionary<string, ICollection<Type>>();
}