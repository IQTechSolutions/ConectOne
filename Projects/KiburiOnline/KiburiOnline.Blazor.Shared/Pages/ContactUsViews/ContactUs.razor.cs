using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;
using Recapcha.Entities;
using Recapcha.Services;

namespace KiburiOnline.Blazor.Shared.Pages.ContactUsViews
{
    /// <summary>
    /// Component for the "Contact Us" page.
    /// </summary>
    public partial class ContactUs
    {
        private VacationContactUsInfoViewModel _contactForm = new VacationContactUsInfoViewModel();
        string token = "";
        GooglereCAPTCHAv3Response? googlereCAPTCHAv3Response;

        /// <summary>
        /// Injects the NavigationManager to handle navigation.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Injects the Snackbar service to display notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework and should be used to
        /// perform JavaScript interop operations.</remarks>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated via dependency injection and provides access to
        /// application settings, such as connection strings, app-specific options, and other configuration
        /// values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to interact with Google reCAPTCHA v3 for verifying user interactions.
        /// </summary>
        [Inject] public GooglereCAPTCHAv3Service GooglereCAPTCHAv3Service { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="HttpClient"/> instance used to send HTTP requests and receive HTTP responses.
        /// </summary>
        [Inject] public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for handling vacation-related contact information.
        /// </summary>
        [Inject] public IVacationContactUsInfoService VacationContactUsInfoService { get; set; } = null!;

        /// <summary>
        /// Handles the form submission.
        /// </summary>
        private async Task HandleSubmit()
        {
            try
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"secret", Configuration["GooglereCAPTCHAv3Settings:SecretKey"]! },
                    {"response", token }
                });
                var response = await HttpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                if (response.IsSuccessStatusCode)
                {
                    _contactForm.Subject = "Pro Golf Contact Us Form Submission";
                    var vacationContactUsInfo = _contactForm.ToDto();

                    var result = await VacationContactUsInfoService.CreateVacationContactUsInfoAsync(vacationContactUsInfo);
                    result.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        SnackBar.Add("Your message has been sent successfully!", Severity.Success);
                        NavigationManager.NavigateTo("/thank-you");
                    });
                }



            }
            catch (Exception ex)
            {
                SnackBar.Add($"There was an error sending your message: {ex.Message}", Severity.Error);
            }
        }
    }
}
