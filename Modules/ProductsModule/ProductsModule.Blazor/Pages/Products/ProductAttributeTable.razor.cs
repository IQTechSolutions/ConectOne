using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Application.ViewModels;
using ProductsModule.Blazor.Pages.Products.Modals;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Blazor.Pages.Products
{
    /// <summary>
    /// This class represents a component that displays a table of product attributes.
    /// </summary>
    public partial class ProductAttributeTable
    {
        private List<ProductDto> _products = null!;

        private int _totalItems;
        private int _currentPage;

        private bool _dense;
        private bool _striped = true;
        private bool _bordered;

        private bool _loaded;        
        private bool _canEdit;

        #region Injections

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display snack bar notifications.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Ensure that a
        /// valid  implementation of <see cref="ISnackbar"/> is configured in the service container before using this
        /// property.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in the
        /// application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set in most
        /// scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing product-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that the dependency is properly configured in the service container before accessing this
        /// property.</remarks>
        [Inject] public IProductService ProductService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the ID of the product being edited.
        /// </summary>
        [Parameter] public string ProductId { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether headers should be displayed.
        /// </summary>
        [Parameter] public bool ShowHeaders { get; set; } = true!;

        /// <summary>
        /// Gets or sets the callback that is invoked when a new product is created.
        /// </summary>
        [Parameter] public EventCallback<ProductDto> OnCreate { get; set; }

        [Parameter] public EventCallback<ProductDto> OnUpdate { get; set; }

        [Parameter] public EventCallback<string> OnDelete { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a product with the specified identifier after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
        /// If the user confirms, the product is deleted from the data source, and the associated table is
        /// reloaded.</remarks>
        /// <param name="productId">The unique identifier of the product to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task CreateProductAttribute()
        {
            var parameters = new DialogParameters<ProductAttributeModal>
            {
                { x => x.ProductId, ProductId }
            };

            var dialog = await DialogService.ShowAsync<ProductAttributeModal>("Create New Attribute", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var modalResult = ((ProductAttributeViewModel)result.Data).ToDto(ProductId);

                if (OnCreate.HasDelegate)
                {
                    await OnCreate.InvokeAsync(modalResult);
                }
                else
                {
                    var createResult = await ProductService.CreateAsync(modalResult);
                    if (!createResult.Succeeded) SnackBar.AddErrors(createResult.Messages);
                }
                _products.Add(modalResult);
            }
        }
        
        /// <summary>
        /// Deletes a product with the specified identifier after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
        /// If the user confirms, the product is deleted from the data source, and the associated table is
        /// reloaded.</remarks>
        /// <param name="productId">The unique identifier of the product to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task UpdateProductAttribute(ProductDto product)
        {
            var parameters = new DialogParameters<ProductAttributeModal>
            {
                { x => x.Attribute, new ProductAttributeViewModel(product) },
                { x => x.ProductId, product.ProductId }
            };

            var dialog = await DialogService.ShowAsync<ProductAttributeModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var index = _products.IndexOf(product);
                var modalResult =((ProductAttributeViewModel)result.Data).ToDto(product.ProductId);

                if (OnUpdate.HasDelegate)
                {
                    await OnUpdate.InvokeAsync(modalResult);
                }
                else
                {
                    var createResult = await ProductService.UpdateAsync(modalResult);
                    if (!createResult.Succeeded) SnackBar.AddErrors(createResult.Messages);
                }

                _products[index] = modalResult;
            }
        }

        /// <summary>
        /// Deletes a product with the specified identifier after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the deletion. 
        /// If the user confirms, the product is deleted from the data source, and the associated table is
        /// reloaded.</remarks>
        /// <param name="productId">The unique identifier of the product to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task DeleteProductAttribute(string productId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this attribute from this product?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                if (OnDelete.HasDelegate)
                {
                    await OnDelete.InvokeAsync(productId);
                }
                else
                {
                    await ProductService.DeleteAsync(productId);
                }
                _products.Remove(_products.FirstOrDefault(c => c.ProductId == productId));
            }
        }

        /// <summary>
        /// Toggles the visibility of the product's attributes and updates the component's state.
        /// </summary>
        /// <param name="product">The product whose attribute visibility is to be toggled. Cannot be <see langword="null"/>.</param>
        public void ShowDetails(ProductDto product)
        {
            product.ShowAttributes = !product.ShowAttributes;
            StateHasChanged();
        }

        /// <summary>
        /// Asynchronously initializes the component and retrieves product attributes for the specified product.
        /// </summary>
        /// <remarks>This method fetches product attributes from the data provider using the current <see
        /// cref="ProductId"/>  and processes the response to populate the product list. If the request is successful,
        /// the product data  is displayed; otherwise, an appropriate message is shown using the <see
        /// cref="SnackBar"/>.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var request = await ProductService.GetAllAttributes(ProductId);
            request.ProcessResponseForDisplay(SnackBar, () =>
            {
                _products = request.Data.ToList();
            });
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
