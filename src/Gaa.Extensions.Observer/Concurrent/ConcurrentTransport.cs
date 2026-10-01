using System.Threading.Channels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Gaa.Extensions.Observer;

/// <summary>
/// Конкурентная транспортная шина в памяти.
/// </summary>
internal sealed partial class ConcurrentTransport : ITransport
{
    private readonly string _name;

    private readonly int _consumerCount;

    private readonly TimeSpan _executionTimeLimit;

    private readonly ILogger _log;

    private readonly Channel<IMessageExecutionContext> _channel;

    private readonly ChannelReader<IMessageExecutionContext> _reader;

    private readonly ChannelWriter<IMessageExecutionContext> _writer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ConcurrentTransport"/>.
    /// </summary>
    /// <param name="loggerFactory">Фабрика журналов протоколирования событий.</param>
    /// <param name="options">Настройки конкурентной транспортной шины в памяти.</param>
    public ConcurrentTransport(ILoggerFactory loggerFactory, ConcurrentTransportOptions options)
    {
        _name = options.Name;
        _consumerCount = options.ConsumerCount;
        _executionTimeLimit = options.ExecutionTimeLimit;
        var channelOptions = new BoundedChannelOptions(options.Capacity)
        {
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = options.ConsumerCount <= 1,
            SingleWriter = false,
        };

        _log = loggerFactory.CreateLogger(CategoryName.Concurrent);
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

    /// <inheritdoc />
    public Task RunAsync(IServiceScopeFactory scopeFactory, CancellationToken stoppingToken)
    {
        var tasks = Enumerable.Range(0, _consumerCount).Select(i => InternalRunAsync(scopeFactory, stoppingToken)).ToArray();
        return Task.WhenAll(tasks);
    }

    /// <inheritdoc />
    public void Close(CancellationToken cancellationToken)
    {
        if (_writer.TryComplete())
        {
            Log.CompleteMessage(_log);
        }
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Transport name: {_name}.";
    }

    private async Task WriteAsync<TMessage>(MessageExecutionContext<TMessage> context, CancellationToken cancellationToken)
        where TMessage : notnull
    {
        await _writer.WriteAsync(context, cancellationToken);
        Log.PublishedMessage(_log, context);
    }

    private async Task InternalRunAsync(IServiceScopeFactory scopeFactory, CancellationToken stoppingToken)
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

    private static partial class Log
    {
        [LoggerMessage(Level = LogLevel.Trace, Message = "Контекст с сообщением '{Context}' добавлен в шину для обработки.")]
        public static partial void PublishedMessage(ILogger log, IMessageExecutionContext context);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Контекст с сообщением '{Context}' излечен из шины для обработки.")]
        public static partial void ExtractedMessage(ILogger log, IMessageExecutionContext context);

        [LoggerMessage(Level = LogLevel.Trace, Message = "Шина закрыта для записи сообщений.")]
        public static partial void CompleteMessage(ILogger log);

        [LoggerMessage(Level = LogLevel.Warning, Message = "Превышено время обработки сообщения '{TimeLimit}'!")]
        public static partial void TimeLimitMessage(ILogger log, Exception ex, TimeSpan timeLimit);

        [LoggerMessage(Level = LogLevel.Error, Message = "Сработала необработанное исключение в процессе обработки сообщения!")]
        public static partial void ErrorMessage(ILogger log, Exception ex);
    }
}