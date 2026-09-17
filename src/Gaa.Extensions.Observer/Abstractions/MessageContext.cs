#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable SA1600  // Elements should be documented

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст сообщения.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
public readonly ref struct MessageContext<TMessage>
    where TMessage : notnull
{
    internal readonly IReadOnlyDictionary<string, string> _headers;

    internal readonly TMessage _message;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageContext{TMessage}"/>.
    /// </summary>
    /// <param name="headers">Заголовки сообщения.</param>
    /// <param name="message">Сообщение.</param>
    internal MessageContext(IReadOnlyDictionary<string, string> headers, TMessage message)
    {
        _headers = headers;
        _message = message;
    }

    /// <summary>
    /// Заголовки сообщения.
    /// </summary>
    public IReadOnlyDictionary<string, string> Headers => _headers;

    /// <summary>
    /// Сообщение.
    /// </summary>
    public TMessage Message => _message;
}