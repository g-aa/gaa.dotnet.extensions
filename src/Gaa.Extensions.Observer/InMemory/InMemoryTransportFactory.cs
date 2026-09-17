using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фабрика для транспортной шины в памяти.
/// </summary>
internal sealed class InMemoryTransportFactory : ITransportFactory
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="InMemoryTransportFactory"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public InMemoryTransportFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    /// <inheritdoc />
    public ITransport CreateTransport(TransportOptions? options)
    {
        if (options == null)
        {
            throw new InvalidOperationException("Для создание экземпляра транспортной шины в памяти требуются настройки!");
        }

        if (options is not InMemoryTransportOptions transportOptions)
        {
            throw new InvalidOperationException($"Не удалось получить настройки вида '{typeof(InMemoryTransportOptions)}' для транспортной шины '{options.Name}'!");
        }

        var loggerFactory = _provider.GetRequiredService<ILoggerFactory>();
        return new InMemoryTransport(loggerFactory, transportOptions);
    }
}