using Gaa.Extensions.Observer;

namespace Gaa.Extensions.Benchmark.Observer.Features;

/// <summary>
/// Заглушка для фабрики транспортных шин.
/// </summary>
internal sealed class MockTransportFactory : ITransportFactory
{
    /// <inheritdoc />
    public Type Type => typeof(MockTransport);

    /// <inheritdoc />
    public ITransport CreateTransport(TransportOptions options)
    {
        return new MockTransport();
    }
}