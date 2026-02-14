namespace Gaa.Extensions.Benchmark.MediatR.Features;

/// <summary>
/// Пример ответа.
/// </summary>
internal sealed class Response
{
    /// <summary>
    /// Текст с сообщением.
    /// </summary>
    public required string Message { get; init; }
}