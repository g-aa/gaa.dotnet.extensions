using BenchmarkDotNet.Running;

namespace Gaa.Extensions.Benchmark;

/// <summary>
/// Базовый класс приложения.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Точка входа в приложение.
    /// </summary>
    /// <param name="args">Аргументы для запуска приложения.</param>
    public static void Main(string[] args)
    {
        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args);
    }
}