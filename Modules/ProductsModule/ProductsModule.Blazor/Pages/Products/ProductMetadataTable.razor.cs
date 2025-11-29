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
    /// Represents a Blazor page that displays and manages product metadata in a paginated table.
    /// </summary>
    public partial class ProductMetadataTable
    {
        private List<ProductMetadataDto> _metaDataItems = null!;
        private MudTable<ProductMetadataDto> _table = null!;

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
        /// <remarks>
        /// This property is typically injected by the dependency injection framework. Ensure that a valid  
        /// implementation of <see cref="ISnackbar"/> is configured in the service container before using this property.
        /// </remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in the application.
        /// </summary>
        /// <remarks>
        /// This property is typically injected by the framework and should not be manually set in most scenarios.
        /// </remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing product-related operations.
        /// </summary>
        [Inject] public IProductService ProductService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the ID of the product being edited.
        /// </summary>
        [Parameter] public string ProductId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the callback that is invoked when a new product is created.
        /// </summary>
        [Parameter] public EventCallback<ProductMetadataDto> OnCreate { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when a product metadata update occurs.
        /// </summary>
        /// <remarks>This callback can be used to handle updates to product metadata, such as saving
        /// changes  or performing additional processing. Ensure that the provided delegate is thread-safe  if it
        /// modifies shared resources.</remarks>
        [Parameter] public EventCallback<ProductMetadataDto> OnUpdate { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when a delete action is triggered.
        /// </summary>
        /// <remarks>Use this property to handle delete actions in a component. The callback is triggered
        /// with a string parameter that typically identifies the item to be deleted.</remarks>
        [Parameter] public EventCallback<string> OnDelete { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Prompts the user with a confirmation dialog before performing the metadata creation operation.
        /// </summary>
        /// <remarks>
        /// The current dialog text indicates a removal action, so verify that the intended functionality aligns with the method name.
        /// </remarks>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task CreateMetadata()
        {
            // Prepare dialog parameters with confirmation message.
            var parameters = new DialogParameters<ProductMetadataModal>
                {
                    { x => x.ProductId, ProductId }
                };

            // Show confirmation dialog to the user.
            var dialog = await DialogService.ShowAsync<ProductMetadataModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If the user confirms (i.e., did not cancel the dialog), perform the deletion.
            if (!result.Canceled)
            {
                var modalResult = ((ProductMetadataViewModel)result.Data!).ToDto();

                if (OnCreate.HasDelegate)
                {
                    await OnCreate.InvokeAsync(modalResult);
                }
                else
                {
                    var creationResult = await ProductService.UpdateMetadata(modalResult);
                    if (!creationResult.Succeeded) SnackBar.AddErrors(creationResult.Messages);
                }

                _metaDataItems.Add(modalResult);
            }
        }

        /// <summary>
        /// Prompts the user with a confirmation dialog before updating metadata for the specified product.
        /// If confirmed, deletes the metadata and reloads the table data.
        /// </summary>
        /// <param name="productId">The identifier of the product whose metadata is to be updated.</param>
        private async Task UpdateMetadata(ProductMetadataDto dto)
        {
            var parameters = new DialogParameters<ProductMetadataModal>
                {
                    { x => x.Metadata, new ProductMetadataViewModel(dto) }
                };

            var dialog = await DialogService.ShowAsync<ProductMetadataModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var index = _metaDataItems.IndexOf(dto);
                var modalResult = ((ProductMetadataViewModel)result.Data!).ToDto();

                if (OnUpdate.HasDelegate)
                {
                    await OnUpdate.InvokeAsync(modalResult);
                }
                else
                {
                    var updateResult = await ProductService.UpdateMetadata(modalResult);
                    if (!updateResult.Succeeded) SnackBar.AddErrors(updateResult.Messages);
                }

                _metaDataItems[index] = modalResult;
            }
        }

        /// <summary>
        /// Deletes metadata for a product with the specified identifier after user confirmation.
        /// </summary>
        /// <remarks>
        /// This method displays a confirmation dialog to the user before proceeding with the deletion.
        /// If the user confirms, the metadata is deleted and the associated table is reloaded.
        /// </remarks>
        /// <param name="productMetaDataId">The unique identifier of the product metadata to be deleted.</param>
        private async Task DeleteMetadata(string productMetaDataId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
                {
                    { x => x.ContentText, "Are you sure you want to remove this metadata from this product?" },
                    { x => x.ButtonText, "Yes" },
                    { x => x.Color, Color.Success }
                };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                if (OnDelete.HasDelegate)
                {
                    await OnDelete.InvokeAsync(productMetaDataId);
                }
                else
                {
                    await ProductService.RemoveMetadata(productMetaDataId);
                }

                _metaDataItems.Remove(_metaDataItems.FirstOrDefault(c => c.ProductId == productMetaDataId));
            }
        }

        #endregion

        /// <summary>
        /// Asynchronously initializes the component and retrieves product metadata for the specified product.
        /// </summary>
        /// <remarks>This method fetches metadata for the product identified by <see cref="ProductId"/> 
        /// and populates the metadata collection if the request is successful. It also calls the base implementation 
        /// of <see cref="OnInitializedAsync"/>.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var request = await ProductService.GetMetadata(ProductId);
            if (request.Succeeded)
            {
                _metaDataItems = request.Data.ToList();
            }
            await base.OnInitializedAsync();
        }
    }
}
