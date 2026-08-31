namespace Gaa.Worker.Messages;

/// <summary>
/// Второе сообщение.
/// </summary>
public sealed record SecondMessage
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