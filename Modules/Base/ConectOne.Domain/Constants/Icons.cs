namespace ConectOne.Domain.Constants;

/// <summary>
/// Provides backwards compatible access to the shared <see cref="IconLibrary"/> for school module components.
/// </summary>
public static class CustomIcons
{
    /// <summary>
    /// Gets a read-only list of available icon options.
    /// </summary>
    public static IReadOnlyList<IconOption> IconOptions => IconLibrary.IconOptions;

    /// <summary>
    /// Gets an array of URLs for all available image icons in the icon library.
    /// </summary>
    public static string[] IconUrls => IconLibrary.ImageIconValues.ToArray();

    /// <summary>
    /// Gets the human-readable display name for the specified icon value.
    /// </summary>
    /// <param name="iconValue">The icon value for which to retrieve the display name. Can be null or an empty string to indicate no icon.</param>
    /// <returns>A string containing the display name of the icon if found; otherwise, an empty string.</returns>
    public static string GetDisplayName(string? iconValue) => IconLibrary.GetDisplayName(iconValue);

    /// <summary>
    /// Determines whether the specified icon value requires an upload to be used.
    /// </summary>
    /// <param name="iconValue">The icon value to evaluate. May be a predefined icon name or a custom value. Can be null.</param>
    /// <returns>true if the icon value requires an upload; otherwise, false.</returns>
    public static bool RequiresUpload(string? iconValue) => IconLibrary.RequiresUpload(iconValue);

    /// <summary>
    /// Determines whether the specified icon value represents a font-based icon.
    /// </summary>
    /// <param name="iconValue">The icon value to evaluate. Can be null or an empty string.</param>
    /// <returns>true if the icon value represents a font icon; otherwise, false.</returns>
    public static bool IsFontIcon(string? iconValue) => IconLibrary.IsFontIcon(iconValue);

    /// <summary>
    /// Returns the CSS class name associated with the specified icon value.
    /// </summary>
    /// <param name="iconValue">The identifier of the icon for which to retrieve the corresponding CSS class name. Cannot be null or empty.</param>
    /// <returns>A string containing the CSS class name for the specified icon. Returns an empty string if the icon value is not
    /// recognized.</returns>
    public static string GetCssClass(string iconValue) => IconLibrary.GetCssClass(iconValue);
}
