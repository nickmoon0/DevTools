namespace DevTools.Dashboard.Common.Helpers;

public static class ValueConversionHelper
{
    /// <summary>
    /// Converts a string into the specified property type. Handles enums specially.
    /// Uses Convert.ChangeType for standard .NET conversions (int, double, bool, DateTime, etc.).
    /// </summary>
    /// <param name="text">String input from the user/UI.</param>
    /// <param name="targetType">The type to which we want to convert.</param>
    /// <returns>The parsed object, or null if conversion fails.</returns>
    /// <exception cref="Exception">Rethrows any parsing/conversion errors for the caller to handle.</exception>
    public static object? ConvertToPropertyType(string? text, Type targetType)
    {
        if (targetType.IsEnum)
        {
            // Enums require special handling
            return Enum.Parse(targetType, text ?? string.Empty, ignoreCase: true);
        }

        return targetType == typeof(string) ?
            text :
            Convert.ChangeType(text, targetType);
    }
}
