using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions;

/// <summary>
/// Фоновая задача.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
internal sealed class BackgroundTask<TMessage>
    : IBackgroundTask
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
    public Task ExecuteAsync(
        IServiceProvider provider,
        CancellationToken cancellationToken)
    {
        var consumer = provider.GetRequiredService<IAsyncConsumer<TMessage>>();
        var context = new MessageContext<TMessage>
        {
            Message = Message,
            Headers = MessageHeaders,
            CancellationToken = cancellationToken,
        };

        return consumer.ConsumeAsync(context);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var messageType = typeof(TMessage);
        return $"BackgroundTask<{messageType.Namespace}.{messageType.Name}>";
    }
}