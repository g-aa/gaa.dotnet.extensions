namespace Gaa.Extensions.Benchmark.Custom.Features;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
internal sealed class RequestPreProcessor : IRequestPreProcessor<WithResponse.Request>
{
    private readonly TextWriter _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestPreProcessor"/>.
    /// </summary>
    /// <param name="writer">Ввод вывод данных.</param>
    public RequestPreProcessor(TextWriter writer)
    {
        _writer = writer;
    }

    /// <inheritdoc />
    public void Process(
        WithResponse.Request request,
        CancellationToken cancellationToken)
    {
        _writer.Write(request.Message);
    }
}

/// <summary>
/// Постпроцессор запросов.
/// </summary>
internal sealed class RequestPostProcessor : IRequestPostProcessor<WithResponse.Request, Response>
{
    private readonly TextWriter _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestPostProcessor"/>.
    /// </summary>
    /// <param name="writer">Ввод вывод данных.</param>
    public RequestPostProcessor(TextWriter writer)
    {
        _writer = writer;
    }

    /// <inheritdoc />
    public void Process(
        WithResponse.Request request,
        Response response,
        CancellationToken cancellationToken)
    {
        _writer.Write(request.Message);
        _writer.Write(response.Message);
    }
}

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
internal sealed class AsyncRequestPreProcessor : IAsyncRequestPreProcessor<AsyncWithResponse.Request>
{
    private readonly TextWriter _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestPreProcessor"/>.
    /// </summary>
    /// <param name="writer">Ввод вывод данных.</param>
    public AsyncRequestPreProcessor(TextWriter writer)
    {
        _writer = writer;
    }

    /// <inheritdoc />
    public Task ProcessAsync(
        AsyncWithResponse.Request request,
        CancellationToken cancellationToken)
    {
        return _writer.WriteAsync(request.Message);
    }
}

/// <summary>
/// Постпроцессор запросов.
/// </summary>
internal sealed class AsyncRequestPostProcessor : IAsyncRequestPostProcessor<AsyncWithResponse.Request, AsyncResponse>
{
    private readonly TextWriter _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AsyncRequestPostProcessor"/>.
    /// </summary>
    /// <param name="writer">Ввод вывод данных.</param>
    public AsyncRequestPostProcessor(TextWriter writer)
    {
        _writer = writer;
    }

    /// <inheritdoc />
    public async Task ProcessAsync(
        AsyncWithResponse.Request request,
        AsyncResponse response,
        CancellationToken cancellationToken)
    {
        await _writer.WriteAsync(request.Message);
        await _writer.WriteAsync(response.Message);
    }
}