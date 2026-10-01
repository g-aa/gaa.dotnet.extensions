#pragma warning disable SA1402 // File may only contain a single type

namespace Gaa.Extensions.Observer.Test.Features;

/// <summary>
/// Пример потребителя сообщений.
/// </summary>
public sealed class FirstConsumer : IAsyncConsumer<FirstMessage>
{
    private readonly IMessageLogger _log;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FirstConsumer"/>.
    /// </summary>
    /// <param name="log">Журнал регистрации сообщений.</param>
    public FirstConsumer(IMessageLogger log)
    {
        _log = log;
    }

    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<FirstMessage> context, CancellationToken cancellationToken)
    {
        _log.Log($"Получено сообщение, ID: {context.Message.Id}.");
        return Task.CompletedTask;
    }
}

/// <summary>
/// Пример сообщения.
/// </summary>
public sealed record FirstMessage
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public required Guid Id { get; init; }
}