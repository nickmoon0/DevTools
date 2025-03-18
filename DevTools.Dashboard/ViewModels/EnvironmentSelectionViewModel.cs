using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DevTools.Dashboard.ViewModels;

public class EnvironmentSelectionViewModel : INotifyPropertyChanged
{
    private string? _selectedEnvironment;

    public ObservableCollection<string> Environments { get; } = [];

    public string? SelectedEnvironment
    {
        get => _selectedEnvironment;
        set
        {
            if (_selectedEnvironment == value) return;
            _selectedEnvironment = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
