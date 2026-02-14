namespace Gaa.Extensions.Benchmark.Requestum.Features;

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