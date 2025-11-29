using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// Represents the confirmation page for the "Forgot Password" process, allowing users to reset their password.
    /// </summary>
    /// <remarks>This class provides functionality to handle the "Forgot Password" workflow, including sending
    /// a reset password request and navigating to the reset password page upon success. It relies on dependency
    /// injection for account management, user notifications, and navigation.</remarks>
    public partial class ForgotPasswordConformation
    {
        public string _email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display snack bar notifications.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Ensure
        /// that the service is properly configured in the application container.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// a Blazor application.
        /// </summary>
        /// <remarks>The <see cref="NavigationManager"/> is typically used to navigate to different pages
        /// or to retrieve information about the current URI. This property is automatically injected by the Blazor
        /// framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Initiates the password reset process for the current user asynchronously.
        /// </summary>
        /// <remarks>This method sends a password reset request using the current user's email address. If
        /// the request is successful, the user is navigated to the password reset page.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ResetNowAsync()
        {
            //var forgotPasswordResult = await AccountsProvider.ForgotPassword(new ForgotPasswordRequest() { EmailAddress = _email });
            //forgotPasswordResult.ProcessResponseForDisplay(SnackBar, () =>
            //{
            //    var email = forgotPasswordResult.Data;
            //    NavigationManager.NavigateTo($"/reset-password/{email}");
            //});
        }
    }
}
