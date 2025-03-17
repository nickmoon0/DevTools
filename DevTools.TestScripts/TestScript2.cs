using DevTools.Tooling.Annotations;
using DevTools.Tooling.Interfaces;

namespace DevTools.TestScripts;

public class TestScript2 : IDevTool
{
    [ConfigParam(ConfigParamOptions.Required)]
    public string? Name { get; set; }
    [ConfigParam]
    public int NumberOfThings { get; set; }
    
    public string DisplayName { get; init; } = "Test Script 2";
    
    [Task]
    public void Execute()
    {
        Console.WriteLine($"Name is: {Name}");
        Console.WriteLine($"Number of things is: {NumberOfThings}");
    }
}