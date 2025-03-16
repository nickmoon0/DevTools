using System.Collections.ObjectModel;
using DevTools.Dashboard.Models;
using DevTools.Tooling;

namespace DevTools.Dashboard.ViewModels;

public class MainWindowViewModel
{
    public ObservableCollection<IDevTool> DevTools { get; init; }

    public MainWindowViewModel()
    {
        // Bootstrap with some test data.
        DevTools = [
            new DevTool { DisplayName = "Tool 1" },
            new DevTool { DisplayName = "Tool 2" },
            new DevTool { DisplayName = "Tool 3" }
        ];
    }
}