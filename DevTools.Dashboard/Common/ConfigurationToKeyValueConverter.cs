using System.Globalization;
using System.Windows.Data;
using Microsoft.Extensions.Configuration;

namespace DevTools.Dashboard.Common;

public class ConfigurationToKeyValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IConfiguration configuration) return Enumerable.Empty<KeyValuePair<string, string>>();
        var keyValuePairs = new List<KeyValuePair<string, string>>();
        AddConfigurationSection(keyValuePairs, configuration, parentKey: null);
        return keyValuePairs;
    }

    private static void AddConfigurationSection(
        List<KeyValuePair<string, string>> keyValuePairs,
        IConfiguration configuration,
        string? parentKey)
    {
        foreach (var section in configuration.GetChildren())
        {
            // Build the key using the parent's key if it exists
            var currentKey = string.IsNullOrEmpty(parentKey) ? section.Key : $"{parentKey}__{section.Key}";
            
            // If the section has a non-null value, add it to the list.
            if (section.Value != null)
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(currentKey, section.Value));
            }
            
            // Recursively add children, if any.
            AddConfigurationSection(keyValuePairs, section, currentKey);
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}