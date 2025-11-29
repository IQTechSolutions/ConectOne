namespace ConectOne.Domain.Extensions;

/// <summary>
/// Provides helper methods for interpreting icon values that can reference either
/// static image paths or CSS based icon classes.
/// </summary>
public static class IconValueHelper
{
    /// <summary>
    /// The prefix that indicates an icon value represents a CSS class rather than an image path.
    /// </summary>
    public const string FontIconPrefix = "icon:";

    /// <summary>
    /// Determines whether the supplied value represents a CSS based icon.
    /// </summary>
    /// <param name="value">The stored icon value.</param>
    /// <returns><c>true</c> when the value represents a CSS icon; otherwise <c>false</c>.</returns>
    public static bool IsFontIcon(string? value)
        => !string.IsNullOrWhiteSpace(value) && value.StartsWith(FontIconPrefix, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Extracts the CSS class from the stored icon value.
    /// </summary>
    /// <param name="value">The stored icon value.</param>
    /// <returns>The CSS class portion of the value, or an empty string when the value does not represent a CSS icon.</returns>
    public static string GetFontIconClass(string? value)
    {
        if (!IsFontIcon(value))
        {
            return string.Empty;
        }

        return value![FontIconPrefix.Length..].Trim();
    }

    /// <summary>
    /// Indicates whether the specified value should be uploaded as a physical asset when the category is saved.
    /// </summary>
    /// <param name="value">The stored icon value.</param>
    /// <returns><c>true</c> when the value refers to a static image path; otherwise <c>false</c>.</returns>
    public static bool RequiresUpload(string? value)
        => !string.IsNullOrWhiteSpace(value) && !IsFontIcon(value);
}
