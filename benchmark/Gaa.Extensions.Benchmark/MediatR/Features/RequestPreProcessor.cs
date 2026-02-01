namespace Gaa.Extensions.Benchmark.MediatR.Features;

#pragma warning disable CA1849
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
internal sealed class RequestPreProcessor : global::MediatR.Pipeline.IRequestPreProcessor<WithResponse.Request>
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
        WithResponse.Request request,
        CancellationToken cancellationToken)
    {
        _writer.Write(request.Message);
        return Task.CompletedTask;
    }
}

/// <summary>
/// Постпроцессор запросов.
/// </summary>
internal sealed class RequestPostProcessor : global::MediatR.Pipeline.IRequestPostProcessor<WithResponse.Request, Response>
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
    public Task Process(
        WithResponse.Request request,
        Response response,
        CancellationToken cancellationToken)
    {
        _writer.Write(request.Message);
        _writer.Write(response.Message);
        return Task.CompletedTask;
    }
}