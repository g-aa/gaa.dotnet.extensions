namespace Gaa.Extensions.DotNet;

/// <summary>
/// Методы расширения для шаблонов.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Преобразует значение в список содержащий это значение.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="value">Значение.</param>
    /// <returns>Список с значением.</returns>
#pragma warning disable CA1002 // Не предоставляйте универсальные списки
    public static List<T> ToListWithValue<T>(this T value)
        where T : notnull
    {
        return new List<T> { value };
    }
#pragma warning restore CA1002 // Не предоставляйте универсальные списки

    /// <summary>
    /// Преобразует значение в массив содержащий значение.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="value">Значение.</param>
    /// <returns>Массив с значением.</returns>
    public static T[] ToArrayWithValue<T>(this T value)
        where T : notnull
    {
        return new T[] { value };
    }
}