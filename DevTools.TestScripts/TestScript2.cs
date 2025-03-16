using DevTools.Tooling.Interfaces;

namespace DevTools.TestScripts;

public class TestScript2 : IDevTool
{
    public string DisplayName { get; init; } = "Test Script 2";
    public void Execute()
    {
        throw new NotImplementedException();
    }
}