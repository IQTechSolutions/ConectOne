using Accomodation.Blazor.Modals;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Blazor.Components.SortableItems;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Partials
{
    /// <summary>
    /// Represents a partial class for managing vacation inclusion settings and display sections.
    /// </summary>
    /// <remarks>This class provides functionality for managing vacation inclusion display sections, including
    /// creating, updating, removing, and sorting sections. It also handles navigation between tabs and integrates with
    /// various services such as HTTP providers, dialog services, and snack bar notifications. The class is designed to
    /// work within a Blazor component and relies on dependency injection for its services.</remarks>
    public partial class VacationInclusionSettingsPartial
    {
        private List<SortableItem<VacationInclusionDisplayTypeInformationDto>>? _vacationInclusionDisplaySectionsDropBoxItems = [];
        private VacationInclusionDisplayTypeInformationDto? _vacationInclusionDisplaySectionBeingDragged;

        #region Injected Services

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

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
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

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

        #region Vacation Inclusion Display Sections

        /// <summary>
        /// Creates a new vacation inclusion display section.
        /// Opens a modal dialog to collect details, creates a new DTO, and sends it to the backend for persistence.
        /// If successful, the section is added to the vacation ViewModel.
        /// </summary>
        private async Task CreateVacationInclusionDisplaySection()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };

            var dialog = await DialogService.ShowAsync<VacationInclusionDisplayModal>("Add a Vacation Inclusion Display Section", options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((VacationInclusionDisplayTypeInformationViewModel)result.Data!).ToDto();
                createdItem.VacationId = Vacation.VacationId;

                var text = Vacation.GeneralInclusionsSummaryInformation.HtmlToPlainText().Replace("\r\n", string.Empty);
                if (createdItem.VacationInclusionDisplayType == VacationInclusionDisplayTypes.General && string.IsNullOrEmpty(text))
                {
                    SnackBar.Add("Please fill in the general information for this vacation before adding the general information display section.", Severity.Warning);
                    return;
                }

                var creationResult = await VacationService.CreateVacationInclusionDisplaySectionAsync(createdItem);
                if (creationResult.Succeeded)
                {
                    Vacation.VacationInclusionDisplayTypeInfos.Add(createdItem);
                    _vacationInclusionDisplaySectionsDropBoxItems.FirstOrDefault(c => c.Selector == createdItem.ColumnSelection).Items.Add(createdItem);
                }
                else
                {
                    SnackBar.AddErrors(creationResult.Messages);
                }
            }
        }

        /// <summary>
        /// Updates an existing vacation inclusion display section.
        /// Opens a modal dialog pre-filled with the section's current details, allows the user to modify them,
        /// and sends the updated DTO to the backend for persistence. If successful, the section is updated in the ViewModel.
        /// </summary>
        /// <param name="dayTourActivity">The DTO containing the current details of the section to update.</param>
        private async Task UpdateVacationInclusionDisplaySection(VacationInclusionDisplayTypeInformationDto dayTourActivity)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
            var parameters = new DialogParameters<VacationInclusionDisplayModal> { { x => x.VacationInclusionDisplayType, new VacationInclusionDisplayTypeInformationViewModel(dayTourActivity) } };

            var dialog = await DialogService.ShowAsync<DayTourActivityModal>("Confirm", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var modalResult = ((VacationInclusionDisplayTypeInformationViewModel)result.Data!).ToDto();
                var creationResult = await VacationService.UpdateVacationInclusionDisplaySectionAsync(modalResult);
                if (creationResult.Succeeded)
                {
                    var vacationInterval = Vacation.VacationInclusionDisplayTypeInfos.FirstOrDefault(c => c.VacationInclusionDisplayTypeInformationId == dayTourActivity.VacationInclusionDisplayTypeInformationId)!;
                    var index = Vacation.VacationInclusionDisplayTypeInfos.IndexOf(vacationInterval);
                    Vacation.VacationInclusionDisplayTypeInfos[index] = modalResult;
                }
                else
                {
                    SnackBar.AddErrors(creationResult.Messages);
                }
            }
        }

        /// <summary>
        /// Removes an existing vacation inclusion display section.
        /// Opens a confirmation dialog to ensure the user wants to delete the section,
        /// and sends a request to the backend to delete it. If successful, the section is removed from the ViewModel.
        /// </summary>
        /// <param name="vacationInclusionDisplayTypeInformationId">The ID of the section to remove.</param>
        private async Task RemoveVacationInclusionDisplaySection(string vacationInclusionDisplayTypeInformationId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this day tour/activity from this vacation?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var creationResult = await VacationService.RemoveVacationInclusionDisplaySectionAsync(vacationInclusionDisplayTypeInformationId);
                if (creationResult.Succeeded)
                {
                    var item = Vacation.VacationInclusionDisplayTypeInfos.FirstOrDefault(c => c.VacationInclusionDisplayTypeInformationId == vacationInclusionDisplayTypeInformationId)!;

                    Vacation.VacationInclusionDisplayTypeInfos.Remove(item);
                    _vacationInclusionDisplaySectionsDropBoxItems.FirstOrDefault(c => c.Selector == item.ColumnSelection).Items.Remove(_vacationInclusionDisplaySectionsDropBoxItems
                        .FirstOrDefault(c => c.Selector == item.ColumnSelection).Items
                        .FirstOrDefault(c => c.VacationInclusionDisplayTypeInformationId == vacationInclusionDisplayTypeInformationId));

                }
                else
                {
                    SnackBar.AddErrors(creationResult.Messages);
                }
            }
        }

        /// <summary>
        /// Sorts the vacation inclusion display sections based on the new order.
        /// Updates the display order of the sections in the ViewModel.
        /// </summary>
        /// <param name="indices">Tuple containing the old index, new index, and the list of items.</param>
        private void SortVacationInclusionDisplaySectionList((int oldIndex, int newIndex, List<VacationInclusionDisplayTypeInformationDto> itemList) indices)
        {
            // deconstruct the tuple
            var (oldIndex, newIndex, itemList) = indices;

            var items = itemList;
            var itemToMove = items[oldIndex];
            items.RemoveAt(oldIndex);

            if (newIndex < items.Count)
            {
                items.Insert(newIndex, itemToMove);
            }
            else
            {
                items.Add(itemToMove);
            }

            for (var i = 0; i < items.Count; i++)
            {
                items[i].DisplayOrder = i;
            }

            StateHasChanged();
        }

        /// <summary>
        /// Handles the removal of a vacation inclusion display section from the sortable list.
        /// </summary>
        /// <param name="indices">Tuple containing the old index, new index, and the selector ID.</param>
        private void VacationInclusionDisplaySectionRemovedFromSortableList((int oldIndex, int newIndex, string id) indices)
        {
            var (oldIndex, newIndex, id) = indices;

            var items = this._vacationInclusionDisplaySectionsDropBoxItems.FirstOrDefault(c => c.Selector == id).Items;

            items.RemoveAt(oldIndex);
        }

        /// <summary>
        /// Updates the vacation inclusion display section in the sortable list after a drag-and-drop operation.
        /// Sends the updated order to the backend for persistence.
        /// </summary>
        /// <param name="indices">Tuple containing the old index, new index, and the selector ID.</param>
        private async Task VacationInclusionDisplaySectionUpdatedOnSortableList((int oldIndex, int newIndex, string id) indices)
        {
            try
            {
                this._vacationInclusionDisplaySectionsDropBoxItems.FirstOrDefault(c => c.Selector == indices.id).Items
                    .Insert(indices.newIndex, _vacationInclusionDisplaySectionBeingDragged);

                for (var i = 0;
                     i < this._vacationInclusionDisplaySectionsDropBoxItems
                         .FirstOrDefault(c => c.Selector == indices.id)
                         .Items.Count;
                     i++)
                {
                    this._vacationInclusionDisplaySectionsDropBoxItems.FirstOrDefault(c => c.Selector == indices.id)
                        .Items[i].DisplayOrder = i;
                    this._vacationInclusionDisplaySectionsDropBoxItems.FirstOrDefault(c => c.Selector == indices.id)
                        .Items[i].ColumnSelection = indices.id;
                }

                var itemsToUpdate = new List<VacationInclusionDisplayTypeInformationDto>();
                foreach (var column in _vacationInclusionDisplaySectionsDropBoxItems)
                {
                    itemsToUpdate.AddRange(column.Items.ToList());
                }

                var creationResult = await VacationService.UpdateVacationInclusionDisplaySectionDisplayOrderAsync(new VacationInclusionDisplayTypeInformationGroupUpdateRequest(Vacation.VacationId, itemsToUpdate));
                if (!creationResult.Succeeded) SnackBar.AddErrors(creationResult.Messages);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Handles the start of a drag operation for a vacation inclusion display section.
        /// </summary>
        /// <param name="indices">Tuple containing the old index and the item being dragged.</param>
        private void VacationInclusionDisplaySectionDragStart((int oldIndex, VacationInclusionDisplayTypeInformationDto item) indices)
        {
            _vacationInclusionDisplaySectionBeingDragged = indices.item;
        }

        #endregion

        #region Methods

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

        #endregion

        /// <summary>
        /// Initializes the component asynchronously and prepares the vacation inclusions and display sections.
        /// </summary>
        /// <remarks>This method sorts the vacation inclusions by their order and organizes the vacation
        /// inclusion display sections into sortable items based on their column selection and display order. It then
        /// calls the base implementation of <see cref="OnInitializedAsync"/>.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            // Sort vacation inclusions by their order
            Vacation.VacationInclusions = Vacation.VacationInclusions.OrderBy(c => c.Order).ToList();

            _vacationInclusionDisplaySectionsDropBoxItems.Add(new SortableItem<VacationInclusionDisplayTypeInformationDto>("One", Vacation.VacationInclusionDisplayTypeInfos.Where(c => c.ColumnSelection == "One").OrderBy(c => c.DisplayOrder).ToList()));
            _vacationInclusionDisplaySectionsDropBoxItems.Add(new SortableItem<VacationInclusionDisplayTypeInformationDto>("Two", Vacation.VacationInclusionDisplayTypeInfos.Where(c => c.ColumnSelection == "Two").OrderBy(c => c.DisplayOrder).ToList()));


            await base.OnInitializedAsync();
        }
    }
}
