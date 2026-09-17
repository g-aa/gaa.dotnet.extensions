#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Селектор для выбора наименования транспортной шины.
/// </summary>
public interface ITransportNameSelector
{
    /// <summary>
    /// Предоставляет наименование <see cref="ITransport"/> по типу <typeparamref name="TMessage"/>.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <returns>Наименование транспортной шины.</returns>
    string? GetTransportName<TMessage>();
}