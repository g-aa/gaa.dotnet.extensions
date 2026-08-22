namespace Gaa.Worker.Messages;

/// <summary>
/// Пример мсообщения.
/// </summary>
public sealed record ExampleMessage
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
    public required DateTimeOffset CreationTime { get; init; }
}