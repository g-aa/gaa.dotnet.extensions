using Gaa.Extensions.Observer.Test.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Gaa.Extensions.Observer.Test;

/// <summary>
/// Вспомогательные метолы для тестирования.
/// </summary>
internal static class TestHelper
{
    /// <summary>
    /// Регистрирует <see cref="IMessageLogger"/> в коллекции сервисов.
    /// </summary>
    /// <param name="services">Исходная коллекция сервисов.</param>
    /// <returns>Модифицированная колекция сервисов.</returns>
    internal static IServiceCollection AddMessageLogger(this IServiceCollection services)
    {
        return services
            .AddSingleton(p =>
            {
                var mockLog = new Mock<IMessageLogger>();
                mockLog.Setup(l => l.Log(It.IsAny<string>()));
                return mockLog;
            })
            .AddTransient(p => p.GetRequiredService<Mock<IMessageLogger>>().Object);
    }
}