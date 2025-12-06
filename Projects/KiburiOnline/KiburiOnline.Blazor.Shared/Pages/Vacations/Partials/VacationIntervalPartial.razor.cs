using Accomodation.Blazor.Modals;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Partials
{
    public partial class VacationIntervalPartial
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
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the dependency injection
        /// container.</remarks>
        [Inject] public IVacationIntervalService VacationIntervalService { get; set; } = null!;

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
        /// Creates a new vacation interval and adds it to the list of intervals for the vacation.
        /// </summary>
        private async Task CreateVacationInterval()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
            var parameters = new DialogParameters<VacationIntervalModal>
            {
                { x => x.Vacation, Vacation.ToDto() }
            };

            var dialog = await DialogService.ShowAsync<VacationIntervalModal>("Create a new Interval", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((VacationIntervalViewModel)result.Data!).ToDto();
                if (NextTab.HasDelegate)
                {
                    Vacation.VacationIntervals.Add(createdItem);
                }
                else
                {
                    var creationResult = await VacationIntervalService.CreateVacationIntervalAsync(createdItem);
                    if (creationResult.Succeeded)
                    {
                        Vacation.VacationIntervals.Add(createdItem);
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Updates an existing vacation interval with new data.
        /// </summary>
        /// <param name="interval">The vacation interval DTO containing updated data.</param>
        private async Task UpdateVacationInterval(VacationIntervalDto interval)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
            var parameters = new DialogParameters<VacationIntervalModal> { { x => x.VacationInterval, new VacationIntervalViewModel(interval) {VacationId = Vacation.VacationId} } };

            var dialog = await DialogService.ShowAsync<VacationIntervalModal>("Confirm", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var modalResult = ((VacationIntervalViewModel)result.Data!).ToDto();

                if (NextTab.HasDelegate)
                {
                    var vacationInterval = Vacation.VacationIntervals.FirstOrDefault(c => c.VacationIntervalId == interval.VacationIntervalId)!;
                    var index = Vacation.VacationIntervals.IndexOf(vacationInterval);
                    Vacation.VacationIntervals[index] = modalResult;
                }
                else
                {
                    var updateResult = await VacationIntervalService.UpdateVacationIntervalAsync(modalResult);
                    if (updateResult.Succeeded)
                    {
                        var vacationInterval = Vacation.VacationIntervals.FirstOrDefault(c => c.VacationIntervalId == interval.VacationIntervalId)!;
                        var index = Vacation.VacationIntervals.IndexOf(vacationInterval);
                        Vacation.VacationIntervals[index] = modalResult;
                    }
                }
            }
        }

        /// <summary>
        /// Removes a vacation interval from the list of intervals for the vacation.
        /// </summary>
        /// <param name="intervalId">The ID of the vacation interval to remove.</param>
        private async Task RemoveVacationInterval(string intervalId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this interval from this vacation?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                if (NextTab.HasDelegate)
                {
                    Vacation.VacationIntervals.Remove(Vacation.VacationIntervals.FirstOrDefault(c => c.VacationIntervalId == intervalId)!);
                }
                else
                {
                    var removalResult = await VacationIntervalService.RemoveVacationIntervalAsync(intervalId);
                    if (removalResult.Succeeded)
                    {
                        Vacation.VacationIntervals.Remove(Vacation.VacationIntervals.FirstOrDefault(c => c.VacationIntervalId == intervalId)!);
                    }
                    else
                    {
                        SnackBar.AddErrors(removalResult.Messages);
                    }
                }
            }
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

        #endregion
    }
}
