using Gaa.Extensions.Observer;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;

namespace Gaa.Worker;

/// <summary>
/// Базовый класс приложения.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Точка входа в приложение.
    /// </summary>
    /// <param name="args">Аргументы запуска приложения.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    internal static async Task Main(string[] args)
    {
        var log = LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();

        try
        {
            log.Info("Сервис запущен на выполнение...");

            var builder = Host.CreateApplicationBuilder(args);

            builder.Logging.ClearProviders().SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace).AddNLog();
            Startup.ConfigureServices(builder.Services, builder.Configuration);

            var host = builder.Build();
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            log.Error(ex, "Сервис остановлен из за перехвата не необработанного исключения!");
        }
        finally
        {
            log.Info("Сервис остановлен.");
            LogManager.Shutdown();
        }
    }
}