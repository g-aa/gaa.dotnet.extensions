using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Селектро дочерних шин.
/// </summary>
internal sealed class DefaultChildBusSelector : IChildBusSelector
{
    private readonly Dictionary<Type, string> _routes;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultChildBusSelector"/>.
    /// </summary>
    /// <param name="options">Настройки шины сообщений.</param>
    public DefaultChildBusSelector(IOptions<BusOptions> options)
    {
        var subscriptions = options.Value.Subscriptions;
        _routes = subscriptions.SelectMany(Reverse).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <inheritdoc />
    public string? GetBusName<TMessage>()
        where TMessage : notnull
    {
        var messageType = typeof(TMessage);
        return _routes.TryGetValue(messageType, out var busName)
            ? busName
            : default;
    }

    private static IEnumerable<KeyValuePair<Type, string>> Reverse(KeyValuePair<string, ICollection<Type>> subscription)
    {
        var busName = subscription.Key;
        return subscription.Value.Select(messageType => new KeyValuePair<Type, string>(messageType, busName));
    }
}