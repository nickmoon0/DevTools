namespace DevTools.Tooling;

public interface IDevTool
{
    public string DisplayName { get; init; }
    public void Execute();
}