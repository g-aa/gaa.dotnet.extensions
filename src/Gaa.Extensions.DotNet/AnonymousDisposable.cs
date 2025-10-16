namespace Gaa.Extensions;

/// <summary>
/// Анонимное освобождение ресурсов.
/// </summary>
public sealed class AnonymousDisposable
    : IDisposable
{
    private Action? _action;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AnonymousDisposable"/>.
    /// </summary>
    /// <param name="action">Выполняемое действие.</param>
    public AnonymousDisposable(Action? action)
    {
        _action = action;
    }

    /// <summary>
    /// Выполнено ли освобождение ресурсов.
    /// </summary>
    public bool IsDisposed => _action == default;

    /// <inheritdoc />
    public void Dispose()
    {
        var action = Interlocked.Exchange(ref _action, default);
        action?.Invoke();
    }
}