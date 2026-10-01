#pragma warning disable SA1402 // File may only contain a single type

namespace Gaa.Extensions.Observer.Test.Features;

/// <summary>
/// Пример потребителя сообщений.
/// </summary>
public sealed class SecondConsumer : IAsyncConsumer<SecondMessage>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SecondConsumer"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public SecondConsumer(IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<SecondMessage> context, CancellationToken cancellationToken)
    {
        _log.Log($"Получено сообщение, ID: {context.Message.Id}.");
        return Task.CompletedTask;
    }
}

/// <summary>
/// Пример сообщения.
/// </summary>
public sealed record SecondMessage
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public required Guid Id { get; init; }
}