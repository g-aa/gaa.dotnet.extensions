using BenchmarkDotNet.Attributes;
using Gaa.Extensions.Benchmark.Custom.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Benchmark.Custom.Standard;

#pragma warning disable CA1515
#pragma warning disable CS8618

/// <summary>
/// Контрольный тест.
/// </summary>
[MemoryDiagnoser]
public class HandlerBenchmark
{
    private IServiceScope _scope;

    private IMediator _mediator;

    /// <summary>
    /// Глобально настраивает окружение.
    /// </summary>
    [GlobalSetup]
    public void GlobalSetup()
    {
        var provider = new ServiceCollection()
            .AddSingleton(TextWriter.Null)
            .AddScopedMediator()
            .AddHandler<WithoutResponse.Handler, WithoutResponse.Request>()
            .AddHandler<WithResponse.Handler, WithResponse.Request, Response>()
            .AddAsyncHandler<AsyncWithoutResponse.Handler, AsyncWithoutResponse.Request>()
            .AddAsyncHandler<AsyncWithResponse.Handler, AsyncWithResponse.Request, Response>()
            .Services
            .BuildServiceProvider();

        _scope = provider.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
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
        var request = new AsyncWithoutResponse.Request { Message = "Input message!" };

        // act
        return _mediator.SendAsync(request, default);
    }

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Benchmark]
    public Task SendingRequestWithResponseAsync()
    {
        // arrange
        var request = new AsyncWithResponse.Request { Message = "Input message!" };

        // act
        return _mediator.SendAsync<AsyncWithResponse.Request, Response>(request, default);
    }
}