using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Modals
{
    /// <summary>
    /// Modal component for creating or editing a payment rule.
    /// </summary>
    public partial class PaymentRuleModal
    {
        #region Parameters

        /// <summary>
        /// Gets or sets the MudDialog instance for controlling the dialog.
        /// </summary>
        [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation form associated with the payment rule.
        /// </summary>
        [Parameter] public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the vacation extension associated with the payment rule.
        /// </summary>
        [Parameter] public string VacationExtensionId { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// Gets or sets the payment rule view model.
        /// </summary>
        [Parameter] public PaymentRuleViewModel PaymentRule { get; set; } = new();

        #endregion

        #region Methods

        /// <summary>
        /// Submits the payment rule and closes the dialog with an OK result.
        /// </summary>
        private void Submit()
        {
            MudDialog.Close(DialogResult.Ok(PaymentRule));
        }

        /// <summary>
        /// Cancels the dialog without saving changes.
        /// </summary>
        private void Cancel() => MudDialog.Cancel();

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and sets the booking form ID for the payment rule.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            PaymentRule.VacationId = VacationId;
            PaymentRule.VacationExtensionId = VacationId;
            await base.OnInitializedAsync();
        }

        #endregion
    }
}