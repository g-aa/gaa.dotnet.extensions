using BenchmarkDotNet.Attributes;

using Gaa.Extensions.Benchmark.MediatR.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Benchmark.MediatR;

#pragma warning disable CA1515
#pragma warning disable CA1822
#pragma warning disable CS8618

/// <summary>
/// Контрольный тест.
/// </summary>
[MemoryDiagnoser]
public class RegistrationBenchmark
{
    /// <summary>
    /// Регистрирует Mediator в коллекции сервисов.
    /// </summary>
    [Benchmark]
    public void RegisterAndBuild()
    {
        // arrange
        var services = new ServiceCollection()
            .AddSingleton(TextWriter.Null)
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.AddRequestPreProcessor<RequestPreProcessor>();
                cfg.AddRequestPostProcessor<RequestPostProcessor>();
            });

        // act
        _ = services.BuildServiceProvider();
    }
}