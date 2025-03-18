using DevTools.Tooling.Annotations;
using DevTools.Tooling.Common;
using Microsoft.Extensions.Logging;

namespace DevTools.TestScripts;

public class TestScript1(ILogger logger) : DevTool(logger)
{
    public override string DisplayName { get; init; } = "Test Script 1";
    
    [ConfigParam(ConfigParamOptions.Required)]
    public string? Name { get; set; }
    
    [Task("This executes the script")]
    public void Execute()
    {
        Logger.LogInformation("Executing the script. Name is {Name}", Name);
    }

    [Task]
    public void PrintPlainName()
    {
        Logger.LogInformation("{Name}", Name);
        Logger.LogInformation("Test Config Value: {Config}", Configuration?["TestVal"]);
    }
}