#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фабрика транспортных шин.
/// </summary>
public interface ITransportFactory
{
    /// <summary>
    /// Создает экземпляр транспортной шины.
    /// </summary>
    /// <param name="options">Настройки транспортной шины.</param>
    /// <returns>Транспортная шина.</returns>
    ITransport CreateTransport(TransportOptions? options);
}