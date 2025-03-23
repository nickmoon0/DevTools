using Microsoft.Extensions.Logging;

namespace DevTools.Common;

public class DevToolLoggerProvider(Action<string> logAction) : ILoggerProvider
{
    public void Dispose()
    {
        
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DevToolLogger(logAction);
    }
}