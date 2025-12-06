using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
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
    public partial class ContactFormPartial
    {
        private VacationContactUsInfoViewModel _contactForm = new VacationContactUsInfoViewModel();

        private GoogleMap _map1 = null!;
        private MapOptions _mapOptions = null!;
        private readonly Stack<Marker> _markers = new Stack<Marker>();
        string token = "";
        GooglereCAPTCHAv3Response? googlereCAPTCHAv3Response;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime abstraction used to invoke JavaScript functions from .NET.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework in Blazor
        /// applications. Ensure that the property is properly initialized before attempting to use it for JavaScript
        /// interop.</remarks>
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
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation operations within the
        /// application.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to interact with Google reCAPTCHA v3 for verifying user interactions.
        /// </summary>
        [Inject] public GooglereCAPTCHAv3Service GooglereCAPTCHAv3Service { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling vacation-related contact information.
        /// </summary>
        [Inject] public IVacationContactUsInfoService VacationContactUsInfoService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="HttpClient"/> instance used to send HTTP requests and receive HTTP responses.
        /// </summary>
        [Inject] public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Gets or sets the subject of the message.
        /// </summary>
        [Parameter] public string? Subject { get; set; }

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

                    if (string.IsNullOrEmpty(_contactForm.Subject))
                        SnackBar.AddError("Subject is a requited field");

                    var vacationContactUsInfo = _contactForm.ToDto();

                    var result =
                        await VacationContactUsInfoService.CreateVacationContactUsInfoAsync(vacationContactUsInfo);
                    result.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        SnackBar.Add("Your message has been sent successfully!", Severity.Success);
                        NavigationManager.NavigateTo("/thank-you", true);
                    });
                }
            }
            catch (Exception ex)
            {
                SnackBar.Add($"There was an error sending your message: {ex.Message}", Severity.Error);
            }
        }

        /// <summary>
        /// Initializes the map after it has been created.
        /// </summary>
        private async Task OnAfterMapInit()
        {
            var latLng = new LatLngLiteral(-33.8744833, 18.6266834);
            var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
            {
                Position = latLng,
                Map = _map1.InteropObject,
                Title = "Kwagga Travel",
            });
            _markers.Push(marker);
            StateHasChanged();
        }

        /// <summary>
        /// Initializes the component asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                

                _mapOptions = new MapOptions
                {
                    Zoom = 13,
                    Center = new LatLngLiteral(-33.8744833, 18.6266834),
                    MapTypeId = MapTypeId.Roadmap
                };

                if (!string.IsNullOrEmpty(Subject))
                {
                    _contactForm.Subject = Subject;
                }

                await base.OnInitializedAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// Invoked after the component has been rendered. Executes additional logic if this is the first render.
        /// </summary>
        /// <remarks>When <paramref name="firstRender"/> is <see langword="true"/>, this method invokes a
        /// JavaScript function  to perform a CAPTCHA operation and updates the component's state with the
        /// result.</remarks>
        /// <param name="firstRender">A boolean value indicating whether this is the first time the component has been rendered. <see
        /// langword="true"/> if this is the first render; otherwise, <see langword="false"/>.</param>
        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                token = await JsRuntime.InvokeAsync<string>("runCaptcha");
                StateHasChanged();
            }
        }
    }
}
