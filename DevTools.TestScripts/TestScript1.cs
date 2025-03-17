using DevTools.Tooling.Annotations;
using DevTools.Tooling.Interfaces;

namespace DevTools.TestScripts;

public class TestScript1 : IDevTool
{
    [ConfigParam(ConfigParamOptions.Required)]
    public string? Name { get; set; }

    public string DisplayName { get; init; } = "Test Script 1";
    public void Execute()
    {
        Console.WriteLine($"Name is : {Name}");
    }
}