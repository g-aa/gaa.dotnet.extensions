namespace Gaa.Extensions;

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

    /// <summary>
    /// Токен отмены.
    /// </summary>
    public CancellationToken CancellationToken { get; init; }
}