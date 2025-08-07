# Gaa.Extensions.DotNet

Пакет предоставляет набор базовых расширений функционала в проектах `.NET Core`.

## Распределенная блокировка

```cs
...
services.AddSingleton<IDistributedLockProvider, SingleNodeLockProvider>();
...
var lockProvider = serviceProvider.GetService<IDistributedLockProvider>();
await using (await distributedLockProvider.Lock("SomeLockId", CancellationToken.None))
{
    // do some locked things
}
```