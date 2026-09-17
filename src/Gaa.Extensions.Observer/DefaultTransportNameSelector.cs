using System.Collections.Frozen;

using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Селектор для выбора наименования транспортной шины.
/// </summary>
internal sealed class DefaultTransportNameSelector : ITransportNameSelector
{
    private readonly FrozenDictionary<Type, string> _routes;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultTransportNameSelector"/>.
    /// </summary>
    /// <param name="options">Настройки шины сообщений.</param>
    public DefaultTransportNameSelector(IOptions<BusOptions> options)
    {
        var subscriptions = options.Value.Subscriptions;
        _routes = subscriptions.SelectMany(Reverse).ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <inheritdoc />
    public string? GetTransportName<TMessage>()
    {
        var messageType = typeof(TMessage);
        return _routes.TryGetValue(messageType, out var transportName)
            ? transportName
            : default;
    }

    private static IEnumerable<KeyValuePair<Type, string>> Reverse(KeyValuePair<string, ICollection<Type>> subscription)
    {
        var transportName = subscription.Key;
        return subscription.Value.Select(messageType => new KeyValuePair<Type, string>(messageType, transportName));
    }
}