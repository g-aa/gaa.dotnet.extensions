namespace Gaa.Extensions.Benchmark.Features;

#pragma warning disable CA1812

/// <summary>
/// Предварительный обработчик запросов.
/// </summary>
internal sealed class RequestPreProcessor
    : IRequestPreProcessor<WithResponse.Request>
{
    private readonly TextWriter _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RequestPreProcessor"/>.
    /// </summary>
    /// <param name="writer">Ввод вывод данных.</param>
    public RequestPreProcessor(
        TextWriter writer)
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