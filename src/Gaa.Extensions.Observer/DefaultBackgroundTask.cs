using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фоновая задача.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
internal sealed class DefaultBackgroundTask<TMessage> : IBackgroundTask
    where TMessage : notnull
{
    /// <summary>
    /// Сообщение.
    /// </summary>
    public required TMessage Message { get; init; }

    /// <summary>
    /// Заголовки сообщения.
    /// </summary>
    public required IReadOnlyDictionary<string, string> MessageHeaders { get; init; }

    /// <inheritdoc />
    public TimeSpan? ExecutionTimeLimit { get; init; }

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
        return $"Gaa.Extensions.BackgroundTask<{messageType.Namespace}.{messageType.Name}>";
    }

    private Task ConsumeAsync(IAsyncConsumer<TMessage> consumer, CancellationToken cancellationToken)
    {
        var message = new MessageContext<TMessage>
        {
            Message = Message,
            Headers = MessageHeaders,
        };

        return consumer.ConsumeAsync(message, cancellationToken);
    }
}