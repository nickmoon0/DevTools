using System.Windows.Input;

namespace DevTools.Dashboard.Models;

public class DevToolTask
{
    public required string TaskName { get; init; }
    public string? Description { get; init; }
    public required ICommand ExecuteTaskCommand { get; init; }
}