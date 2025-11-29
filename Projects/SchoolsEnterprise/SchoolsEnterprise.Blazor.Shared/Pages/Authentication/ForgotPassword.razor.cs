using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
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
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
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

            // Process the response and display appropriate messages.
            forgotPasswordResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                NavigationManager.NavigateTo($"/forgot-password-conformation");
            });
        }

        #endregion
    }
}
