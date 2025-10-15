namespace Gaa.Extensions.Test.Features;

/// <summary>
/// Пример запроса.
/// </summary>
public class ExampleRequestWithResponse
    : IRequest<ExampleResponse>
{
    /// <summary>
    /// Текст с сообщением.
    /// </summary>
    public string Message { get; init; } = "Test message from response!";
}