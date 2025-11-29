using ConectOne.Domain.Constants;
using ConectOne.Domain.Interfaces;

namespace ConectOne.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a set of user interface preferences for a client, including theme, layout direction, navigation
    /// drawer state, primary color, and language settings.
    /// </summary>
    public record ClientPreference : IPreference
    {
        /// <summary>
        /// Gets or sets a value indicating whether dark mode is enabled for the application.
        /// </summary>
        public bool IsDarkMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content is displayed in a right-to-left layout.
        /// </summary>
        public bool IsRTL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the drawer is currently open.
        /// </summary>
        public bool IsDrawerOpen { get; set; }

        /// <summary>
        /// Gets or sets the primary color used for the application's theme or branding.
        /// </summary>
        public string PrimaryColor { get; set; }

        /// <summary>
        /// Gets or sets the language code used for localization.
        /// </summary>
        /// <remarks>The language code should conform to the IETF BCP 47 standard (for example, "en-US" or
        /// "fr-FR"). This property determines which language resources are used for localized content.</remarks>
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-ZA";
    }
}
