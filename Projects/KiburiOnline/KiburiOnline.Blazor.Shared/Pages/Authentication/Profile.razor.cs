using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using FilingModule.Blazor.Modals;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// Represents a user profile component that allows viewing and editing user information.
    /// </summary>
    /// <remarks>The <see cref="Profile"/> component is designed to manage user profile data, including
    /// updating user information, navigating back to a user list, and handling changes to the user's cover image. It
    /// interacts with external services for data retrieval and submission, and provides event callbacks for integration
    /// with parent components.</remarks>
    public partial class Profile
    {
        private string _imageSource = "_content/SchoolsModule.Blazor/images/profileImage128x128.png";

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in
        /// Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Parameter, EditorRequired] public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the callback that is invoked when the form is submitted.
        /// </summary>
        /// <remarks>The callback receives the submitted value as a <see cref="string"/> parameter. Use
        /// this property to handle form submission events and process the submitted data.</remarks>
        [Parameter] public EventCallback<string> OnSubmit { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when a cancel action occurs.
        /// </summary>
        /// <remarks>Use this property to handle cancel actions in your component. The callback can be
        /// used to perform cleanup, notify other components, or log the cancellation reason.</remarks>
        [Parameter] public EventCallback<string> OnCancel { get; set; }

        /// <summary>
        /// Gets or sets the callback to be invoked when a "Back" action occurs.
        /// </summary>
        /// <remarks>Use this property to handle "Back" navigation or actions in your component. The
        /// callback can be used to perform custom logic or update the state based on the provided context.</remarks>
        [Parameter] public EventCallback<string> OnBack { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the cover image changes.
        /// </summary>
        /// <remarks>The callback receives the new cover image URL as a string parameter. Use this
        /// property to handle cover image updates in your component.</remarks>
        [Parameter] public EventCallback<MudCropperResponse> OnCoverImageChanged { get; set; }
        
        /// <summary>
        /// Represents the user information for the current session.
        /// </summary>
        /// <remarks>This field holds an instance of <see cref="UserInfoViewModel"/> that contains details
        /// about the user. It is intended for internal use and should not be accessed directly outside of the class.</remarks>
        private UserInfoViewModel _user = new UserInfoViewModel();

        /// <summary>
        /// Submits the user information for an update operation.
        /// </summary>
        /// <remarks>This method sends the current user information to the server for updating. If the
        /// update is successful, a success message is displayed. Otherwise, error messages are shown.</remarks>
        /// <returns></returns>
        public async Task Submit()
        {
            var updateResult = await Provider.PostAsync("account/users/update", _user.ToDto());
            if (updateResult.Succeeded)
            {
                SnackBar.AddSuccess("User info updated successfully");
            }
            else
            {
                SnackBar.AddErrors(updateResult.Messages);
            }
        }

        /// <summary>
        /// Navigates the application to the user registrations page.
        /// </summary>
        /// <remarks>This method redirects the user to the "/registrations/users" route. It is typically
        /// used to navigate back to the user registrations view.</remarks>
        /// <returns></returns>
        public void Back()
        {
            NavigationManager.NavigateTo("/registrations/users");
        }

        /// <summary>
        /// Updates the user's cover image URL and triggers the associated event.
        /// </summary>
        /// <remarks>This method sets the user's cover image URL to the specified value and invokes the 
        /// <see cref="OnCoverImageChanged"/> event asynchronously. Ensure that the provided  <paramref
        /// name="coverImage"/> is a valid URL.</remarks>
        /// <param name="coverImage">The URL of the new cover image. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _user.CoverImageUrl = _imageSource;
            
            await OnCoverImageChanged.InvokeAsync(coverImage);
        }

        /// <summary>
        /// Asynchronously initializes the component and performs user-specific data fetching.
        /// </summary>
        /// <remarks>This method retrieves user information based on the provided <see cref="UserId"/> and
        /// maps the data into a view model for binding. If the operation fails, error messages are displayed using the
        /// <see cref="SnackBar"/> component.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            // Fetch the user information by ID
            var response = await Provider.GetAsync<UserInfoDto>($"account/users/{UserId}");
            if (response.Succeeded)
            {
                // Map the returned data into the UserInfoViewModel for binding
                _user = new UserInfoViewModel(response.Data!);
                _imageSource = _user.CoverImageUrl.Contains("_content") ? _user.CoverImageUrl?.TrimStart('/') : $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{_user.CoverImageUrl?.TrimStart('/')}" ;
            }
            else
            {
                // Show errors if the user fetch operation fails
                SnackBar.AddErrors(response.Messages);
            }
            await base.OnInitializedAsync();
        }
    }
}
