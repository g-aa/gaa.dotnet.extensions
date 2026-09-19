using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Gaa.Extensions.Observer;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Benchmark.Observer;

#pragma warning disable CS8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.

/// <summary>
/// Контрольный тест.
/// </summary>
[Orderer(SummaryOrderPolicy.Declared)]
[MemoryDiagnoser]
public class SelectionBenchmark
{
    private ServiceProvider _provider;

    private DefaultTransportNameSelector _selector;

    /// <summary>
    /// Глобально настраивает окружение.
    /// </summary>
    [GlobalSetup]
    public void GlobalSetup()
    {
        _provider = new ServiceCollection()
            .Configure<BusOptions>(options =>
            {
                options.Subscriptions.Add("Guid", [typeof(Guid)]);
                options.Subscriptions.Add("Bool", [typeof(bool)]);
                options.Subscriptions.Add("Char", [typeof(char)]);
                options.Subscriptions.Add("Integer", [typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong)]);
                options.Subscriptions.Add("Float", [typeof(float), typeof(double), typeof(decimal)]);
            })
            .AddSingleton<DefaultTransportNameSelector>()
            .BuildServiceProvider();

        _selector = _provider.GetRequiredService<DefaultTransportNameSelector>();
    }

    /// <summary>
    /// Глобально освобождает ресурсы.
    /// </summary>
    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _provider.Dispose();
    }

    /// <summary>
    /// Публикует и потребляет сообщение.
    /// </summary>
    [Benchmark]
    public void GetTransportName()
    {
        // arrange & act
        _ = _selector.GetTransportName<long>();
    }
}