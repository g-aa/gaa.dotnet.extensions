#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Настройки транспортной шины.
/// </summary>
public abstract class TransportOptions
{
    /// <summary>
    /// Наименование.
    /// </summary>
    public string Name { get; init; } = "Default";
}