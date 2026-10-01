using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Контекст для конфигурирования <see cref="IPublisher"/> и <see cref="ITransport"/>.
/// </summary>
public sealed class BusConfigurationBuilder
{
    private readonly IServiceCollection _services;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="BusConfigurationBuilder"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public BusConfigurationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// Коллекция сервисов.
    /// </summary>
    public IServiceCollection Services => _services;

    /// <summary>
    /// Регистрирует компоненты транспортной шины в памяти в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <param name="configureOptions">Настройки транспортной шины.</param>
    /// <returns>Контекст конфигурирования.</returns>
    public TransportConfigurationBuilder AddInMemoryTransport(
        string transportName,
        Action<InMemoryTransportOptions> configureOptions) => Services.AddInMemoryTransport(transportName, configureOptions);

    /// <summary>
    /// Регистрирует компоненты конкурентной транспортной шины в памяти в коллекции сервисов <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <param name="configureOptions">Настройки транспортной шины.</param>
    /// <returns>Контекст конфигурирования.</returns>
    public TransportConfigurationBuilder AddConcurrentTransport(
        string transportName,
        Action<ConcurrentTransportOptions> configureOptions) => Services.AddConcurrentTransport(transportName, configureOptions);
}