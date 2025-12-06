using ConectOne.Domain.Mailing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Authentication
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
     //   [Inject] public IAccountsProvider AccountsProvider { get; set; } = null!;

        /// <summary>
        /// Service for sending emails.
        /// </summary>
        [Inject] public DefaultEmailSender EmailSender { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Properties

        /// <summary>
        /// The email address entered by the user for password reset.
        /// </summary>
        public string _email { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Initiates the password reset process.
        /// </summary>
        public async Task ResetNowAsync()
        {
            // Request to initiate the forgot password process.
            //var forgotPasswordResult = await AccountsProvider.ForgotPassword(new ForgotPasswordRequest() { EmailAddress = _email });

            //// Process the response and display appropriate messages.
            //forgotPasswordResult.ProcessResponseForDisplay(SnackBar, async () =>
            //{
            //    // Request to get the password reset token.
            //    var passwordResetCode = await AccountsProvider.GetResetPasswordTokenAsync(new ForgotPasswordRequest() { EmailAddress = _email });
            //    if (passwordResetCode.Succeeded)
            //    {
            //        var code = passwordResetCode.Data.Code;

            //        // Construct the reset URL with the token as a query parameter.
            //        var callbackUrl = NavigationManager.GetUriWithQueryParameters($"{Configuration["ApplicationConfiguration:ForgotPasswordResetLink"]}/{_email}", new Dictionary<string, object?> { ["code"] = code });

            //        // Send the password reset email using injected EmailSender.
            //        var emailResult = await EmailSender.SendForgotPasswordEmailAsync(
            //            _email,
            //            HtmlEncoder.Default.Encode(callbackUrl),
            //            Configuration["EmailConfiguration:logoUrl"],
            //            Configuration["EmailConfiguration:caption"],
            //            Configuration["EmailConfiguration:logoLink"]
            //        );

            //        // Process the email sending response and navigate to confirmation page if successful.
            //        emailResult.ProcessResponseForDisplay(SnackBar, () =>
            //        {
            //            NavigationManager.NavigateTo($"/forgot-password-conformation");
            //        });
            //    }
            //    else
            //    {
            //        // Display errors if the token generation failed.
            //        SnackBar.AddErrors(passwordResetCode.Messages);
            //    }
            //});
        }

        #endregion
    }
}