using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows;
using System.Windows.Input;
using DevTools.Dashboard.Common;
using DevTools.Dashboard.Models;
using DevTools.Tooling.Annotations;
using DevTools.Tooling.Interfaces;
using Microsoft.Win32;

namespace DevTools.Dashboard.ViewModels;

public sealed class DevToolViewModel : INotifyPropertyChanged
{
    private IDevTool? _selectedDevTool;
    public ObservableCollection<IDevTool> DevTools { get; } = [];
    public ObservableCollection<DevToolTask> DevToolTasks { get; private set; } = [];
    public ObservableCollection<ConfigParamViewModel> ConfigParams { get; } = [];
    
    public IDevTool? SelectedDevTool
    {
        get => _selectedDevTool;
        set
        {
            if (_selectedDevTool == value) return;
            
            _selectedDevTool = value;
            OnPropertyChanged(nameof(SelectedDevTool));
            
            LoadSelectedDevTool();
        }
    }
    
    public ICommand SelectAssemblyCommand { get; }

    public DevToolViewModel()
    {
        SelectAssemblyCommand = new RelayCommand(SelectAssembly);
    }

    private void SelectAssembly()
    {
        // Open the file dialog to let the user select an assembly.
        var openFileDialog = new OpenFileDialog
        {
            Filter = "DLL Files (*.dll)|*.dll",
            Title = "Select an Assembly"
        };

        if (openFileDialog.ShowDialog() != true) return;
        
        var selectedAssemblyPath = openFileDialog.FileName;
        LoadAssembly(selectedAssemblyPath);
    }

    private void LoadAssembly(string path)
    {
        try
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            var toolTypes = assembly.GetTypes()
                .Where(t => typeof(IDevTool).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
                .ToList();

            foreach (var toolType in toolTypes)
            {
                if (Activator.CreateInstance(toolType) is IDevTool toolInstance)
                {
                    DevTools.Add(toolInstance);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading assembly: {ex.Message}", "Assembly Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoadSelectedDevTool()
    {
        ConfigParams.Clear();
        DevToolTasks.Clear();
        
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

        var taskList = (
            from method in methods
            let attr = (TaskAttribute)method.GetCustomAttributes(typeof(TaskAttribute), true).First()
            let command = new RelayCommand(() =>
            {
                method.Invoke(_selectedDevTool, null);
            })
            select new DevToolTask
            {
                TaskName = method.Name,
                Description = attr.Description,
                ExecuteTaskCommand = command
            }
        ).ToList();
        
        foreach (var task in taskList)
        {
            DevToolTasks.Add(task);
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
