using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Application.ViewModels;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Blazor.Pages.Products.Modals
{
    /// <summary>
    /// Represents a modal dialog for handling a product attribute.
    /// </summary>
    public partial class ProductAttributeModal
    {
        #region Parameters

        /// <summary>
            /// The cascading MudBlazor dialog instance.
            /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
            /// The identifier of the associated product.
            /// </summary>
        [Parameter] public string? ProductId { get; set; } = null!;

        /// <summary>
            /// The view model for the product attribute.
            /// </summary>
        [Parameter] public ProductAttributeViewModel? Attribute { get; set; }

        /// <summary>
        /// Service for managing product-related operations.
        /// </summary>
        [Inject]public IProductService ProductService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications and messages to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
        /// valid  implementation of <see cref="ISnackbar"/> is provided before using this property.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Saves the attribute by closing the dialog with the attribute as the result.
        /// </summary>
        private void SaveAsync()
        {
            MudDialog.Close(Attribute);
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
            /// Initializes the component and ensures the Attribute is set.
            /// </summary>
            /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
            {
                var productResult = await ProductService.PricedProductAsync(ProductId);
                if (productResult.Succeeded)
                {
                    Attribute = new ProductAttributeViewModel()
                    {
                        ProductId = ProductId,
                        NamePrefix = productResult.Data.Details.Name + "-",
                        SkuPrefix = productResult.Data.Details.Sku,
                        Price = new PricingViewModel(productResult.Data.Pricing)
                    };
                }
                else
                {
                    SnackBar.AddErrors(productResult.Messages);
                }
                await base.OnInitializedAsync();
            }

        #endregion
    }
}
