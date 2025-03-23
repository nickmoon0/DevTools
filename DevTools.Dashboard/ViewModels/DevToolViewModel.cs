using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using DevTools.Common;
using DevTools.Common.Attributes;
using DevTools.Dashboard.Common.Assemblies;
using DevTools.Dashboard.Common.Commands;
using DevTools.Dashboard.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace DevTools.Dashboard.ViewModels;

public sealed class DevToolViewModel : INotifyPropertyChanged
{
    private readonly AssemblyManager _assemblyManager;
    private DevTool? _selectedDevTool;
    private string? _loadedAssemblyName;
    
    public ObservableCollection<ConfigParamViewModel> ConfigParams { get; } = [];
    public ObservableCollection<DevTool> DevTools { get; } = [];
    public ObservableCollection<DevToolTask> DevToolTasks { get; } = [];
    public ObservableCollection<MonitoredPropertyViewModel> MonitoredProperties { get; } = [];
    
    // Dictionary mapping environment names (e.g., "Development", "Production") to their IConfiguration.
    private Dictionary<string, IConfiguration> EnvironmentConfigurations { get; } = [];
    public EnvironmentSelectionViewModel EnvironmentSelection { get; } = new();
    
    public ICommand ClearLogsCommand { get; }
    public ICommand SelectEnvironmentCommand { get; }
    public ICommand SelectAssemblyCommand { get; }

    public IConfiguration? SelectedEnvConfig
    {
        get
        {
            EnvironmentConfigurations.TryGetValue(EnvironmentSelection.SelectedEnvironment ?? "", out var envConfig);
            return envConfig;
        }
    }
    
    public string? LoadedAssemblyName
    {
        get => _loadedAssemblyName;
        set
        {
            if (_loadedAssemblyName == value) return;
            _loadedAssemblyName = value;
            OnPropertyChanged(nameof(LoadedAssemblyName));
        }
    }
    
    public DevTool? SelectedDevTool
    {
        get => _selectedDevTool;
        set
        {
            if (_selectedDevTool == value) return;
            _selectedDevTool = value;
            OnPropertyChanged(nameof(SelectedDevTool));
            OnPropertyChanged(nameof(SelectedDevToolAssemblyName));
            LoadSelectedDevTool();
        }
    }
    
    public string? SelectedDevToolAssemblyName =>
        _assemblyManager.LoadedAssemblies
            .FirstOrDefault(x => x.Value.Contains(SelectedDevTool!)).Key;
    
    public DevToolViewModel()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new DevToolLoggerProvider(AddLog));
        });

        // Load all environment configuration files on startup.
        EnvironmentSelection.PropertyChanged += OnEnvironmentSelectionChanged;
        LoadEnvironmentConfigurations();
        
        _assemblyManager = new AssemblyManager(loggerFactory, SelectedEnvConfig);
        
        ClearLogsCommand = new RelayCommand(ClearLogs);
        SelectAssemblyCommand = new RelayCommand(SelectAssembly);
        SelectEnvironmentCommand = new RelayCommand(SelectEnvironment);
    }
    
    private void OnEnvironmentSelectionChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(EnvironmentSelection.SelectedEnvironment)) return;
        
        UpdateDevToolsConfigurations();
        OnPropertyChanged(nameof(SelectedEnvConfig));
    }
    
    private void UpdateDevToolsConfigurations()
    {
        // Grab the newly selected environment from EnvironmentSelection
        var selectedEnv = EnvironmentSelection.SelectedEnvironment;
        if (selectedEnv is null) return;

        // If we have a matching config, update every DevTool
        if (!EnvironmentConfigurations.TryGetValue(selectedEnv, out var config)) return;
        
        foreach (var devTool in DevTools)
        {
            var toolType = devTool.GetType();
            var envConfigProperty = toolType.GetProperty(nameof(DevTool.Configuration), BindingFlags.Public | BindingFlags.Instance);
            if (envConfigProperty != null && envConfigProperty.PropertyType.IsInstanceOfType(config))
            {
                envConfigProperty.SetValue(devTool, config);
            }
        }
    }

    private void ClearLogs()
    {
        _selectedDevTool?.ToolLogs.Clear();
    }
    
    private void SelectEnvironment()
    {
        var envWindow = new Views.EnvironmentSelectionWindow
        {
            DataContext = EnvironmentSelection
        };

        EnvironmentSelection.Environments.Clear();
        foreach (var env in EnvironmentConfigurations.Keys)
        {
            EnvironmentSelection.Environments.Add(env);
        }

        if (envWindow.ShowDialog() == true)
        {
            OnPropertyChanged(nameof(EnvironmentSelection));
        }
    }
    
    private void SelectAssembly()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "DLL Files (*.dll)|*.dll",
            Title = "Select an Assembly"
        };

        if (openFileDialog.ShowDialog() != true) return;

        var assemblyPath = openFileDialog.FileName;
        var assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);

        try
        {
            _assemblyManager.LoadAssembly(assemblyPath);
            RefreshDevToolsCollection();
        
            LoadedAssemblyName = assemblyName;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load assembly: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RefreshDevToolsCollection()
    {
        DevTools.Clear();

        foreach (var (_, devTools) in _assemblyManager.LoadedAssemblies)
        {
            foreach (var tool in devTools)
                DevTools.Add(tool);
        }
    }
    
    private void LoadSelectedDevTool()
    {
        ConfigParams.Clear();
        DevToolTasks.Clear();
        MonitoredProperties.Clear();
        
        if (_selectedDevTool is null) return;

        // ---- Load Config Params ----
        var configProperties = _selectedDevTool
            .GetType()
            .GetProperties()
            .Where(p => p.GetCustomAttribute<ConfigParamAttribute>() != null);

        foreach (var prop in configProperties)
        {
            var configAttribute = prop.GetCustomAttribute<ConfigParamAttribute>();
            var value = prop.GetValue(_selectedDevTool)?.ToString();

            var configParamVm = new ConfigParamViewModel(
                propertyInfo: prop,
                devToolInstance: _selectedDevTool,
                value: value,
                description: configAttribute?.Description
            );
            
            ConfigParams.Add(configParamVm);
        }
        
        // ---- Load Tasks ----
        var methods = _selectedDevTool
            .GetType()
            .GetMethods()
            .Where(m => m.GetCustomAttribute<TaskAttribute>() != null);

        var taskList = methods.Select(method =>
        {
            var attr = (TaskAttribute)method.GetCustomAttributes(typeof(TaskAttribute), true).First();
            ICommand command;

            // Check if the method returns Task or Task<T>
            if (typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                // Use AsyncRelayCommand for async methods.
                command = new AsyncRelayCommand(async () =>
                {
                    var result = method.Invoke(_selectedDevTool, null);
                    if (result is Task task)
                    {
                        await task.ConfigureAwait(false);
                    }
                });
            }
            else
            {
                // Use a synchronous RelayCommand for sync methods.
                command = new RelayCommand(() =>
                {
                    method.Invoke(_selectedDevTool, null);
                });
            }

            return new DevToolTask
            {
                TaskName = method.Name,
                Description = attr.Description,
                ExecuteTaskCommand = command
            };
        }).ToList();
        
        foreach (var task in taskList)
        {
            DevToolTasks.Add(task);
        }
        
        // ---- Load Monitored Properties ----
        var monitoredProperties = _selectedDevTool
            .GetType()
            .GetProperties()
            .Where(p => p.GetCustomAttribute<MonitoredAttribute>() != null);

        foreach (var prop in monitoredProperties)
        {
            MonitoredProperties.Add(new MonitoredPropertyViewModel(prop, _selectedDevTool));
        }
    }

    private void LoadEnvironmentConfigurations()
    {
        // Get the base path where your configuration files are stored.
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        
        // Find all files that match the pattern "appsettings.*.json"
        var configFiles = Directory.GetFiles(basePath, "appsettings.*.json");

        foreach (var configFile in configFiles)
        {
            // Get the file name (e.g., "appsettings.Development.json")
            var fileName = Path.GetFileName(configFile);

            // Split the file name into parts based on '.'
            // Expected format: "appsettings.[environment].json"
            var parts = fileName.Split('.');
            if (parts.Length < 3) continue;
            
            // Extract the environment name, which is the second part.
            var environment = parts[1];

            // Build the configuration for this file.
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(fileName, optional: false, reloadOnChange: true)
                .Build();

            // Add or update the dictionary with the configuration for the environment.
            EnvironmentConfigurations[environment] = config;
        }
    }

    private void AddLog(string log)
    {
        _selectedDevTool?.ToolLogs.Add(log);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}