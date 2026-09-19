using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст выполнения для сообщения.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
internal sealed class MessageExecutionContext<TMessage> : IMessageExecutionContext
    where TMessage : notnull
{
    private readonly IReadOnlyDictionary<string, string> _headers;

    private readonly TMessage _message;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageExecutionContext{TMessage}"/>.
    /// </summary>
    /// <param name="headers">Заголовки сообщения.</param>
    /// <param name="message">Сообщение.</param>
    public MessageExecutionContext(IReadOnlyDictionary<string, string> headers, TMessage message)
    {
        _headers = headers;
        _message = message;
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageExecutionContext{TMessage}"/>.
    /// </summary>
    /// <param name="context">Заголовки сообщения.</param>
    public MessageExecutionContext(MessageContext<TMessage> context)
    {
        _headers = context._headers;
        _message = context._message;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(IServiceScopeFactory scopeFactory, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        await ExecuteAsync(scope.ServiceProvider, cancellationToken);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var messageType = typeof(TMessage);
        return $"Gaa.Extensions.Observer.MessageExecutionContext<{messageType.Namespace}.{messageType.Name}>";
    }

    private Task ExecuteAsync(IServiceProvider provider, CancellationToken cancellationToken)
    {
        var consumer = provider.GetService<IAsyncConsumer<TMessage>>();
        return consumer != null ? ConsumeAsync(consumer, cancellationToken) : Task.CompletedTask;
    }

    private Task ConsumeAsync(IAsyncConsumer<TMessage> consumer, CancellationToken cancellationToken)
    {
        var message = new MessageContext<TMessage>(_headers, _message);
        return consumer.ConsumeAsync(message, cancellationToken);
    }
}