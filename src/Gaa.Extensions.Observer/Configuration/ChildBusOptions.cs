#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Настройки дочерней шины сообщений и событий.
/// </summary>
public sealed class ChildBusOptions
{
    /// <summary>
    /// Наименовани шины.
    /// </summary>
    public string Name { get; init; } = "Default";

    /// <summary>
    /// Емкость очереди шины.
    /// </summary>
    public int Capacity { get; set; } = 1_000;
}