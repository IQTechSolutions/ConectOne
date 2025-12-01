using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Authentication
{
    /// <summary>
    /// Component for handling the forgot password functionality.
    /// </summary>
    public partial class ForgotPassword
    {
        #region Injected Services

        /// <summary>
        /// Provides account-related operations such as password reset.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// configuration values such as connection strings, application settings, and environment variables.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        #endregion

        #region Properties

        /// <summary>
        /// The email address entered by the user for password reset.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        /// Initiates the password reset process.
        /// </summary>
        public async Task ResetNowAsync()
        {
            var forgotPasswordResult = await Provider.PostAsync<string, ForgotPasswordRequest>($"account/forgot", new ForgotPasswordRequest() { EmailAddress = Email, ReturnUrl = Configuration["ApplicationConfiguration:ForgotPasswordResetLink"]! });

            forgotPasswordResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                NavigationManager.NavigateTo($"/forgot-password-conformation");
            });
        }

        #endregion
    }
}
