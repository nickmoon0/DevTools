namespace DevTools.Tooling.Interfaces;

public interface IDevTool
{
    public string DisplayName { get; init; }
    public void Execute();
}