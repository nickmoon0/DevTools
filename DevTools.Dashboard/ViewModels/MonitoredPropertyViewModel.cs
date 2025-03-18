using System.ComponentModel;
using System.Reflection;

namespace DevTools.Dashboard.ViewModels;

public class MonitoredPropertyViewModel : INotifyPropertyChanged
{
    public string PropertyName { get; }
    private object? _value;

    public MonitoredPropertyViewModel(PropertyInfo propertyInfo, object source)
    {
        var propertyInfo1 = propertyInfo;
        var source1 = source;
        PropertyName = propertyInfo.Name;
        _value = propertyInfo.GetValue(source);

        // Subscribe to changes on the underlying source if possible.
        if (source1 is INotifyPropertyChanged npc)
        {
            npc.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == PropertyName)
                {
                    Value = propertyInfo1.GetValue(source1);
                }
            };
        }
    }

    public object? Value
    {
        get => _value;
        set
        {
            if (Equals(_value, value)) return;
            _value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
