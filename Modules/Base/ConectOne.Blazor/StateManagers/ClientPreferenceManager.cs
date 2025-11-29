using System.Globalization;
using Blazored.LocalStorage;
using ConectOne.Domain.Constants;
using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace ConectOne.Blazor.StateManagers
{
    /// <summary>
    /// This class manages user preferences for the application, including theme (light/dark), language, and layout direction (RTL/LTR).
    /// </summary>
    /// <param name="localStorageService">The injected local storage service</param>
    public class ClientPreferenceManager(ILocalStorageService localStorageService) : IClientPreferenceManager
    {
        /// <summary>
        /// The local storage service for storing and retrieving user preferences.
        /// </summary>
        public async Task<bool> ToggleDarkModeAsync()
        {
            if (await GetPreference() is not ClientPreference preference) return false;
            preference.IsDarkMode = !preference.IsDarkMode;
            await SetPreference(preference);
            return !preference.IsDarkMode;

        }

        /// <summary>
        /// Toggles the layout direction between right-to-left (RTL) and left-to-right (LTR).
        /// </summary>
        public async Task<bool> ToggleLayoutDirection()
        {
            if (await GetPreference() is not ClientPreference preference) return false;
            preference.IsRTL = !preference.IsRTL;
            await SetPreference(preference);
            return preference.IsRTL;
        }

        /// <summary>
        /// Changes the application language based on the provided language code.
        /// </summary>
        /// <param name="languageCode">The language code the app should be changed to</param>
        public async Task<IBaseResult> ChangeLanguageAsync(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                return new Result
                {
                    Succeeded = false,
                    Messages = ["Failed to set client preferences"]
                };
            }

            var culture = new CultureInfo(languageCode);
            culture.NumberFormat.CurrencySymbol = "R";

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            if (await GetPreference() is not ClientPreference preference)
                return new Result
                {
                    Succeeded = false,
                    Messages = ["Failed to get client preferences"]
                };
            preference.LanguageCode = languageCode;
            await SetPreference(preference);
            return new Result
            {
                Succeeded = true,
                Messages = ["Client Language has been changed"]
            };
        }

        /// <summary>
        /// Checks if the application is in right-to-left (RTL) mode based on user preference.
        /// </summary>
        public async Task<bool> IsRTL()
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference == null) return preference.IsRTL;
            return preference.IsDarkMode != true && preference.IsRTL;
        }

        /// <summary>
        /// Retrieves the user preference from local storage. If not found, returns a new ClientPreference object.
        /// </summary>
        public async Task<IPreference> GetPreference()
        {
            return await localStorageService.GetItemAsync<ClientPreference>(StorageConstants.Local.Preference) ?? new ClientPreference();
        }

        /// <summary>
        /// Sets the user preference in local storage.
        /// </summary>
        /// <param name="preference">The preference that should be set</param>
        public async Task SetPreference(IPreference preference)
        {
            await localStorageService.SetItemAsync(StorageConstants.Local.Preference, preference as ClientPreference);
        }
    }
}
