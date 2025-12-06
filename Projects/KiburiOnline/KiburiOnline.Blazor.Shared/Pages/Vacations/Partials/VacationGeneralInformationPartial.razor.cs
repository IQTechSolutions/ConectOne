using System.Globalization;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Partials
{
    /// <summary>
    /// Represents a partial component for managing and displaying general information related to a vacation.
    /// </summary>
    /// <remarks>This class is responsible for handling the selection and processing of general information
    /// templates, managing callbacks for navigation and update events, and rendering processed content. It integrates
    /// with services to retrieve templates and process variable tags within the text. This partial class is typically
    /// used in a Blazor application to manage vacation-related data and user interactions.</remarks>
    public partial class VacationGeneralInformationPartial
    {
        private List<GeneralInformationTemplateDto> _availableGeneralInformationTemplates = [];
        private readonly Func<GeneralInformationTemplateDto?, string?> _generalInformationTemplateConverter = p => p?.Name;
        private MarkupString? _generalInformationText;

        #region Injected Services

        /// <summary>
        /// Gets or sets the service responsible for managing general information templates.
        /// </summary>
        [Inject] public IGeneralInformationTemplateService GeneralInformationTemplateService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing custom variable tags.
        /// </summary>
        /// <remarks>This property is automatically injected and should be configured in the dependency
        /// injection container.</remarks>
        [Inject] public ICustomVariableTagService CustomVariableTagService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The <see cref="Configuration"/> property is typically used to retrieve application
        /// settings and configuration values, such as connection strings, API keys, or other environment-specific
        /// settings. Ensure that the property is properly initialized before accessing it.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the vacation details for the current context.
        /// </summary>
        [Parameter, EditorRequired] public VacationViewModel Vacation { get; set; } = new VacationViewModel();

        /// <summary>
        /// Gets or sets the callback that is invoked to navigate to the next tab.
        /// </summary>
        /// <remarks>This property is typically used to handle user interactions for advancing to the next
        /// tab in a tabbed interface. Ensure that the callback is properly assigned to handle the navigation
        /// logic.</remarks>
        [Parameter] public EventCallback NextTab { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the "Previous Tab" action is triggered.
        /// </summary>
        /// <remarks>This callback is typically used to handle navigation to the previous tab in a tabbed
        /// interface. Assign a method or delegate to this property to define the behavior when the action
        /// occurs.</remarks>
        [Parameter] public EventCallback PreviousTab { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the cancel action is triggered.
        /// </summary>
        /// <remarks>Use this property to specify the action to perform when a cancel event occurs, such
        /// as closing a dialog or reverting changes.</remarks>
        [Parameter] public EventCallback Cancel { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked to handle update events.
        /// </summary>
        /// <remarks>This callback is typically used to notify the component of changes or trigger updates
        /// in response to user actions or other events.</remarks>
        [Parameter] public EventCallback Update { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the selection change of a general information template.
        /// </summary>
        /// <remarks>This method updates the vacation's general information and processes the template's
        /// text to generate HTML content.</remarks>
        /// <param name="generalInformation">The selected general information template. Must not be <see langword="null"/>.</param>
        private async Task OnGeneralInformationTemplateSelectionChanged(GeneralInformationTemplateDto? generalInformation)
        {
            if (generalInformation == null) return;

            Vacation.GeneralInformation = generalInformation;
            var generalInformationHtml = await ProcessVariableTagsOfText(generalInformation.Information);
            _generalInformationText = (MarkupString)generalInformationHtml;
        }

        /// <summary>
        /// Handles the event when the selected tab changes.
        /// </summary>
        private async Task OnNextTab()
        {
            await NextTab.InvokeAsync();
        }

        /// <summary>
        /// Invokes the action to navigate to the previous tab asynchronously.
        /// </summary>
        /// <remarks>This method triggers the <see cref="PreviousTab"/> delegate, allowing the caller to
        /// handle the logic for navigating to the previous tab. Ensure that the <see cref="PreviousTab"/> delegate is
        /// not null before invoking this method to avoid runtime exceptions.</remarks>
        /// <returns></returns>
        private async Task OnPreviousTab()
        {
            await PreviousTab.InvokeAsync();
        }

        /// <summary>
        /// Invokes the <see cref="Cancel"/> event asynchronously to signal a cancellation action.
        /// </summary>
        /// <remarks>This method triggers the cancellation logic by invoking the associated event
        /// callback. Ensure that the <see cref="Cancel"/> event is properly configured before calling this
        /// method.</remarks>
        /// <returns></returns>
        private async Task OnCancel()
        {
            await Cancel.InvokeAsync();
        }

        /// <summary>
        /// Invokes the <see cref="Update"/> event asynchronously.
        /// </summary>
        /// <remarks>This method triggers the <see cref="Update"/> event, allowing subscribers to handle
        /// the update operation. Ensure that any event handlers attached to <see cref="Update"/> are thread-safe and
        /// capable of handling asynchronous execution.</remarks>
        /// <returns></returns>
        private async Task OnUpdate()
        {
            await Update.InvokeAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private async Task<string> ProcessVariableTagsOfText(string? text)
        {
            try
            {
                if (string.IsNullOrEmpty(text)) return "text";

                var result = text.Replace("<strong style=\"color: blue\">&lt;---ReferenceNr---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.ReferenceNr}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---HostName---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Host?.Name}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---ArrivalCity---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Flights?.FirstOrDefault(c => c.DepartureDayNr == null)?.ArrivalAirport?.City?.Name}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---StartDate---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.StartDate?.ToShortDateString()}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---EndDate---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.EndDate?.ToShortDateString()}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---NightCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Nights.ToString()}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---DayCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.DayCount.ToString(CultureInfo.InvariantCulture)}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---AccommodationCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.RoomCount.ToString()}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---FlightCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Flights?.Count.ToString()}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---GolferPackagesCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.GolferPackages.Count().ToString()}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---Consultant---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Consultant?.Name + Vacation.Consultant?.Surname}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---Coordinator---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Coordinator?.Name + Vacation.Consultant?.Surname}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---TourDirector---&gt;</strong>", $"<strong style=\"color: black\">{Vacation.Coordinator?.Name + Vacation.Consultant?.Surname}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---CompanyEmail---&gt;</strong>", $"<strong style=\"color: black\">{Configuration["CompanyDetails:EmailAddress"]}</strong>")
                    .Replace("<strong style=\"color: blue\">&lt;---CompanyPhoneNr---&gt;</strong>", $"<strong style=\"color: black\">{Configuration["CompanyDetails:PhoneNr"]}</strong>");

                var availableCustomVariablePlaceholderResult = await CustomVariableTagService.GetAllAsync();
                if (availableCustomVariablePlaceholderResult.Succeeded)
                {
                    foreach (var customTag in availableCustomVariablePlaceholderResult.Data)
                    {
                        result = result.Replace($"<strong style=\"color: blue\">&lt;---{customTag.VariablePlaceholder}---&gt;</strong>", $"<strong style=\"color: black\">{customTag.VariablePlaceholder}</strong>");
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        #endregion

        /// <summary>
        /// Asynchronously initializes the component and loads the necessary data for rendering.
        /// </summary>
        /// <remarks>This method retrieves a list of general information templates and processes the
        /// general information  associated with the current vacation, if available. The processed information is stored
        /// as a  <see cref="MarkupString"/> for rendering. If no general information is available, a default message 
        /// is displayed. This method also invokes the base class implementation of <see
        /// cref="OnInitializedAsync"/>.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var generalInformationTemplateListResult = await GeneralInformationTemplateService.GetAllAsync();
            if (generalInformationTemplateListResult.Succeeded)
            {
                _availableGeneralInformationTemplates = generalInformationTemplateListResult.Data.ToList();
            }

            var generalInformationHtml = Vacation?.GeneralInformation == null ? "No General Information available" : await ProcessVariableTagsOfText(Vacation?.GeneralInformation?.Information);
            _generalInformationText = (MarkupString)generalInformationHtml;

            await base.OnInitializedAsync();
        }
    }
}
