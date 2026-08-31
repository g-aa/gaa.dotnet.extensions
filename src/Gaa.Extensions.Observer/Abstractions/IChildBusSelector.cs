#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Селектро дочерних шин.
/// </summary>
internal interface IChildBusSelector
{
    /// <summary>
    /// Предоставляет наименование <see cref="IChildBus"/> по <typeparamref name="TMessage"/>.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <returns>Наименование дочерней шины.</returns>
    string? GetBusName<TMessage>()
        where TMessage : notnull;
}