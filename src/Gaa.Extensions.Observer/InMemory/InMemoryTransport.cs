using System.Threading.Channels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable SA1204  // Static elements should appear before instance elements

namespace Gaa.Extensions.Observer;

/// <summary>
/// Транспортная шина в памяти.
/// </summary>
internal sealed partial class InMemoryTransport : ITransport
{
    private readonly string _name;

    private readonly TimeSpan _executionTimeLimit;

    private readonly ILogger _log;

    private readonly Channel<IMessageExecutionContext> _channel;

    private readonly ChannelReader<IMessageExecutionContext> _reader;

    private readonly ChannelWriter<IMessageExecutionContext> _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="InMemoryTransport"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика журналов протоколирования событий.</param>
    /// <param name="options">Настройки транспортной шины в памяти.</param>
    public InMemoryTransport(ILoggerFactory loggerFactory, InMemoryTransportOptions options)
    {
        _name = options.Name;
        _executionTimeLimit = options.ExecutionTimeLimit;
        var channelOptions = new BoundedChannelOptions(options.Capacity)
        {
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false,
        };

        _log = loggerFactory.CreateLogger(CategoryName.InMemory);
        _channel = Channel.CreateBounded<IMessageExecutionContext>(channelOptions);
        _reader = _channel.Reader;
        _writer = _channel.Writer;
    }

    /// <inheritdoc />
    public string Name => _name;

    /// <summary>
    /// Количество сообщений в шине.
    /// </summary>
    public int Count => _reader.Count;

    /// <inheritdoc />
    public Task PublishAsync<TMessage>(MessageContext<TMessage> message, CancellationToken cancellationToken)
        where TMessage : notnull
    {
        var context = new MessageExecutionContext<TMessage>(message);
        return WriteAsync(context, cancellationToken);
    }

    /// <summary>
    /// Запускает транспортную шину на выполнение.
    /// </summary>
    /// <param name="scopeFactory">Фабрика сервисов.</param>
    /// <param name="stoppingToken">Токен останавливающий выполнение операции.</param>
    /// <returns>Результат выполнения асинхронной задачи.</returns>
    public async Task RunAsync(IServiceScopeFactory scopeFactory, CancellationToken stoppingToken)
    {
        try
        {
            while (await _reader.WaitToReadAsync(stoppingToken))
            {
                while (_reader.TryRead(out var executionContext))
                {
                    try
                    {
                        Log.ExtractedMessage(_log, executionContext);
                        using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                        cts.CancelAfter(_executionTimeLimit);
                        await executionContext.ExecuteAsync(scopeFactory, cts.Token);
                    }
                    catch (OperationCanceledException ocEx) when (!stoppingToken.IsCancellationRequested)
                    {
                        Log.TimeLimitMessage(_log, ocEx, _executionTimeLimit);
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        Log.ErrorMessage(_log, ex);
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            /* Можно не обрабатывать */
        }
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Transport name: {_name}.";
    }

    /// <summary>
    /// Считывает контекст сообщения из шины для исполнения.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Контекст сообщения.</returns>
    internal ValueTask<IMessageExecutionContext> ReadAsync(CancellationToken cancellationToken)
    {
        return _reader.ReadAsync(cancellationToken);
    }

    private async Task WriteAsync<TMessage>(MessageExecutionContext<TMessage> context, CancellationToken cancellationToken)
        where TMessage : notnull
    {
        await _writer.WriteAsync(context, cancellationToken);
        Log.PublishedMessage(_log, context);
    }

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Trace, Message = "Контекст с сообщением '{Context}' добавлен в шину для обработки.")]
        public static partial void PublishedMessage(ILogger log, IMessageExecutionContext context);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Контекст с сообщением '{Context}' излечен из шины для обработки.")]
        public static partial void ExtractedMessage(ILogger log, IMessageExecutionContext context);

        [LoggerMessage(Level = LogLevel.Warning, Message = "Превышено время обработки сообщения '{TimeLimit}'!")]
        public static partial void TimeLimitMessage(ILogger log, Exception ex, TimeSpan timeLimit);

        [LoggerMessage(Level = LogLevel.Error, Message = "Сработала необработанное исключение в процессе обработки сообщения!")]
        public static partial void ErrorMessage(ILogger log, Exception ex);
    }
}