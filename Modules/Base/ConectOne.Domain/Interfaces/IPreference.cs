namespace ConectOne.Domain.Interfaces
{
    /// <summary>
    /// This interface defines the structure for user preferences in the application.
    /// </summary>
    public interface IPreference
    {
        /// <summary>
        /// Gets or sets a value indicating whether the application is in dark mode.
        /// </summary>
        public bool IsDarkMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the text direction is right-to-left (RTL).
        /// </summary>
        public bool IsRTL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the drawer is currently open.
        /// </summary>
        public bool IsDrawerOpen { get; set; }

        /// <summary>
        /// Gets or sets the primary color used for the theme.
        /// </summary>
        public string PrimaryColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application is in dark mode.
        /// </summary>
        public string LanguageCode { get; set; }
    }
}
