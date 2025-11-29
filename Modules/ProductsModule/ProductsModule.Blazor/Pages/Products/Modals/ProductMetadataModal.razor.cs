using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Application.ViewModels;

namespace ProductsModule.Blazor.Pages.Products.Modals
{
    /// <summary>
    /// Represents a modal for editing product metadata.
    /// </summary>
    public partial class ProductMetadataModal
    {
        #region Parameters

        /// <summary>
            /// Gets the instance of the MudDialog.
            /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

        /// <summary>
            /// Gets or sets the product identifier.
            /// </summary>
        [Parameter] public string? ProductId { get; set; }

        /// <summary>
            /// Gets or sets the product metadata view model.
            /// </summary>
        [Parameter] public ProductMetadataViewModel? Metadata { get; set; }

        #endregion

        #region Properties

        /// <summary>
            /// Gets or sets a value indicating whether vacation arrival is enabled.
            /// </summary>
        public bool VacationArrival { get; set; }

        #endregion

        #region Methods

        /// <summary>
            /// Saves the metadata and closes the dialog.
            /// </summary>
        private void SaveAsync()
            {
                MudDialog.Close(Metadata);
            }

        /// <summary>
            /// Cancels the operation and closes the dialog.
            /// </summary>
        private void Cancel()
            {
                MudDialog.Cancel();
            }

        #endregion

        #region Lifecycle Methods

        /// <summary>
            /// Initializes the component.
            /// </summary>
            /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
            {
                Metadata ??= new ProductMetadataViewModel() { ProductId = ProductId };
                await base.OnInitializedAsync();
            }

        #endregion
    }
}
