#pragma warning disable IDE0130 // Пространство имен (namespace) не соответствует структуре папок.

namespace Gaa.Extensions;

/// <summary>
/// Шина для обмена сообщениями и событиями.
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// Публикует сообщение в шину.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="message">Сообщение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken)
        where TMessage : notnull;

    /// <summary>
    /// Публикует сообщение в шину.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="message">Сообщение.</param>
    /// <param name="executionTimeLimit">Предьлное время обрабтки сообщения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task PublishAsync<TMessage>(
        TMessage message,
        TimeSpan executionTimeLimit,
        CancellationToken cancellationToken)
        where TMessage : notnull;

    /// <summary>
    /// Публикует сообщение в шину.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="message">Сообщение.</param>
    /// <param name="messageHeaders">Заголовки сообщения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task PublishAsync<TMessage>(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        CancellationToken cancellationToken)
        where TMessage : notnull;

    /// <summary>
    /// Публикует сообщение в шину.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="message">Сообщение.</param>
    /// <param name="messageHeaders">Заголовки сообщения.</param>
    /// <param name="executionTimeLimit">Предьлное время обрабтки сообщения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task PublishAsync<TMessage>(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        TimeSpan executionTimeLimit,
        CancellationToken cancellationToken)
        where TMessage : notnull;
}