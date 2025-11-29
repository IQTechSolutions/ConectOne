namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents a language code and its corresponding display name.
    /// This can be used to support multilingual applications by providing
    /// a user-friendly name alongside an ISO language code.
    /// </summary>
    public class LanguageCode
    {
        /// <summary>
        /// The display name of the language, such as "English", "Français", or "Español".
        /// This is what might be shown to end-users when allowing them to select a language.
        /// </summary>
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// The code representing the language, often following ISO standards, e.g., "en" for English,
        /// "fr" for French, "es" for Spanish. This code is used internally to localize content.
        /// </summary>
        public string Code { get; set; } = null!;
    }
}