using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using DevTools.Common;
using DevTools.Dashboard.Common;

namespace DevTools.Dashboard.ViewModels;

public sealed class ConfigParamViewModel : INotifyPropertyChanged
{
    private readonly PropertyInfo _propertyInfo;
    private readonly DevTool _devToolInstance;
    private string? _value;

    public string Name { get; }
    public string? Description { get; }

    public string? Value
    {
        get => _value;
        set
        {
            if (_value == value) return;
            _value = value;
            OnPropertyChanged();

            var propertyType = _propertyInfo.PropertyType;

            try
            {
                var convertedValue = ValueConversionHelper.ConvertToPropertyType(_value, propertyType);
                _propertyInfo.SetValue(_devToolInstance, convertedValue);
            }
            catch (Exception ex)
            {
                // Handle or log parse/conversion errors
                Console.WriteLine($"Error converting '{_value}' to {propertyType.Name}: {ex.Message}");
            }
        }
    }

    // Pass in the property info, the devTool object, plus anything else (e.g., description)
    public ConfigParamViewModel(PropertyInfo propertyInfo, DevTool devToolInstance, string? value = null, string? description = null)
    {
        _propertyInfo = propertyInfo;
        _devToolInstance = devToolInstance;

        Name = propertyInfo.Name;
        Description = description;
        
        // Initialize Value from the devTool’s current property value
        var initialValue = _propertyInfo.GetValue(devToolInstance);
        _value = initialValue?.ToString();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}