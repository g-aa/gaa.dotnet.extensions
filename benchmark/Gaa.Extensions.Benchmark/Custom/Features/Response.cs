namespace Gaa.Extensions.Benchmark.Custom.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Пример ответа.
/// </summary>
internal readonly ref struct Response
{
    /// <summary>
    /// Текст с сообщением.
    /// </summary>
    public ReadOnlySpan<char> Message { get; init; }
}

/// <summary>
/// Пример ответа.
/// </summary>
internal sealed class AsyncResponse
{
    /// <summary>
    /// Текст с сообщением.
    /// </summary>
    public required string Message { get; init; }
}