using ConectOne.Domain.Interfaces;

namespace ConectOne.Blazor.StateManagers
{
    /// <summary>
    /// This interface defines the methods for managing user preferences in the application.
    /// </summary>
    public interface IClientPreferenceManager : IPreferenceManager
    {

        /// <summary>
        /// Toggles the application theme between light and dark mode.
        /// </summary>
        Task<bool> ToggleDarkModeAsync();

        /// <summary>
        /// Changes the application language based on the provided language code.
        /// </summary>
        Task<bool> IsRTL();

        /// <summary>
        /// Toggles the layout direction between right-to-left (RTL) and left-to-right (LTR).
        /// </summary>
        Task<bool> ToggleLayoutDirection();
    }
}
