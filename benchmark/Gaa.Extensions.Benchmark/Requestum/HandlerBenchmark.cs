using BenchmarkDotNet.Attributes;

using Gaa.Extensions.Benchmark.Requestum.Features;

using Microsoft.Extensions.DependencyInjection;

using Requestum;

namespace Gaa.Extensions.Benchmark.Requestum;

#pragma warning disable CA1515
#pragma warning disable CS8618

/// <summary>
/// Контрольный тест.
/// </summary>
[MemoryDiagnoser]
public class HandlerBenchmark
{
    private IServiceScope _scope;

    private IRequestum _requestum;

    /// <summary>
    /// Глобально настраивает окружение.
    /// </summary>
    [GlobalSetup]
    public void GlobalSetup()
    {
        var provider = new ServiceCollection()
            .AddSingleton(TextWriter.Null)
            .AddRequestum(cfg =>
            {
                cfg.RegisterHandler<WithoutResponse.Handler>();
                cfg.RegisterHandler<WithResponse.Handler>();
                cfg.RegisterHandler<AsyncWithoutResponse.Handler>();
                cfg.RegisterHandler<AsyncWithResponse.Handler>();
            })
            .BuildServiceProvider();

        _scope = provider.CreateScope();
        _requestum = _scope.ServiceProvider.GetRequiredService<IRequestum>();
    }

    /// <summary>
    /// Глобально освобождает ресурсы.
    /// </summary>
    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _scope.Dispose();
    }

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Benchmark]
    public Task SendingRequestWithoutResponseAsync()
    {
        // arrange
        var command = new AsyncWithoutResponse.Command { Message = "Input message!" };

        // act
        return _requestum.ExecuteAsync(command, default);
    }

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Benchmark]
    public Task SendingRequestWithResponseAsync()
    {
        // arrange
        var query = new AsyncWithResponse.Query { Message = "Input message!" };

        // act
        return _requestum.HandleAsync<AsyncWithResponse.Query, Response>(query, default);
    }
}