using DevTools.Tooling.Annotations;
using DevTools.Tooling.Common;
using Microsoft.Extensions.Logging;

namespace DevTools.TestScripts;

public class TestScript2(ILogger logger) : DevTool(logger)
{
    public override string DisplayName { get; init; } = "Test Script 2";

    [ConfigParam(ConfigParamOptions.Required)]
    public string? Name { get; set; }
    
    [ConfigParam]
    public int NumberOfThings { get; set; }
    
    [Task]
    public void Execute()
    {
        Logger.LogInformation("Name is: {Name}", Name);
        Logger.LogInformation("Number of things is: {NumberOfThings}", NumberOfThings);
    }
}