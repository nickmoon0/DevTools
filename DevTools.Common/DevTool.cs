using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DevTools.Common;

public abstract class DevTool(ILogger logger) : INotifyPropertyChanged
{
    public abstract string DisplayName { get; }
    public virtual string? Description => null;
    
    public ObservableCollection<string> ToolLogs { get; private set; } = [];
    
    protected readonly ILogger Logger = logger;
    public IConfiguration? Configuration { get; set; }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}