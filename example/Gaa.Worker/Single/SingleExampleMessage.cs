namespace Gaa.Worker.Single;

/// <summary>
/// Пример сообщения.
/// </summary>
public sealed record SingleExampleMessage
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Текст сообщения.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public required long CreationTime { get; init; }
}