namespace DevTools.Tooling.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public class TaskAttribute : Attribute
{
    public string? Description { get; init; }

    public TaskAttribute(string? description = null)
    {
        Description = description;
    }
}