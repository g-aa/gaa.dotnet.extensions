using Gaa.Extensions.Observer;

namespace Gaa.Extensions.Benchmark.Observer.Features;

/// <summary>
/// Пример потребителя строковых сообщений.
/// </summary>
internal sealed class StringConsumer : IAsyncConsumer<string>
{
    /// <inheritdoc />
    public Task ConsumeAsync(MessageContext<string> context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}