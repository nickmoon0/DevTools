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
        var timestamp = DateTime.UtcNow.ToString("o");
        
        if (string.IsNullOrEmpty(message))
            return;
        
        logAction($"[{timestamp}] [{logLevel}] {message}");
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }
}