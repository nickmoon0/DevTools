using DevTools.Tooling.Attributes;
using DevTools.Tooling.Common;
using Microsoft.Extensions.Logging;

namespace DevTools.TestScripts;

public class TestScript1(ILogger logger) : DevTool(logger)
{
    private int _count = 0;
    private string _testVal = "";
    
    public override string DisplayName { get; init; } = "Test Script 1";
    
    [ConfigParam(ConfigParamOptions.Required)]
    public string? Name { get; set; }

    [Monitored]
    public int Count
    {
        get => _count;
        set
        {
            if (_count == value) return;
            _count = value;
            OnPropertyChanged();
        }
    }

    [Monitored]
    public string TestVal
    {
        get => _testVal;
        set
        {
            if (_testVal == value) return;
            _testVal = value;
            OnPropertyChanged();
        }
    }

    [Task("This executes the script")]
    public void Execute()
    {
        Logger.LogInformation("Increasing count. Current value {Count}", Count);
        Count += 1;
    }

    [Task]
    public void PrintPlainName()
    {
        Logger.LogInformation("{Name}", Name);
        TestVal += Name;
    }
}