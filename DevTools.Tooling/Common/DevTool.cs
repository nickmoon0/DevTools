using Microsoft.Extensions.Logging;

namespace DevTools.Tooling.Common;

public abstract class DevTool(ILogger logger)
{
    public abstract string DisplayName { get; init; }
    protected readonly ILogger Logger = logger;
}