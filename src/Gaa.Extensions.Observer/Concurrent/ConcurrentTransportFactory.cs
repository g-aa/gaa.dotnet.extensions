using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фабрика для транспортной шины в памяти.
/// </summary>
internal sealed class ConcurrentTransportFactory : ITransportFactory
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ConcurrentTransportFactory"/>.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public ConcurrentTransportFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    /// <inheritdoc />
    public Type Type => typeof(ConcurrentTransport);

    /// <inheritdoc />
    public ITransport CreateTransport(TransportOptions options)
    {
        if (options is not ConcurrentTransportOptions transportOptions)
        {
            throw new InvalidOperationException($"Не удалось получить настройки вида '{typeof(ConcurrentTransportOptions)}' для транспортной шины '{options.Name}'!");
        }

        var loggerFactory = _provider.GetRequiredService<ILoggerFactory>();
        return new ConcurrentTransport(loggerFactory, transportOptions);
    }
}