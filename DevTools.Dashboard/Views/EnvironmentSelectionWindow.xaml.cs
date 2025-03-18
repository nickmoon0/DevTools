using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DevTools.Dashboard.ViewModels;

namespace DevTools.Dashboard.Views;

public sealed partial class EnvironmentSelectionWindow : Window, INotifyPropertyChanged
{
    private string? _localSelectedEnvironment;
    public string? LocalSelectedEnvironment
    {
        get => _localSelectedEnvironment;
        set
        {
            if (_localSelectedEnvironment == value) return;
            _localSelectedEnvironment = value;
            OnPropertyChanged();
        }
    }

    public EnvironmentSelectionWindow()
    {
        InitializeComponent();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(LocalSelectedEnvironment))
        {
            MessageBox.Show(
                "Please select an environment.", 
                "Selection Required", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Only update application environment when okay pressed
        if (DataContext is EnvironmentSelectionViewModel vm)
        {
            vm.SelectedEnvironment = LocalSelectedEnvironment;
        }

        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

