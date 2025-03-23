namespace DevTools.Common.Attributes;

[Flags]
public enum ConfigParamOptions
{
    None = 0,
    Required = 1 << 0,
}

[AttributeUsage(AttributeTargets.Property)]
public class ConfigParamAttribute : Attribute
{
    public bool Required { get; }
    public string? Description { get; }
    public ConfigParamAttribute(ConfigParamOptions options = ConfigParamOptions.None, string? description = null)
    {
        Description = description;
        if (options.HasFlag(ConfigParamOptions.Required))
        {
            Required = true;
        }
    }
}