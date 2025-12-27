namespace Gaa.Extensions.Test.Features;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Пример запроса.
/// </summary>
internal sealed class AsyncRequestWithoutResponse
    : IAsyncRequest
{
    /// <summary>
    /// Текст с сообщением.
    /// </summary>
    public string Message { get; init; } = "Test message from async request!";
}

/// <summary>
/// Пример запроса.
/// </summary>
internal sealed class AsyncRequestWithResponse
    : IAsyncRequest<Response>
{
    /// <summary>
    /// Текст с сообщением.
    /// </summary>
    public string Message { get; init; } = "Test message from async request!";
}