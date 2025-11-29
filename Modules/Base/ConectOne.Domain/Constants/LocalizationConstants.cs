using ConectOne.Domain.Entities;

namespace ConectOne.Domain.Constants
{
    /// <summary>
    /// Provides constants related to localization, including supported language codes and display names.
    /// </summary>
    /// <remarks>This class is intended for use in applications that require information about available
    /// languages for localization purposes. It cannot be instantiated.</remarks>
    public static class LocalizationConstants
    {
        /// <summary>
        /// Gets the list of language codes supported by the application.
        /// </summary>
        /// <remarks>Each entry in the array contains a language code and its display name. The list is
        /// read-only and may be used to validate or present supported language options to users.</remarks>
        public static readonly LanguageCode[] SupportedLanguages = [
            new() {
                Code = "en-ZA",
                DisplayName= "English"
            },
            new() {
                Code = "af-ZA",
                DisplayName= "Afrikaans"
            }
        ];
    }
}
