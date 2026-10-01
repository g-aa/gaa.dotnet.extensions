namespace Gaa.Extensions.Observer.Test.Features;

/// <summary>
/// Заглушка для фабрики транспортных шин.
/// </summary>
public sealed class MockTransportFactory : ITransportFactory
{
    /// <inheritdoc />
    public Type Type => typeof(MockTransport);

    /// <inheritdoc />
    public ITransport CreateTransport(TransportOptions options)
    {
        return new MockTransport();
    }
}