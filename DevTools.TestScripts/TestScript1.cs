using DevTools.Tooling.Interfaces;

namespace DevTools.TestScripts;

public class TestScript1 : IDevTool
{
    public string DisplayName { get; init; } = "Test Script 1";
    
    public void Execute()
    {
        throw new NotImplementedException();
    }
}