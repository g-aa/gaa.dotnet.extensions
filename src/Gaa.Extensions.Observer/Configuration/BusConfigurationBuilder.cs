using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст для конфигурирования <see cref="IPublisher"/> и <see cref="ITransport"/>.
/// </summary>
public sealed class BusConfigurationBuilder
{
    /// <summary>
    /// Коллекция сервисов.
    /// </summary>
    public IServiceCollection Services { get; init; } = null!;

    /// <summary>
    /// Регистрирует компоненты транспортной шины в памяти в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <param name="configureOptions">Настройки транспортной шины.</param>
    /// <returns>Контекст конфигурирования.</returns>
    public TransportConfigurationBuilder InMemoryTransport(
        string transportName,
        Action<InMemoryTransportOptions> configureOptions) => Services.InMemoryTransport(transportName, configureOptions);
}