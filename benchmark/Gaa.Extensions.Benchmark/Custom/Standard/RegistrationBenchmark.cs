using BenchmarkDotNet.Attributes;

using Gaa.Extensions.Benchmark.Custom.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Benchmark.Custom.Standard;

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
            .AddMediator()
            .AddHandler<WithoutResponse.Handler, WithoutResponse.Request>()
            .AddHandler<WithoutResponse2.Handler, WithoutResponse2.Request>()
            .AddHandler<WithoutResponse3.Handler, WithoutResponse3.Request>()

            .AddHandler<WithResponse.Handler, WithResponse.Request, Response>()
                .AddPreProcessor<RequestPreProcessor>()
                .AddPostProcessor<RequestPostProcessor>()
            .AddHandler<WithResponse2.Handler, WithResponse2.Request, Response>()
            .AddHandler<WithResponse3.Handler, WithResponse3.Request, Response>()

            .AddAsyncHandler<AsyncWithoutResponse.Handler, AsyncWithoutResponse.Request>()
            .AddAsyncHandler<AsyncWithoutResponse2.Handler, AsyncWithoutResponse2.Request>()
            .AddAsyncHandler<AsyncWithoutResponse3.Handler, AsyncWithoutResponse3.Request>()

            .AddAsyncHandler<AsyncWithResponse.Handler, AsyncWithResponse.Request, Response>()
                .AddAsyncPreProcessor<AsyncRequestPreProcessor>()
                .AddAsyncPostProcessor<AsyncRequestPostProcessor>()
            .AddAsyncHandler<AsyncWithResponse2.Handler, AsyncWithResponse2.Request, Response>()
            .AddAsyncHandler<AsyncWithResponse3.Handler, AsyncWithResponse3.Request, Response>()
            .Services;

        // act
        _ = services.BuildServiceProvider();
    }
}