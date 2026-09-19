#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Селектор для выбора транспортной шины.
/// </summary>
public interface ITransportSelector
{
    /// <summary>
    /// Предоставляет транспортную шину по наименованию.
    /// </summary>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <returns>Экземпляр транспортной шины.</returns>
    ITransport GetTransport(string transportName);

    /// <summary>
    /// Пытается предоставить конкретный тип шины по наименованию.
    /// </summary>
    /// <typeparam name="T">Требуемый тип шины.</typeparam>
    /// <param name="transportName">Наименование транспортной шины.</param>
    /// <returns>Экземпляр транспортной шины.</returns>
    T GetTransport<T>(string transportName)
        where T : ITransport;
}