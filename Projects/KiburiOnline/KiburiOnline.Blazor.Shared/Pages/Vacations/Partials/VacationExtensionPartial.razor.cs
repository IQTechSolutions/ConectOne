using Accomodation.Blazor.Modals;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Arguments.Requests;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Partials
{
    /// <summary>
    /// Represents a partial class for managing vacation extensions and related operations.
    /// </summary>
    /// <remarks>This class provides functionality for adding, removing, and managing vacation extensions
    /// within the context of a vacation. It includes injected services for HTTP operations, dialog management, and
    /// notifications, as well as parameters and callbacks for handling user interactions and navigation in a tabbed
    /// interface.</remarks>
    public partial class VacationExtensionPartial
    {
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
        /// Gets or sets the service used to manage vacation-related operations.
        /// </summary>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Adds a new vacation extension for this vacation.
        /// </summary>
        private async Task AddVacationExtension()
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            var parameters = new DialogParameters<AddVacationExtensionForVacationModal>
            {
                { x => x.VacationId, Vacation.VacationId }
            };

            var dialog = await DialogService.ShowAsync<AddVacationExtensionForVacationModal>("Confirm", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var model = (VacationDto)result.Data;
                if (NextTab.HasDelegate)
                {
                    Vacation.VacationExtensions.Add(model);
                }
                else
                {
                    var request = new CreateVacationExtensionForVacationRequest(model.VacationId, Vacation.VacationId);
                    var dd = await VacationService.CreateExtensionAsync(request);
                    if (dd.Succeeded)
                    {
                        Vacation.VacationExtensions.Add(model);
                    }
                    else
                    {
                        SnackBar.AddErrors(dd.Messages);
                    }
                        
                }
                StateHasChanged();
            }
        }

        /// <summary>
        /// Removes a vacation extension from this vacation.
        /// </summary>
        /// <param name="vacationExtensionId">The ID of the vacation extension to remove.</param>
        private async Task RemoveVacationExtension(string vacationExtensionId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this extension?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                if (NextTab.HasDelegate)
                {
                    Vacation.VacationExtensions.Remove(Vacation.VacationExtensions.FirstOrDefault(c => c.VacationId == vacationExtensionId)!);
                }
                else
                {
                    var removalResult = await VacationService.RemoveExtensionAsync(vacationExtensionId);
                    if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);

                    Vacation.VacationExtensions.Remove(Vacation.VacationExtensions.FirstOrDefault(c => c.VacationId == vacationExtensionId)!);
                }
                StateHasChanged();
            }
        }

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
    }
}
