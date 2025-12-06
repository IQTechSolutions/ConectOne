using System.Globalization;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FeedbackModule.Domain.DataTransferObjects;
using KiburiOnline.Blazor.Shared.Pages.Vacations.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations
{
    /// <summary>
    /// Component for updating vacation details.
    /// </summary>
    public partial class Update
    {
        private bool _visible;
        private readonly MudTable<ReviewDto> _referenceTable = null!;
        private VacationViewModel _vacation = new();
        private int _selectedTab;


        private bool _loaded;

        #region Injected Services

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display brief, non-intrusive
        /// notifications to the user. Ensure that the service is properly injected before use.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The <see cref="Configuration"/> property is typically used to retrieve application
        /// settings and configuration values, such as connection strings, API keys, or other environment-specific
        /// settings. Ensure that the property is properly initialized before accessing it.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage itinerary item templates.
        /// </summary>
        [Inject] public IItineraryItemTemplateService ItineraryItemTemplateService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage vacation-related operations.
        /// </summary>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage custom variable tags within the application.
        /// </summary>
        [Inject] public ICustomVariableTagService CustomVariableTagService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Parameter to receive the vacation ID from the parent component or route.
        /// </summary>
        [Parameter] public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the index of the currently selected tab. If null, no tab is selected by default.
        /// </summary>
        /// <remarks>Set this property to specify which tab should be selected when the component is
        /// rendered. Changing the value updates the selected tab. The index is zero-based.</remarks>
        [Parameter] public int? SelectedTabIndex { get; set; } = null;

        #endregion

        #region Itinerary Entry Item Templates

        /// <summary>
        /// Displays a dialog to create a new vacation pricing item, processes the user's input,  and adds the created
        /// item to the vacation's pricing list if the operation succeeds.
        /// </summary>
        /// <remarks>This method opens a modal dialog for the user to input details for a new vacation
        /// pricing item.  If the user confirms the operation, the item is created and sent to the server for
        /// persistence.  Upon successful creation, the item is added to the local vacation pricing list and associated 
        /// dropdown items. If the operation fails, error messages are displayed to the user.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CreateItineraryEntryItemTemplate()
        {
            var parameters = new DialogParameters<ItineraryItemTemplateModal>
            {
                { x => x.Template, new ItineraryEntryItemTemplateViewModel() { Id = Guid.NewGuid().ToString(), DayNr = _vacation.ItineraryEntryItemTemplates.Count() + 1, VacationId = _vacation.VacationId }}
            };
            var dialog = await DialogService.ShowAsync<ItineraryItemTemplateModal>("Create Itinerary Item", parameters);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((ItineraryEntryItemTemplateViewModel)result.Data).ToDto();
                var creationResult = await ItineraryItemTemplateService.AddAsync(createdItem);
                if (creationResult.Succeeded)
                {
                    _vacation.ItineraryEntryItemTemplates.Add(createdItem);
                    _vacation.Nights = _vacation.ItineraryEntryItemTemplates.Count - 1;
                }
                else
                {
                    SnackBar.AddErrors(creationResult.Messages);
                }
            }
        }

        /// <summary>
        /// Updates a vacation price group item by displaying a confirmation dialog, creating a new item based on user
        /// input,  and adding it to the vacation pricing list if the operation succeeds.
        /// </summary>
        /// <remarks>This method displays a modal dialog to confirm the addition of a new vacation price
        /// group item. If the user confirms,  the method creates a new item, assigns it an order, and sends it to the
        /// server for persistence. Upon successful creation,  the item is added to the local vacation pricing list and
        /// associated dropdown items. If the operation fails, error messages  are displayed to the user.</remarks>
        /// <param name="dto">The data transfer object containing information about the vacation price group to be updated.</param>
        private async Task UpdateItineraryEntryItemTemplate(ItineraryEntryItemTemplateDto dto)
        {
            var parameters = new DialogParameters<ItineraryItemTemplateModal>
            {
                { x => x.Template, new ItineraryEntryItemTemplateViewModel(dto) }
            };
            var dialog = await DialogService.ShowAsync<ItineraryItemTemplateModal>("Update Itinerary Item", parameters);

            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((ItineraryEntryItemTemplateViewModel)result.Data).ToDto();

                var updateResult = await ItineraryItemTemplateService.EditAsync(createdItem);
                if (updateResult.Succeeded)
                {
                    var index = _vacation.ItineraryEntryItemTemplates.IndexOf(dto);
                    _vacation.ItineraryEntryItemTemplates[index] = createdItem;

                }
                else
                {
                    SnackBar.AddErrors(updateResult.Messages);
                }
            }
        }

        /// <summary>
        /// Removes a vacation price group item based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>This method displays a confirmation dialog before proceeding with the removal. If the
        /// operation is confirmed,  the item is removed from the vacation price group and the associated data
        /// structures are updated accordingly.</remarks>
        /// <param name="dto">The <see cref="AccomodationModule.Domain.DataTransferObjects.VacationPriceGroupDto"/> representing the vacation price group item to be removed.</param>
        private async Task RemoveItineraryEntryItemTemplate(ItineraryEntryItemTemplateDto dto)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this template from this package?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var creationResult = await ItineraryItemTemplateService.DeleteAsync(dto.Id);
                if (creationResult.Succeeded)
                {
                    _vacation.ItineraryEntryItemTemplates.Remove(dto);
                }
                else
                {
                    SnackBar.AddErrors(creationResult.Messages);
                }
            }
        }

        #endregion

        #region Update and Cancel

        /// <summary>
        /// Updates the vacation details.
        /// </summary>
        private async Task UpdateAsync(int? tabIndex = null)
        {
            _visible = true;
            try
            {
                var updateResult = await VacationService.UpdateAsync(_vacation.ToDto(), "");
                updateResult.ProcessResponseForDisplay(SnackBar, async () =>
                {
                    SnackBar.Add($"Action Successful. Package \"{_vacation.VacationTitle.VacationTitle}\" was successfully updated.", Severity.Success);

                    if(tabIndex.HasValue)
                        _selectedTab = tabIndex.Value;
                });

                _visible = false;
            }
            catch (Exception ex)
            {
                SnackBar.Add($"An error occurred: {ex.Message}", Severity.Error);
                _visible = false;
            }
        }

        /// <summary>
        /// Marks the current vacation as published and updates its state asynchronously.
        /// </summary>
        /// <remarks>This method sets the <see cref="_vacation"/> object's <c>Published</c> property to
        /// <see langword="true"/>  and then performs an asynchronous update operation to persist the changes.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task PublishVacation()
        {
            _vacation.Published = true;
            await UpdateAsync();
        }

        /// <summary>
        /// Marks the current vacation as unpublished and updates its state asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task UnPublishVacation()
        {
            _vacation.Published = false;
            await UpdateAsync();
        }

        /// <summary>
        /// Cancels the creation or update process and navigates back to the lodgings categories page.
        /// </summary>
        private void Cancel()
        {
            NavigationManager.NavigateTo($"/packages");
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the component and loads the vacation details.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await VacationService.VacationAsync(VacationId);
                _vacation = new VacationViewModel(result.Data);

                if(SelectedTabIndex is not null)
                    _selectedTab = SelectedTabIndex.Value;


                _loaded = true;
            }
            catch (Exception ex)
            {
                SnackBar.Add($"An error occurred: {ex.Message}", Severity.Error);
            }

            await base.OnInitializedAsync();
        }

        #endregion

        /// <summary>
        /// Replaces predefined variable tags in the provided text with corresponding values from the vacation data and
        /// configuration.
        /// </summary>
        /// <remarks>This method processes a set of predefined variable tags (e.g.,
        /// <c>&lt;---ReferenceNr---&gt;</c>, <c>&lt;---HostName---&gt;</c>)  and replaces them with values derived from
        /// the vacation data, such as reference numbers, host names, and dates.  Additionally, it retrieves custom
        /// variable tags from an external provider and replaces them in the text as well.</remarks>
        /// <param name="text">The input text containing variable tags to be replaced. Can be <see langword="null"/>.</param>
        /// <returns>A <see cref="string"/> with the variable tags replaced by their corresponding values.  If <paramref
        /// name="text"/> is <see langword="null"/>, the method returns <see langword="null"/>.</returns>
        private async Task<string> ProcessVariableTagsOfText(string text)
        {
            var result = text?.Replace("<strong style=\"color: blue\">&lt;---ReferenceNr---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.ReferenceNr}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---HostName---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Host?.Name}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---ArrivalCity---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Flights?.FirstOrDefault(c => c.DepartureDayNr == null)?.ArrivalAirport?.City?.Name}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---StartDate---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.StartDate?.ToShortDateString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---EndDate---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.EndDate.Value.ToShortDateString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---NightCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Nights.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---DayCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.DayCount.ToString(CultureInfo.InvariantCulture)}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---AccommodationCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.RoomCount.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---FlightCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Flights?.Count.ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---GolferPackagesCount---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.GolferPackages.Count().ToString()}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---Consultant---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Consultant?.Name + _vacation.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---Coordinator---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Coordinator?.Name + _vacation.Consultant?.Surname}</strong>")
                .Replace("<strong style=\"color: blue\">&lt;---TourDirector---&gt;</strong>", $"<strong style=\"color: black\">{_vacation.Coordinator?.Name + _vacation.Consultant?.Surname}</strong>")
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
    }
}
