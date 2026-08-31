using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фоновая задача.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
internal sealed class DefaultBackgroundTask<TMessage> : IBackgroundTask
    where TMessage : notnull
{
    private readonly TMessage _message;

    private readonly IReadOnlyDictionary<string, string> _messageHeaders;

    private readonly TimeSpan? _executionTimeLimit;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBackgroundTask{TMessage}"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="messageHeaders">Заголовки сообщения.</param>
    internal DefaultBackgroundTask(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders)
    {
        _message = message;
        _messageHeaders = messageHeaders;
        _executionTimeLimit = null;
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultBackgroundTask{TMessage}"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="messageHeaders">Заголовки сообщения.</param>
    /// <param name="executionTimeLimit">Предьлное время обрабтки сообщения.</param>
    internal DefaultBackgroundTask(
        TMessage message,
        IReadOnlyDictionary<string, string> messageHeaders,
        TimeSpan? executionTimeLimit)
    {
        _message = message;
        _messageHeaders = messageHeaders;
        _executionTimeLimit = executionTimeLimit;
    }

    /// <summary>
    /// Сообщение.
    /// </summary>
    public TMessage Message => _message;

    /// <summary>
    /// Заголовки сообщения.
    /// </summary>
    public IReadOnlyDictionary<string, string> MessageHeaders => _messageHeaders;

    /// <inheritdoc />
    public TimeSpan? ExecutionTimeLimit => _executionTimeLimit;

    /// <inheritdoc />
    public Task ExecuteAsync(IServiceProvider provider, CancellationToken cancellationToken)
    {
        var consumer = provider.GetService<IAsyncConsumer<TMessage>>();
        return consumer != null ? ConsumeAsync(consumer, cancellationToken) : Task.CompletedTask;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var messageType = typeof(TMessage);
        return $"Gaa.Extensions.Observer.BackgroundTask<{messageType.Namespace}.{messageType.Name}>";
    }

    private Task ConsumeAsync(IAsyncConsumer<TMessage> consumer, CancellationToken cancellationToken)
    {
        var message = new MessageContext<TMessage>(Message, MessageHeaders);
        return consumer.ConsumeAsync(message, cancellationToken);
    }
}