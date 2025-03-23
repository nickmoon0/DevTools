using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DevTools.Dashboard.Common;
public class PluginLoadContext(string pluginPath) : AssemblyLoadContext
{
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // Check if the assembly is already loaded (e.g. DevTools.Common)
        var defaultAssembly = Default.Assemblies.FirstOrDefault(a => a.FullName == assemblyName.FullName);
        if (defaultAssembly != null)
        {
            return defaultAssembly;
        }

        // Look for the dependency in the same directory as the plugin
        var dependencyPath = Path.Combine(Path.GetDirectoryName(pluginPath) ?? string.Empty, assemblyName.Name + ".dll");
        return File.Exists(dependencyPath) ? LoadFromAssemblyPath(dependencyPath) :
            // Fallback to the default context (returns null)
            null;
    }
}