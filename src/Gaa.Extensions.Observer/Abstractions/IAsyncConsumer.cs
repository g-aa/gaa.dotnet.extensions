#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Потребитель сообщения.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
public interface IAsyncConsumer<TMessage>
    where TMessage : notnull
{
    /// <summary>
    /// Обрабатывает сообщение.
    /// </summary>
    /// <param name="context">Контекст сообщения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ConsumeAsync(MessageContext<TMessage> context, CancellationToken cancellationToken);
}