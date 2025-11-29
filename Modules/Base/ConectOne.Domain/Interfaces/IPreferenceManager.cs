using ConectOne.Domain.ResultWrappers;

namespace ConectOne.Domain.Interfaces
{
    /// <summary>
    /// This interface defines the methods for managing user preferences in the application.
    /// </summary>
    public interface IPreferenceManager
    {
        /// <summary>
        /// Sets the user preference for the application.
        /// </summary>
        /// <param name="preference">The preference that should be set</param>
        Task SetPreference(IPreference preference);

        /// <summary>
        /// Gets the user preference for the application.
        /// </summary>
        Task<IPreference> GetPreference();

        /// <summary>
        /// Changes the application language based on the provided language code.
        /// </summary>
        /// <param name="languageCode">The language code that should be changed to</param>
        Task<IBaseResult> ChangeLanguageAsync(string languageCode);
    }
}
