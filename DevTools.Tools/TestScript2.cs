using DevTools.Common;
using DevTools.Common.Attributes;
using Microsoft.Extensions.Logging;

namespace DevTools.Tools;

public class TestScript2(ILogger logger) : DevTool(logger)
{
    public override string DisplayName => "Test Script 2";
    
    [ConfigParam(ConfigParamOptions.Required)]
    public string? Name { get; set; }
    
    [ConfigParam(description:"Number of items")]
    public int NumberOfThings { get; set; }
    
    [Task]
    public void Execute()
    {
        Logger.LogInformation("Name is: {Name}", Name);
        Logger.LogInformation("Number of things is: {NumberOfThings}", NumberOfThings);
    }
}