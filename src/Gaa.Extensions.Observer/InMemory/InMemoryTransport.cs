using System.Threading.Channels;

using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Транспортная шина в памяти.
/// </summary>
internal sealed partial class InMemoryTransport : ITransport
{
    private readonly int _capacity;

    private readonly string _name;

    private readonly ILogger _log;

    private readonly Channel<IMessageExecutionContext> _queue;

    private readonly ChannelReader<IMessageExecutionContext> _reader;

    private readonly ChannelWriter<IMessageExecutionContext> _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="InMemoryTransport"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика журналов протоколирования событий.</param>
    /// <param name="options">Настройки шины сообщений.</param>
    public InMemoryTransport(ILoggerFactory loggerFactory, InMemoryTransportOptions options)
    {
        _name = options.Name;
        _capacity = options.Capacity;
        var channelOptions = new BoundedChannelOptions(_capacity)
        {
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false,
        };

        _log = loggerFactory.CreateLogger(CategoryName.DefaultBus);
        _queue = Channel.CreateBounded<IMessageExecutionContext>(channelOptions);
        _reader = _queue.Reader;
        _writer = _queue.Writer;
    }

    /// <inheritdoc />
    public string Name => _name;

    /// <summary>
    /// Количество сообщений в шине.
    /// </summary>
    public int Count => _reader.Count;

    /// <summary>
    /// Емкость шины сообщений.
    /// </summary>
    public int Capacity => _capacity;

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(MessageContext<TMessage> message, CancellationToken cancellationToken)
        where TMessage : notnull
    {
        var context = new MessageExecutionContext<TMessage>(message);
        return WriteAsync(context, cancellationToken);
    }

    /// <summary>
    /// Записывает контекст сообщения в шину для дальнейшего исполнения.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="context">Контекст с сообщением.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    public async Task WriteAsync<TMessage>(MessageExecutionContext<TMessage> context, CancellationToken cancellationToken)
        where TMessage : notnull
    {
        await _writer.WriteAsync(context, cancellationToken);
        Log.CompletionOfWritingMessage(_log, context);
    }

    /// <summary>
    /// Считывает контекст сообщения из шины для исполнения.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Контекст сообщения.</returns>
    public async Task<IMessageExecutionContext> ReadAsync(CancellationToken cancellationToken)
    {
        var context = await _reader.ReadAsync(cancellationToken);
        Log.CompletionOfReadingMessage(_log, context);
        return context;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Transport name: {_name}.";
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Trace, Message = "Контекст с сообщением '{Context}' добавлен в шину для обработки.")]
        public static partial void CompletionOfWritingMessage(ILogger log, IMessageExecutionContext context);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Контекст с сообщением '{Context}' излечен из шины для обработки.")]
        public static partial void CompletionOfReadingMessage(ILogger log, IMessageExecutionContext context);
    }
}