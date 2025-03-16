using DevTools.Tooling;

namespace DevTools.Dashboard.Models;

public class DevTool : IDevTool
{
    public string DisplayName { get; init; }

    public void Execute()
    {
        throw new NotImplementedException();
    }
}