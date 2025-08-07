namespace Gaa.Extensions.DotNet;

/// <summary>
/// Анонимное освобождение ресурсов.
/// </summary>
public sealed class AnonymousAsyncDisposable
    : IAsyncDisposable
{
    private Func<Task>? _func;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AnonymousAsyncDisposable"/>.
    /// </summary>
    /// <param name="func">Выполняемое действие.</param>
    public AnonymousAsyncDisposable(Func<Task>? func)
    {
        _func = func;
    }

    /// <summary>
    /// Выполнено ли освобождение ресурсов.
    /// </summary>
    public bool IsDisposed => _func == default;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        var func = Interlocked.Exchange(ref _func, default);
        await (func?.Invoke() ?? Task.CompletedTask);
    }
}