using Accomodation.Blazor.Modals;
using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using KiburiOnline.Blazor.Shared.Pages.VacationReviews;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Pages.Vacations.Partials
{
    public partial class VacationReferencePartial
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
        /// Gets or sets the service used to manage and review vacation requests.
        /// </summary>
        [Inject] public IVacationReviewService VacationReviewService { get; set; } = null!;

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
        /// Gets or sets the callback invoked when the user navigates to the previous tab.
        /// </summary>
        /// <remarks>This callback is triggered when an action to move to the previous tab is performed. 
        /// Use this property to define the behavior or logic that should occur during such navigation.</remarks>
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
        /// Displays a dialog for creating a review for the current vacation and processes the result.
        /// </summary>
        /// <remarks>This method opens a modal dialog to allow the user to add a review for the current
        /// vacation.  If the review is successfully created, it is added to the vacation's references.  If the
        /// operation fails, error messages are displayed in a snackbar notification.</remarks>
        private async Task CreateReview()
        {
            var parameters = new DialogParameters<VacationReviewModal> { { x => x.VacationId, Vacation.VacationId } };

            var dialog = await DialogService.ShowAsync<VacationReviewModal>("Add a Review", parameters);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((VacationReviewViewModel)result.Data!).ToDto();
                if (PreviousTab.HasDelegate)
                {
                    Vacation.References.Add(createdItem);
                }
                else
                {
                    var creationResult = await VacationReviewService.CreateVacationReviewAsync(createdItem.Review);
                    if (creationResult.Succeeded)
                    {
                        Vacation.References.Add(createdItem);
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the details of an existing vacation review by displaying a modal dialog for user input.
        /// </summary>
        /// <remarks>This method opens a modal dialog to allow the user to update the details of a
        /// vacation review.  If the user confirms the changes, the updated review is either saved locally or sent to
        /// the server,  depending on the state of the application. If the operation fails, error messages are displayed
        /// using a notification service.</remarks>
        /// <param name="flight">The <see cref="VacationReviewDto"/> object representing the vacation review to be updated.  This parameter
        /// must not be null and should contain the current details of the review.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task UpdateReview(VacationReviewDto flight)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
            var parameters = new DialogParameters<VacationReviewModal> { { x => x.Review, new VacationReviewViewModel(flight) } };

            var dialog = await DialogService.ShowAsync<FlightModal>("Update Review", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var modalResult = ((VacationReviewViewModel)result.Data!).ToDto();

                if(PreviousTab.HasDelegate)
                {
                    var vacationInterval = Vacation.References.FirstOrDefault(c => c.Id == flight.Id)!;
                    var index = Vacation.References.IndexOf(vacationInterval);
                    Vacation.References[index] = modalResult;
                }
                else
                {
                    var creationResult = await VacationReviewService.UpdateVacationReviewAsync(modalResult.Review);
                    if (creationResult.Succeeded)
                    {
                        var vacationInterval = Vacation.References.FirstOrDefault(c => c.Id == flight.Id)!;
                        var index = Vacation.References.IndexOf(vacationInterval);
                        Vacation.References[index] = modalResult;
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Removes a review associated with the specified identifier after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// removal of the review.  If the user confirms, the review is removed either locally or via an API call,
        /// depending on the state of the application. If the API call fails, error messages are displayed to the
        /// user.</remarks>
        /// <param name="id">The unique identifier of the review to be removed. Cannot be null or empty.</param>
        private async Task RemoveReview(string id)
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
                if (PreviousTab.HasDelegate)
                {
                    Vacation.References.Remove(Vacation.References.FirstOrDefault(c => c.Id == id)!);
                }
                else
                {
                    var creationResult = await VacationReviewService.RemoveVacationReviewAsync(id);
                    if (creationResult.Succeeded)
                    {
                        Vacation.References.Remove(Vacation.References.FirstOrDefault(c => c.Id == id)!);
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
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
        /// Handles the event when the selected tab changes.
        /// </summary>
        /// <remarks>This method invokes the <see cref="SelectedTabChanged"/> event asynchronously,
        /// passing the index of the selected tab.</remarks>
        /// <param name="tabIndex">The zero-based index of the newly selected tab.</param>
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
        /// updates. It is executed asynchronously to ensure non-blocking behavior.</remarks>
        /// <returns></returns>
        private async Task OnUpdate()
        {
            await Update.InvokeAsync();
        }

        #endregion
    }
}
