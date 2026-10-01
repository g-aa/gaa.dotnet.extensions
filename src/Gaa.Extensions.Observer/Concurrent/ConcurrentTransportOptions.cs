#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Настройки конкурентной транспортной шины в памяти.
/// </summary>
public sealed class ConcurrentTransportOptions : TransportOptions
{
    /// <summary>
    /// Ограничение по времени выполнения обработки одного сообщения.
    /// </summary>
    public TimeSpan ExecutionTimeLimit { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Количество потребителей.
    /// </summary>
    public int ConsumerCount { get; set; } = 2;

    /// <summary>
    /// Емкость.
    /// </summary>
    public int Capacity { get; set; } = 1_000;
}