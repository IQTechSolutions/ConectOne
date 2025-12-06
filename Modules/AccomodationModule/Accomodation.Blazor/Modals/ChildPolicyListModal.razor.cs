using AccomodationModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for displaying a list of child policy rules.
    /// This component is used to show and manage child policy rules in a modal dialog.
    /// </summary>
    public partial class ChildPolicyListModal
    {
        #region Cascading Parameters

        /// <summary>
        /// Gets or sets the cascading parameter for the MudBlazor dialog instance.
        /// Used to control the modal dialog (e.g., close or cancel actions).
        /// </summary>
        [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the list of child policy rules to be displayed in the modal.
        /// </summary>
        [Parameter] public List<ChildPolicyRuleDto> ContentList { get; set; } = new List<ChildPolicyRuleDto>();

        /// <summary>
        /// Gets or sets the text for the accept button in the modal dialog.
        /// </summary>
        public string AcceptButtonText { get; set; } = "OK";

        #endregion

        #region Methods

        /// <summary>
        /// Cancels the modal dialog and closes it without performing any action.
        /// </summary>
        public void Cancel() => MudDialog.Cancel();

        #endregion
    }
}
