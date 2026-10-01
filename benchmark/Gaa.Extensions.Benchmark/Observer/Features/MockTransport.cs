using Gaa.Extensions.Observer;
using Microsoft.Extensions.DependencyInjection;

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

    /// <inheritdoc />
    public Task RunAsync(IServiceScopeFactory scopeFactory, CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Close(CancellationToken cancellationToken)
    {
    }
}