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
    public partial class VacationMealsPartial
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
        /// Gets or sets the service responsible for managing meal additions.
        /// </summary>
        /// <remarks>This property is automatically injected and should be configured in the dependency
        /// injection container.</remarks>
        [Inject] public IMealAdditionService MealAdditionService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display brief, non-intrusive
        /// notifications to the user. Ensure that the service is properly injected before use.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

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
        /// Creates a new meal addition associated with the vacation.
        /// Opens a modal dialog to collect meal addition details, creates a new meal addition DTO,
        /// and sends it to the backend for persistence. If successful, the meal addition is added
        /// to the list of meal additions in the vacation ViewModel.
        /// </summary>
        private async Task CreateMealAddition()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };

            var dialog = await DialogService.ShowAsync<MealAdditionModal>("Create a new Meal Addition", options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var createdItem = ((MealAdditionViewModel)result.Data!).ToDto() with {VacationId = Vacation.VacationId};
                if (NextTab.HasDelegate)
                {
                    Vacation.Meals.Add(createdItem);
                }
                else
                {
                    var creationResult = await MealAdditionService.CreateMealAdditionAsync(createdItem);
                    if (creationResult.Succeeded)
                    {
                        Vacation.Meals.Add(createdItem);
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Updates an existing meal addition associated with the vacation.
        /// Opens a modal dialog pre-filled with the meal addition's current details,
        /// allows the user to modify them, and sends the updated meal addition DTO
        /// to the backend for persistence. If successful, the meal addition is updated
        /// in the vacation ViewModel.
        /// </summary>
        /// <param name="mealAddition">The meal addition DTO containing the current details.</param>
        private async Task UpdateMealAddition(MealAdditionDto mealAddition)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
            var parameters = new DialogParameters<MealAdditionModal> { { x => x.MealAddition, new MealAdditionViewModel(mealAddition) } };

            var dialog = await DialogService.ShowAsync<MealAdditionModal>("Confirm", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                var modalResult = ((MealAdditionViewModel)result.Data!).ToDto();
                if (NextTab.HasDelegate)
                {
                    var dinner = Vacation.Meals.FirstOrDefault(c => c.MealAdditionId == mealAddition.MealAdditionId)!;
                    if (dinner is not null)
                    {
                        var index = Vacation.Meals.IndexOf(dinner);
                        Vacation.Meals[index] = modalResult;
                    }
                }
                else
                {
                    var creationResult = await MealAdditionService.UpdateMealAdditionAsync(modalResult);
                    if (creationResult.Succeeded)
                    {
                        var dinner = Vacation.Meals.FirstOrDefault(c => c.MealAdditionId == mealAddition.MealAdditionId)!;
                        if (dinner is not null)
                        {
                            var index = Vacation.Meals.IndexOf(dinner);
                            Vacation.Meals[index] = modalResult;
                        }
                    }
                    else
                    {
                        SnackBar.AddErrors(creationResult.Messages);
                    }
                }
            }
        }

        /// <summary>
        /// Removes an existing meal addition associated with the vacation.
        /// Opens a confirmation dialog to ensure the user wants to delete the meal addition,
        /// and sends a request to the backend to delete the meal addition. If successful,
        /// the meal addition is removed from the vacation ViewModel.
        /// </summary>
        /// <param name="mealAdditionId">The ID of the meal addition to remove.</param>
        private async Task RemoveMealAddition(string mealAdditionId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this meal addition from this vacation?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                if (NextTab.HasDelegate)
                {
                    if (Vacation.Meals.Any(c => c.MealAdditionId == mealAdditionId))
                        Vacation.Meals.Remove(Vacation.Meals.FirstOrDefault(c => c.MealAdditionId == mealAdditionId));
                }
                else
                {
                    var removalResult = await MealAdditionService.RemoveMealAdditionAsync(mealAdditionId);
                    if (removalResult.Succeeded)
                    {
                        if (Vacation.Meals.Any(c => c.MealAdditionId == mealAdditionId))
                            Vacation.Meals.Remove(Vacation.Meals.FirstOrDefault(c => c.MealAdditionId == mealAdditionId));
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
        /// <remarks>This method invokes the <see cref="SelectedTabChanged"/> event asynchronously,
        /// passing the index of the selected tab.</remarks>
        /// <param name="tabIndex">The zero-based index of the newly selected tab.</param>
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
