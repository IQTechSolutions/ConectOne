using Blazored.LocalStorage;
using ConectOne.Domain.Constants;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace SchoolsEnterprise.Blazor.Shared.Components
{
    /// <summary>
    /// A reusable card that displays information about the currently logged-in user, 
    /// including first name, last name, email, and a profile image. 
    /// This component fetches data from the user's authentication state 
    /// and optionally loads a custom image from local storage.
    /// </summary>
    public partial class UserCard
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;

        private string _firstLetterOfName => _firstName?.Substring(0, 1).ToUpper();

        /// <summary>
        /// Provides the current authentication state (i.e., the logged-in user).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// An injected service that can be used to retrieve user account data, if needed.
        /// </summary>
        [Inject] public IUserService AccountProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used for accessing local storage in the browser.
        /// </summary>
        [Inject] public ILocalStorageService LocalStorage { get; set; } = null!;

        /// <summary>
        /// A CSS class name passed in from the parent to style this component.
        /// </summary>
        [Parameter] public string Class { get; set; } = null!;

        /// <summary>
        /// The URL or base64-encoded data representing the user's profile image. 
        /// Defaults to a placeholder image if not provided.
        /// </summary>
        [Parameter] public string ImageDataUrl { get; set; } = "_content/FilingModule.Blazor/images/profileImage128x128.png";        

        /// <summary>
        /// Called after each render. On the first render, this method triggers LoadDataAsync 
        /// to fetch and set user details (name, email, image).
        /// </summary>
        /// <param name="firstRender">True if this is the component's first render.</param>
        /// <returns>Asynchronous task.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
                StateHasChanged();
            }
        }

        /// <summary>
        /// Loads the user's basic info from the AuthenticationState 
        /// and attempts to read a profile image URL from local storage.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        private async Task LoadDataAsync()
        {
            // Retrieve the authentication state, which contains the user principal.
            var authState = await AuthenticationStateTask;

            // Extract user email, first name, and last name from claims.
            _email = authState.User.GetUserEmail();
            _firstName = authState.User.GetUserFirstName();
            _lastName = authState.User.GetUserLastName();

            // Optionally load a custom profile image from local storage, if available.
            var imageResponse = await LocalStorage.GetItemAsync<string>(StorageConstants.Local.UserImageURL);
            if (!string.IsNullOrEmpty(imageResponse))
            {
                ImageDataUrl = imageResponse;
            }
        }
    }
}
