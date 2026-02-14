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
    private const string _message = "Input message!";

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
                cfg.RegisterHandler<WithoutResponse2.Handler>();
                cfg.RegisterHandler<WithoutResponse3.Handler>();

                cfg.RegisterHandler<WithResponse.Handler>();
                cfg.RegisterHandler<WithResponse2.Handler>();
                cfg.RegisterHandler<WithResponse3.Handler>();

                cfg.RegisterHandler<AsyncWithoutResponse.Handler>();
                cfg.RegisterHandler<AsyncWithoutResponse2.Handler>();
                cfg.RegisterHandler<AsyncWithoutResponse3.Handler>();

                cfg.RegisterHandler<AsyncWithResponse.Handler>();
                cfg.RegisterHandler<AsyncWithResponse2.Handler>();
                cfg.RegisterHandler<AsyncWithResponse3.Handler>();
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
    /// Отправить синхронный запрос.
    /// </summary>
    [Benchmark]
    public void SendingRequestWithoutResponse()
    {
        // arrange
        var command = new WithoutResponse.Command { Message = _message };

        // act
        _requestum.Execute(command);
    }

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Benchmark]
    public Task SendingRequestWithoutResponseAsync()
    {
        // arrange
        var command = new AsyncWithoutResponse.Command { Message = _message };

        // act
        return _requestum.ExecuteAsync(command, default);
    }

    /// <summary>
    /// Отправить синхронный запрос.
    /// </summary>
    [Benchmark]
    public void SendingRequestWithResponse()
    {
        // arrange
        var query = new WithResponse.Query { Message = _message };

        // act
        _requestum.Handle<WithResponse.Query, Response>(query);
    }

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Benchmark]
    public Task SendingRequestWithResponseAsync()
    {
        // arrange
        var query = new AsyncWithResponse.Query { Message = _message };

        // act
        return _requestum.HandleAsync<AsyncWithResponse.Query, Response>(query, default);
    }
}