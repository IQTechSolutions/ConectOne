using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// A modal dialog component for adding or editing a vacation inclusion.
    /// </summary>
    /// <remarks>This component utilizes MudBlazor's dialog system to present a form for creating or updating
    /// vacation inclusions. It interacts with the <see cref="IVacationService"/> to manage vacation-related
    /// operations.</remarks>
	public partial class AddInclusionModal
    {
        /// <summary>
        /// Gets the current instance of the dialog, allowing interaction with the dialog's lifecycle.
        /// </summary>
        /// <remarks>This property is typically set by the MudBlazor framework through cascading
        /// parameters and should not be set manually.</remarks>
		[CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the vacation.
        /// </summary>
		[Parameter] public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the vacation extension.
        /// </summary>
        [Parameter] public string? VacationExtensionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the vacation inclusion.
        /// </summary>
        [Parameter] public string? VacationInclusionId { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation  of <see cref="IVacationService"/> is provided before using this property.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation inclusion details.
        /// </summary>
        [Parameter] public VacationInclusionViewModel VacationInclusion { get; set; } = new() { VacationInclusionId = Guid.NewGuid().ToString() };

        /// <summary>
        /// Saves the current vacation inclusion data and closes the dialog.
        /// </summary>
        /// <remarks>This method finalizes the current operation by closing the dialog and passing the 
        /// <see cref="VacationInclusion"/> object as the result. Ensure that <see cref="VacationInclusion"/>  is
        /// properly set before calling this method.</remarks>
        private void SaveAsync()
		{
            MudDialog.Close(VacationInclusion);
        }

        /// <summary>
        /// Cancels the current dialog and closes it.
        /// </summary>
        /// <remarks>This method signals the dialog to close without returning a result.  It is typically
        /// used to dismiss the dialog when no further action is required.</remarks>
		public void Cancel()
		{
			MudDialog.Cancel();
		}

        /// <summary>
        /// Asynchronously initializes the component and sets up the initial state based on the provided vacation
        /// identifiers.
        /// </summary>
        /// <remarks>This method assigns the <see cref="VacationId"/> and <see
        /// cref="VacationExtensionId"/> values to the corresponding properties  of the <see cref="VacationInclusion"/>
        /// object. It also invokes the base implementation of <see cref="OnInitializedAsync"/>.</remarks>
        /// <returns></returns>
		protected override async Task OnInitializedAsync()
        {
            VacationInclusion.VacationId = VacationId!;
            VacationInclusion.VacationExtensionId = VacationExtensionId!;

            //if (!string.IsNullOrEmpty(VacationInclusionId))
            //{
            //    var result = await Provider.GetAsync<VacationInclusionDto>($"vacations/inclusions/inclusion/{VacationInclusionId}");
            //    result.ProcessResponseForDisplay(Snackbar, () =>
            //    {
            //        VacationInclusion = new VacationInclusionViewModel(result.Data);
            //    });
            //}

            await base.OnInitializedAsync();
		}
	}
}
