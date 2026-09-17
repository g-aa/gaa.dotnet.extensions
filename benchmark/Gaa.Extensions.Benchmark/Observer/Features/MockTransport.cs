using Gaa.Extensions.Observer;

namespace Gaa.Extensions.Benchmark.Observer.Features;

/// <summary>
/// Заглушка транспорт.
/// </summary>
internal sealed class MockTransport : ITransport
{
    /// <inheritdoc />
    public string Name => "Mock.Transport";

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(MessageContext<TMessage> message, CancellationToken cancellationToken)
        where TMessage : notnull
    {
        return Task.CompletedTask;
    }
}