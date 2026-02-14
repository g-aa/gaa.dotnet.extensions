using BenchmarkDotNet.Attributes;

using Gaa.Extensions.Benchmark.Requestum.Features;

using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Benchmark.Requestum;

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
            });

        // act
        _ = services.BuildServiceProvider();
    }
}