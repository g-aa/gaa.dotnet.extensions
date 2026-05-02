using Requestum.Contract;

namespace Gaa.Extensions.Benchmark.Requestum.Features;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Промежуточное ПО для запроса.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
internal sealed class RequestMiddleware<TRequest, TResponse> : IRequestMiddleware<TRequest, TResponse>
{
    private readonly TextWriter _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestMiddleware{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="writer">Ввод вывод данных.</param>
    public RequestMiddleware(TextWriter writer)
    {
        _writer = writer;
    }

    /// <inheritdoc />
    public TResponse Invoke(
        TRequest request,
        RequestNextDelegate<TRequest, TResponse> next)
    {
        _writer.Write(request?.ToString());
        var response = next.Invoke(request);
        _writer.Write(response?.ToString());
        return response;
    }
}

/// <summary>
/// Промежуточное ПО для запроса.
/// </summary>
/// <typeparam name="TRequest">Тип запроса.</typeparam>
/// <typeparam name="TResponse">Тип ответа.</typeparam>
internal sealed class AsyncRequestMiddleware<TRequest, TResponse> : IAsyncRequestMiddleware<TRequest, TResponse>
{
    private readonly TextWriter _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestMiddleware{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="writer">Ввод вывод данных.</param>
    public AsyncRequestMiddleware(TextWriter writer)
    {
        _writer = writer;
    }

    /// <inheritdoc />
    public async Task<TResponse> InvokeAsync(
        TRequest request,
        AsyncRequestNextDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken = default)
    {
        await _writer.WriteAsync(request?.ToString());
        var response = await next.InvokeAsync(request);
        await _writer.WriteAsync(response?.ToString());
        return response;
    }
}