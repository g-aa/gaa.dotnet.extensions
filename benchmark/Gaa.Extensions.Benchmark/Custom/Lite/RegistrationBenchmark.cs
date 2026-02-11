using BenchmarkDotNet.Attributes;

using Gaa.Extensions.Benchmark.Custom.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Benchmark.Custom.Lite;

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
            .AddMediatorLite(ServiceLifetime.Scoped)
            .AddHandler<WithoutResponse.Handler, WithoutResponse.Request>()
            .AddHandler<WithResponse.Handler, WithResponse.Request, Response>()
            .AddAsyncHandler<AsyncWithoutResponse.Handler, AsyncWithoutResponse.Request>()
            .AddAsyncHandler<AsyncWithResponse.Handler, AsyncWithResponse.Request, Response>()
            .Services;

        // act
        _ = services.BuildServiceProvider();
    }
}