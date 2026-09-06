using Gaa.Extensions.Observer;

namespace Gaa.Worker.Error;

/// <summary>
/// Потребитель сообщение вызывающих ошибку.
/// </summary>
internal sealed class ErrorConsumer : IAsyncConsumer<ErrorMessage>
{
    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<ErrorMessage> context, CancellationToken cancellationToken)
    {
        throw new InvalidOperationException($"Сообщение типа '{typeof(ErrorMessage)}' привело к возникновению ошибки во время обработки!");
    }
}