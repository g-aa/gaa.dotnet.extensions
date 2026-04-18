using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

using Gaa.Extensions.Benchmark.Custom.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Benchmark.Custom.Standard;

#pragma warning disable CA1515
#pragma warning disable CS8618

/// <summary>
/// Контрольный тест.
/// </summary>
[Orderer(SummaryOrderPolicy.Declared)]
[MemoryDiagnoser]
public class ProcessorBenchmark
{
    private const string _message = "Input message!";

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
            .AddMediator(ServiceLifetime.Singleton, ServiceLifetime.Singleton, ServiceLifetime.Singleton)
            .AddHandler<WithoutResponse.Handler, WithoutResponse.Request>()
            .AddHandler<WithoutResponse2.Handler, WithoutResponse2.Request>()
            .AddHandler<WithoutResponse3.Handler, WithoutResponse3.Request>()

            .AddHandler<WithResponse.Handler, WithResponse.Request, Response>(b => b
                .AddPreProcessor<RequestPreProcessor>()
                .AddPostProcessor<RequestPostProcessor>())
            .AddHandler<WithResponse2.Handler, WithResponse2.Request>()
            .AddHandler<WithResponse3.Handler, WithResponse3.Request>()

            .AddAsyncHandler<AsyncWithoutResponse.Handler, AsyncWithoutResponse.Request>()
            .AddAsyncHandler<AsyncWithoutResponse2.Handler, AsyncWithoutResponse2.Request>()
            .AddAsyncHandler<AsyncWithoutResponse3.Handler, AsyncWithoutResponse3.Request>()

            .AddAsyncHandler<AsyncWithResponse.Handler, AsyncWithResponse.Request, AsyncResponse>(b => b
                .AddAsyncPreProcessor<AsyncRequestPreProcessor>()
                .AddAsyncPostProcessor<AsyncRequestPostProcessor>())
            .AddAsyncHandler<AsyncWithResponse2.Handler, AsyncWithResponse2.Request>()
            .AddAsyncHandler<AsyncWithResponse3.Handler, AsyncWithResponse3.Request>()
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
    /// Отправить синхронный запрос.
    /// </summary>
    [Benchmark]
    public void SendingRequestWithoutResponse()
    {
        // arrange
        var request = new WithoutResponse.Request { Message = _message };

        // act
        _mediator.RequiredSend(request, default);
    }

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Benchmark]
    public Task SendingRequestWithoutResponseAsync()
    {
        // arrange
        var request = new AsyncWithoutResponse.Request { Message = _message };

        // act
        return _mediator.RequiredSendAsync(request, default);
    }

    /// <summary>
    /// Отправить синхронный запрос.
    /// </summary>
    [Benchmark]
    public void SendingRequestWithResponse()
    {
        // arrange
        var request = new WithResponse.Request { Message = _message };

        // act
        _mediator.RequiredSend<WithResponse.Request, Response>(request, default);
    }

    /// <summary>
    /// Отправить асинхронный запрос.
    /// </summary>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    [Benchmark]
    public Task SendingRequestWithResponseAsync()
    {
        // arrange
        var request = new AsyncWithResponse.Request { Message = _message };

        // act
        return _mediator.RequiredSendAsync<AsyncWithResponse.Request, AsyncResponse>(request, default);
    }
}