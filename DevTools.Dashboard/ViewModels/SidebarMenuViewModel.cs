using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using DevTools.Dashboard.Common;
using DevTools.Tooling.Interfaces;
using Microsoft.Win32;

namespace DevTools.Dashboard.ViewModels;

public sealed class SidebarMenuViewModel : INotifyPropertyChanged
{
    public ObservableCollection<IDevTool> DevTools { get; } = [];
    public ICommand SelectAssemblyCommand { get; }

    public SidebarMenuViewModel()
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
            // Load the assembly dynamically.
            var assembly = Assembly.LoadFrom(path);

            // Find all non-abstract types that implement IDevTool.
            var toolTypes = assembly.GetTypes()
                                    .Where(t => typeof(IDevTool).IsAssignableFrom(t)
                                                && t is { IsInterface: false, IsAbstract: false })
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

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
