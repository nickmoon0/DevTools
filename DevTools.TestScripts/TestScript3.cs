using DevTools.Common;
using DevTools.Common.Attributes;
using Microsoft.Extensions.Logging;

namespace DevTools.TestScripts;

public class TestScript3(ILogger Logger) : DevTool(Logger)
{
    public override string DisplayName => "Test Script 3";
    public override string Description => "Contains async tasks";

    [Task("Waits 5 seconds before logging final message")]
    public async Task Task1Async()
    {
        Logger.LogInformation("Running Task1");
        await Task.Delay(5000);
        Logger.LogInformation("Finished Running Task1");
    }

    [Task("Logs 1 message, runs synchronously")]
    public void Task2()
    {
        Logger.LogInformation("Running Task2");
    }
}