namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Пример запроса.
/// </summary>
public class ExampleRequest
    : IRequest
{
    /// <summary>
    /// Текст с сообщением.
    /// </summary>
    public string Message { get; init; } = "Test message from request!";
}