using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Services
{
    public class CultureService
    {
        public const string CultureStorageKey = "appCulture";

        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;

        public CultureService(ILocalStorageService localStorage, NavigationManager navigationManager)
        {
            _localStorage = localStorage;
            _navigationManager = navigationManager;
        }

        public async Task<string> GetCurrentCultureAsync()
        {
            var storedCulture = await _localStorage.GetItemAsync<string>(CultureStorageKey);
            return string.IsNullOrWhiteSpace(storedCulture) ? CultureInfo.CurrentUICulture.Name : storedCulture;
        }

        public async Task SetCultureAsync(string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return;
            }

            var culture = new CultureInfo(cultureName);
            culture.NumberFormat.CurrencySymbol = "R";

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            await _localStorage.SetItemAsync(CultureStorageKey, cultureName);

            _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }
    }
}
