#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фабрика дочерних шин.
/// </summary>
public interface IBackgroundTaskBusFactory
{
    /// <summary>
    /// Создает дочернюю шину.
    /// </summary>
    /// <param name="busName">Наименование шины.</param>
    /// <returns>Дочерняя шина.</returns>
    IBackgroundTaskBus GetOrCreate(string busName);
}