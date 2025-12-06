using System.Globalization;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Partials
{
    /// <summary>
    /// Represents a partial class for managing vacation details, including available vacation title templates and
    /// hosts, as well as injected services and parameters.
    /// </summary>
    /// <remarks>This partial class is designed to be extended with additional functionality in other parts of
    /// the application. It includes injected services and parameters necessary for managing vacation-related
    /// data.</remarks>
    public partial class VacationDetailsPartial
    {
        private List<VacationTitleTemplateDto> _availableVacationTitleTemplates = [];
        private readonly Func<VacationTitleTemplateDto?, string?> _vacationTitleTemplateConverter = p => p?.VacationTitle;

        private List<VacationHostDto> _availableVacationHosts = [];
        private readonly Func<VacationHostDto?, string> _vacationHostConverter = p => p?.Name ?? "";

        private List<ShortDescriptionTemplateDto> _availableShortDescriptionTemplates = [];
        private readonly Func<ShortDescriptionTemplateDto?, string?> _shortDescriptionTemplateConverter = p => p?.Title;
        private MarkupString? _shortDescriptionText;

        #region Injected Services

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation  of <see cref="IVacationService"/> is provided before using this property.</remarks>
        [Inject] public IVacationHostService VacationHostService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing custom variable tags.
        /// </summary>
        [Inject] public ICustomVariableTagService CustomVariableTagService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation title templates.
        /// </summary>
        [Inject] public IVacationTitleTemplateService VacationTitleTemplateService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing short description templates.
        /// </summary>
        [Inject] public IShortDescriptionTemplateService ShortDescriptionTemplateService { get; set; } = null!;

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
        /// Updates the vacation title and generates a slug if it is not already set.
        /// </summary>
        /// <remarks>If the <see cref="Vacation.Slug"/> property is empty, a slug is automatically
        /// generated based on the provided vacation title. The generated slug replaces spaces with hyphens, converts
        /// the title to lowercase, and substitutes certain special characters (e.g., "/" with "-", "&" with
        /// "and").</remarks>
        /// <param name="dto">An object containing the new vacation title details.</param>
        public void VacationTitleChanged(VacationTitleTemplateDto dto)
        {
            Vacation.VacationTitle = dto;
            if(string.IsNullOrEmpty(Vacation.Slug))
                Vacation.Slug = dto.VacationTitle.ToLower().Replace(" ", "-").Replace("/", "-").Replace("&", "and");
        }

        /// <summary>
        /// Sets the start date of the vacation to <see langword="null"/>.
        /// </summary>
        /// <remarks>This method clears the start date of the vacation, indicating that no start date is
        /// currently set.</remarks>
        public void SetStartDateToNull()
        {
            Vacation.StartDate = null;
        }

        /// <summary>
        /// Sets the availability cut-off date to null, indicating that no cut-off date is specified.
        /// </summary>
        /// <remarks>This method clears the current availability cut-off date by setting it to null.  Use
        /// this method when you want to remove any restrictions related to the availability cut-off date.</remarks>
        public void SetAvailabilityCutOffDateToNull()
        {
            Vacation.AvailabilityCutOffDate = null;
        }

        /// <summary>
        /// Handles the event when a short description template is selected or changed.
        /// </summary>
        /// <remarks>Updates the vacation's short description and processes the template's content to
        /// generate the corresponding HTML representation.</remarks>
        /// <param name="shortDescription">The selected short description template. Cannot be <see langword="null"/>.</param>
        /// <returns></returns>
        private async Task OnShortDescriptionsTemplateSelectionChanged(ShortDescriptionTemplateDto shortDescription)
        {
            if (shortDescription == null) return;

            Vacation.ShortDescription = shortDescription;
            var shortDescriptionTextHtml = await ProcessVariableTagsOfText(shortDescription.Content);
            _shortDescriptionText = (MarkupString)shortDescriptionTextHtml;
        }

        /// <summary>
        /// Replaces predefined variable tags in the specified text with corresponding values from the vacation details
        /// and configuration settings.
        /// </summary>
        /// <remarks>This method processes both standard and custom variable tags. Standard tags are
        /// replaced with values from the vacation object and configuration settings. Custom tags are retrieved
        /// asynchronously from a provider and replaced accordingly.</remarks>
        /// <param name="text">The input text containing variable tags to be replaced.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the processed text with variable
        /// tags replaced by actual values.</returns>
        private async Task<string> ProcessVariableTagsOfText(string text)
        {
            var result = text.Replace("<strong style=\"color: blue\">&lt;---ReferenceNr---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.ReferenceNr}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---HostName---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.Host?.Name}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---ArrivalCity---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.Flights?.FirstOrDefault(c => c.DepartureDayNr == null)?.ArrivalAirport?.City?.Name}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---StartDate---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.StartDate?.ToShortDateString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---EndDate---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.EndDate?.ToShortDateString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---NightCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.Nights.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---DayCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.DayCount.ToString(CultureInfo.InvariantCulture)}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---AccommodationCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.RoomCount.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---FlightCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.Flights?.Count.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---GolferPackagesCount---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.GolferPackages?.Count()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---Consultant---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.Consultant?.Name + Vacation?.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---Coordinator---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.Coordinator?.Name + Vacation?.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---TourDirector---&gt;</strong>", $"<strong style=\"color: black\">{Vacation?.Coordinator?.Name + Vacation?.Consultant?.Surname}</strong>")
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

        /// <summary>
        /// Handles the event when the selected tab changes.
        /// </summary>
        /// <remarks>This method invokes the <see cref="SelectedTabChanged"/> event asynchronously,
        /// passing the index of the selected tab.</remarks>
        /// <param name="tabIndex">The zero-based index of the newly selected tab.</param>
        private async Task OnNextTab()
        {
            await NextTab.InvokeAsync();
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

        #endregion

        #region Overrides

        /// <summary>
        /// Asynchronously initializes the component by loading vacation host data and vacation title templates.
        /// </summary>
        /// <remarks>This method retrieves a paginated list of vacation hosts and a complete list of
        /// vacation title templates  from the underlying data provider. The results are stored in local fields for use
        /// within the component.</remarks>
        protected override async Task OnInitializedAsync()
        {
            var vacationHostResult = await VacationHostService.PagedVacationHostsAsync(new RequestParameters() { PageSize = 100 });
            if (vacationHostResult.Succeeded)
                _availableVacationHosts = vacationHostResult.Data;

            var vacationTitleTemplateListResult = await VacationTitleTemplateService.GetAllAsync();
            if (vacationTitleTemplateListResult.Succeeded)
            {
                _availableVacationTitleTemplates = vacationTitleTemplateListResult.Data.ToList();
            }

            var shortDescriptionTemplateListResult = await ShortDescriptionTemplateService.GetAllAsync();
            if (shortDescriptionTemplateListResult.Succeeded)
            {
                _availableShortDescriptionTemplates = shortDescriptionTemplateListResult.Data.ToList();
            }

            var shortDescriptionHtml = Vacation?.ShortDescription == null ? "No ShortDescription Available" : await ProcessVariableTagsOfText(Vacation.ShortDescription.Content);
            _shortDescriptionText = (MarkupString)shortDescriptionHtml;
        }

        #endregion
    }
}
