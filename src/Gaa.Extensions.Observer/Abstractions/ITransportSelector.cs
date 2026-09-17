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
    /// <returns>Транспортная шина.</returns>
    ITransport GetTransport(string transportName);
}