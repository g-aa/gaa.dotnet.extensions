namespace Gaa.Extensions;

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
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    Task ConsumeAsync(MessageContext<TMessage> context);
}