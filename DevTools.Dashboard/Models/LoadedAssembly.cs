using DevTools.Common;
using DevTools.Dashboard.Common.Assemblies;

namespace DevTools.Dashboard.Models;

public class LoadedAssembly
{
    public PluginLoadContext? LoadContext { get; set; }
    public List<DevTool> DevTools { get; set; } = [];
}