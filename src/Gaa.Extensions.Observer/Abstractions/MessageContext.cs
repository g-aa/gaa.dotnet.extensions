#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст сообщения.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
public readonly ref struct MessageContext<TMessage>
    where TMessage : notnull
{
    private readonly TMessage _message;

    private readonly IReadOnlyDictionary<string, string> _headers;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageContext{TMessage}"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="headers">Заголовки сообщения.</param>
    internal MessageContext(TMessage message, IReadOnlyDictionary<string, string> headers)
    {
        _message = message;
        _headers = headers;
    }

    /// <summary>
    /// Сообщение.
    /// </summary>
    public TMessage Message => _message;

    /// <summary>
    /// Заголовки сообщения.
    /// </summary>
    public IReadOnlyDictionary<string, string> Headers => _headers;
}