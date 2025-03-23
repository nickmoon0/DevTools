using System.IO;
using System.Reflection;
using DevTools.Common;
using DevTools.Dashboard.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DevTools.Dashboard.Common.Assemblies;

public class AssemblyManager(ILoggerFactory loggerFactory, IConfiguration? environmentConfig = null)
{
    private readonly Dictionary<string, LoadedAssembly> _assemblies = [];

    public IReadOnlyDictionary<string, List<DevTool>> LoadedAssemblies =>
        _assemblies.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.DevTools);

    public IReadOnlyDictionary<string, List<DevTool>> UnloadableAssemblies => 
        _assemblies
            .Where(kvp => kvp.Value.LoadContext is not null)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.DevTools);
    
    public void LoadAssembly(string assemblyPath)
    {
        var assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);

        if (_assemblies.ContainsKey(assemblyName))
            throw new InvalidOperationException($"Assembly '{assemblyName}' is already loaded.");

        var loadContext = new PluginLoadContext(assemblyPath);
        var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);

        var devTools = assembly.GetTypes()
            .Where(t => typeof(DevTool).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
            .Select(t => (DevTool)Activator.CreateInstance(t, loggerFactory.CreateLogger(t))!)
            .ToList();

        // Set environment configuration for each DevTool, if provided
        if (environmentConfig != null)
        {
            foreach (var devTool in devTools)
            {
                devTool.Configuration = environmentConfig;
            }
        }

        _assemblies[assemblyName] = new LoadedAssembly
        {
            LoadContext = loadContext,
            DevTools = devTools
        };
    }

    public void LoadBuiltInTools()
    {
        var assembly = Assembly.GetAssembly(typeof(Tools.ToolsMarker))
                       ?? throw new InvalidOperationException("Built-in tools assembly not found.");

        var assemblyName = assembly.GetName().Name!;

        if (_assemblies.ContainsKey(assemblyName))
            throw new InvalidOperationException($"Assembly '{assemblyName}' is already loaded.");

        var devTools = assembly.GetTypes()
            .Where(t => typeof(DevTool).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
            .Select(t => (DevTool)Activator.CreateInstance(t, loggerFactory.CreateLogger(t))!)
            .ToList();

        if (environmentConfig != null)
        {
            foreach (var devTool in devTools)
            {
                devTool.Configuration = environmentConfig;
            }
        }

        _assemblies[assemblyName] = new LoadedAssembly
        {
            LoadContext = null,
            DevTools = devTools
        };
    }
    
    public void UnloadAssembly(string assemblyName)
    {
        if (!_assemblies.TryGetValue(assemblyName, out var loadedAssembly)) 
            return;

        if (loadedAssembly.LoadContext == null)
        {
            throw new InvalidOperationException($"Cannot unload built-in tools assembly \'{assemblyName}\'.");
        }
        
        // Clear references
        loadedAssembly.DevTools.Clear();

        // Unload the collectible context
        loadedAssembly.LoadContext?.Unload();

        _assemblies.Remove(assemblyName);

        // Force garbage collection to finalize unloading
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect(); // Recommend calling twice to ensure complete cleanup
    }

    public void UnloadAllAssemblies()
    {
        foreach (var assemblyName in _assemblies.Keys.ToList())
        {
            UnloadAssembly(assemblyName);
        }
    }
}