using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for adding a meal addition.
    /// This component allows users to input details for a meal addition and save or cancel the operation.
    /// </summary>
    public partial class MealAdditionModal
    {
        private List<MealAdditionTemplateDto> _availableMealAdditionTemplates = [];
        private readonly Func<MealAdditionTemplateDto?, string> _mealAdditionTemplateConverter = p => $"{p?.Restaurant.Name} - {(string.IsNullOrEmpty(p?.Notes) ? p?.MealType.GetDescription() : p.Notes.TruncateLongString(55))}";

        #region Parameters

        /// <summary>
        /// Gets or sets the MudBlazor dialog instance for managing the modal's state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation associated with the interval.
        /// </summary>
        [Parameter] public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation interval view model containing the interval details.
        /// </summary>
        [Parameter] public MealAdditionViewModel MealAddition { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for managing meal addition templates.
        /// </summary>
        /// <remarks>This property is automatically injected and should be configured in the dependency
        /// injection container.</remarks>
        [Inject] public IMealAdditionTemplateService MealAdditionTemplateService { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Saves the vacation interval and closes the modal dialog.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(MealAddition);
        }

        /// <summary>
        /// Cancels the operation and closes the modal dialog.
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (MealAddition == null)
            {
                MealAddition = new MealAdditionViewModel() { VacationId = VacationId};
            }

            var mealAdditionTemplateListResult = await MealAdditionTemplateService.GetAllAsync();
            if (mealAdditionTemplateListResult.Succeeded)
            {
                _availableMealAdditionTemplates = mealAdditionTemplateListResult.Data.ToList();
            }

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
