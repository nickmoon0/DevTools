using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DevTools.Dashboard.Common.Assemblies;
public class PluginLoadContext(string pluginPath, bool isCollectible = true)
    : AssemblyLoadContext(name: Path.GetFileNameWithoutExtension(pluginPath), isCollectible: isCollectible)
{
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // Check if the assembly is already loaded (e.g. DevTools.Common)
        var defaultAssembly = Default.Assemblies.FirstOrDefault(a => 
            string.Equals(a.FullName, assemblyName.FullName, StringComparison.OrdinalIgnoreCase));
        
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