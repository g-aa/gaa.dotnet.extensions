#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст сообщения.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
public sealed class MessageContext<TMessage>
    where TMessage : notnull
{
    /// <summary>
    /// Сообщение.
    /// </summary>
    public required TMessage Message { get; init; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    public required IReadOnlyDictionary<string, string> Headers { get; init; }
}