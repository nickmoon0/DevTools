using Microsoft.Extensions.Logging;

namespace DevTools.Tooling.Common;

public class DevToolLogger(Action<string> logAction) : ILogger
{
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
        
        if (string.IsNullOrEmpty(message))
            return;
        
        logAction($"[{timestamp}] [{AbbreviateLogLevel(logLevel)}] {message}");
    }
    
    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }
    
    private static string AbbreviateLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "TRCE",
            LogLevel.Debug => "DBG",
            LogLevel.Information => "INFO",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "CRIT",
            _ => logLevel.ToString().ToUpperInvariant()
        };
    }
}