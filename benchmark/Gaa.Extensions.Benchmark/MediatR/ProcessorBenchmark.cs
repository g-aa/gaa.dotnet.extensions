using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

using Gaa.Extensions.Benchmark.MediatR.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Benchmark.MediatR;

#pragma warning disable CS8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.

/// <summary>
/// Контрольный тест.
/// </summary>
[Orderer(SummaryOrderPolicy.Declared)]
[MemoryDiagnoser]
public class ProcessorBenchmark
{
    private const string _message = "Input message!";

    private IServiceScope _scope;

    private global::MediatR.IMediator _mediator;

    /// <summary>
    /// Глобально настраивает окружение.
    /// </summary>
    [GlobalSetup]
    public void GlobalSetup()
    {
        var provider = new ServiceCollection()
            .AddSingleton(TextWriter.Null)
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.AddRequestPreProcessor<RequestPreProcessor>();
                cfg.AddRequestPostProcessor<RequestPostProcessor>();
            })
            .BuildServiceProvider();

        _scope = provider.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<global::MediatR.IMediator>();
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
        var request = new AsyncWithoutResponse.Request { Message = _message };

        // act
        return _mediator.Send(request, default);
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
        return _mediator.Send(request, default);
    }
}