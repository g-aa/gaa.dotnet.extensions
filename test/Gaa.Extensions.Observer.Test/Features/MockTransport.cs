using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer.Test.Features;

/// <summary>
/// Заглушка транспорт.
/// </summary>
public sealed class MockTransport : ITransport
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