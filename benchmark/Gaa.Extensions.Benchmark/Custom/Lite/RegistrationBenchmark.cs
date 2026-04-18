using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

using Gaa.Extensions.Benchmark.Custom.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Benchmark.Custom.Lite;

#pragma warning disable CA1515
#pragma warning disable CA1822
#pragma warning disable CS8618

/// <summary>
/// Контрольный тест.
/// </summary>
[Orderer(SummaryOrderPolicy.Declared)]
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
            .AddMediatorLite()
            .AddHandler<WithoutResponse.Handler, WithoutResponse.Request>()
            .AddHandler<WithoutResponse2.Handler, WithoutResponse2.Request>()
            .AddHandler<WithoutResponse3.Handler, WithoutResponse3.Request>()

            .AddHandler<WithResponse.Handler, WithResponse.Request>()
            .AddHandler<WithResponse2.Handler, WithResponse2.Request>()
            .AddHandler<WithResponse3.Handler, WithResponse3.Request>()

            .AddAsyncHandler<AsyncWithoutResponse.Handler, AsyncWithoutResponse.Request>()
            .AddAsyncHandler<AsyncWithoutResponse2.Handler, AsyncWithoutResponse2.Request>()
            .AddAsyncHandler<AsyncWithoutResponse3.Handler, AsyncWithoutResponse3.Request>()

            .AddAsyncHandler<AsyncWithResponse.Handler, AsyncWithResponse.Request>()
            .AddAsyncHandler<AsyncWithResponse2.Handler, AsyncWithResponse2.Request>()
            .AddAsyncHandler<AsyncWithResponse3.Handler, AsyncWithResponse3.Request>()
            .Services;

        // act
        _ = services.BuildServiceProvider();
    }
}