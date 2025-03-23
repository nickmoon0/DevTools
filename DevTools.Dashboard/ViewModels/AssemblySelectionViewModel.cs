using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DevTools.Dashboard.ViewModels;

public class AssemblySelectionViewModel
{
    public ObservableCollection<AssemblySelectionItem> Assemblies { get; }

    public AssemblySelectionViewModel(IEnumerable<string> assemblyNames)
    {
        Assemblies = new ObservableCollection<AssemblySelectionItem>(
            assemblyNames.Select(name => new AssemblySelectionItem(name)));
    }
    
    public IEnumerable<string> SelectedAssemblies =>
        Assemblies.Where(a => a.IsSelected).Select(a => a.Name);
}

public class AssemblySelectionItem : INotifyPropertyChanged
{
    public string Name { get; }
    
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
        }
    }

    public AssemblySelectionItem(string name)
    {
        Name = name;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}