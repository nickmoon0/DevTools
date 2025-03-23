using System.Reflection;
using DevTools.Common;

namespace DevTools.Dashboard.Common;

public static class ToolLoader
{
    public static IEnumerable<Type>? BuiltInTools()
    {
        var toolTypes = Assembly.GetAssembly(typeof(Tools.ToolsMarker))
            ?.GetTypes()
            .Where(t => !t.IsAbstract && typeof(DevTool).IsAssignableFrom(t));

        return toolTypes;
    }
}