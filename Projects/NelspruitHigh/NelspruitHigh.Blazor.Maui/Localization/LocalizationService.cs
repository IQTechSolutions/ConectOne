using System.Globalization;

namespace NelspruitHigh.Blazor.Maui.Localization;

/// <summary>
/// Provides helpers for managing UI cultures across the application.
/// </summary>
public class LocalizationService
{
    /// <summary>
    /// Occurs when the active culture changes.
    /// </summary>
    public event Action? CultureChanged;

    /// <summary>
    /// Gets the currently active culture for the UI.
    /// </summary>
    public CultureInfo CurrentCulture => CultureInfo.DefaultThreadCurrentUICulture
        ?? CultureInfo.CurrentUICulture
        ?? CultureInfo.CurrentCulture;

    /// <summary>
    /// Sets the current culture for the application.
    /// </summary>
    /// <param name="cultureName">The culture name to set.</param>
    public void SetCulture(string cultureName)
    {
        if (string.IsNullOrWhiteSpace(cultureName))
        {
            return;
        }

        var culture = new CultureInfo(cultureName);
        if (string.Equals(CurrentCulture.Name, culture.Name, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        CultureChanged?.Invoke();
    }
}
