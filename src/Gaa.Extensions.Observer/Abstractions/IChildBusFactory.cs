#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Фабрика дочерних шин.
/// </summary>
/// <typeparam name="TBus">Тип дочерней шины.</typeparam>
internal interface IChildBusFactory<out TBus>
    where TBus : IChildBus
{
    /// <summary>
    /// Создает дочернюю шину.
    /// </summary>
    /// <param name="name">Наименование шины.</param>
    /// <returns>Дочерняя шина.</returns>
    TBus GetOrCreate(string name);
}