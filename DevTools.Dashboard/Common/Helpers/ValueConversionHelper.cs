namespace DevTools.Dashboard.Common.Helpers;

public static class ValueConversionHelper
{
    /// <summary>
    /// Converts a string into the specified property type. Handles enums and nullable types specially.
    /// Uses Convert.ChangeType for standard .NET conversions (int, double, bool, DateTime, etc.).
    /// </summary>
    /// <param name="text">String input from the user/UI.</param>
    /// <param name="targetType">The type to which we want to convert.</param>
    /// <returns>The parsed object, or null if conversion results in null or empty input for nullable types.</returns>
    /// <exception cref="Exception">Rethrows any parsing/conversion errors for the caller to handle.</exception>
    public static object? ConvertToPropertyType(string? text, Type targetType)
    {
        // Check if the target type is nullable, extract underlying type if so
        var underlyingType = Nullable.GetUnderlyingType(targetType);

        // Handle nullable types explicitly
        if (underlyingType != null)
        {
            // Return null if input is null or whitespace
            if (string.IsNullOrWhiteSpace(text))
                return null;

            // Otherwise, convert to underlying non-nullable type
            targetType = underlyingType;
        }

        if (targetType.IsEnum)
        {
            // Enums require special handling
            return Enum.Parse(targetType, text ?? string.Empty, ignoreCase: true);
        }

        // Direct conversion for strings
        return targetType == typeof(string) ? text :
            // Standard conversions (int, double, bool, DateTime, etc.)
            Convert.ChangeType(text, targetType);
    }
}