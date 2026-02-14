namespace Gaa.Extensions.Benchmark.MediatR.Features;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
internal sealed class RequestPreProcessor : global::MediatR.Pipeline.IRequestPreProcessor<AsyncWithResponse.Request>
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
    public Task Process(
        AsyncWithResponse.Request request,
        CancellationToken cancellationToken)
    {
        return _writer.WriteAsync(request.Message);
    }
}

/// <summary>
/// Постпроцессор запросов.
/// </summary>
internal sealed class RequestPostProcessor : global::MediatR.Pipeline.IRequestPostProcessor<AsyncWithResponse.Request, Response>
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
    public async Task Process(
        AsyncWithResponse.Request request,
        Response response,
        CancellationToken cancellationToken)
    {
        await _writer.WriteAsync(request.Message);
        await _writer.WriteAsync(response.Message);
    }
}