using BusinessModule.Application.ViewModel;
using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Interfaces;
using FilingModule.Application.Services;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using FilingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Headers;
using BusinessModule.Blazor.Modals;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using ShoppingModule.Blazor.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.BusinessListings
{
    /// <summary>
    /// Represents a component for editing business listings, including managing images, videos, services, and products.
    /// </summary>
    /// <remarks>This component provides functionality for updating business listing details, uploading and
    /// managing images and videos,  and adding, editing, or removing associated services and products. It interacts
    /// with an HTTP provider to perform  server-side operations and displays feedback to the user via a
    /// snackbar.</remarks>
    public partial class ListingEditor
    {
        private string _userId = string.Empty;

        private BusinessListingViewModel _listing = new();
        private string _imageSource = "_content/BusinessModule.Blazor/background.png";
        private string? _coverImageToUpload;
        private ICollection<string> _galleryImageToUpload = new List<string>();

        private List<ListingTierDto> _availableListingTiers = [];
        private readonly Func<ListingTierDto?, string> _listingTierConverter = p => p?.Name;

        private ICollection<IBrowserFile> _videosToUpload = new List<IBrowserFile>();
        private ICollection<UploadResult> _filesBusyUploading = new List<UploadResult>();
        private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;
        private bool uploadInProgress;
        private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
        private string _dragClass = DefaultDragClass;

        private ListingTierDto _selectedListingTier;

        private List<CategoryDto> _availableCategories = [];
        private readonly Func<CategoryDto?, string> _categoryConverter = p => p?.Name ?? "";

        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new BreadcrumbItem("Listings", href: "/businesslistings", icon: Icons.Material.Filled.List),
            new BreadcrumbItem("Update Listing", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        };

        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="CartStateProvider"/> instance used to manage the state of the shopping cart.
        /// </summary>
        [CascadingParameter] public CartStateProvider ShoppingCartService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query the business directory.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation of <see cref="IBusinessDirectoryQueryService"/> is provided before using this
        /// property.</remarks>
        [Inject] public IBusinessDirectoryQueryService BusinessDirectoryQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for executing commands related to the business directory.
        /// </summary>
        [Inject] public IBusinessDirectoryCommandService BusinessDirectoryCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for performing image processing operations.
        /// </summary>
        [Inject] public IImageProcessingService ImageProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for processing video-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid implementation  of <see cref="IVideoProcessingService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVideoProcessingService VideoProcessingService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query listing tier information.
        /// </summary>
        [Inject] public IListingTierQueryService ListingTierQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI in the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current URI and related navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing business directory categories.
        /// </summary>
        [Inject] public IBusinessDirectoryCategoryService BusinessDirectoryCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use the snack bar
        /// service to show brief messages or alerts in the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the listing.
        /// </summary>
        [Parameter] public string ListingId { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the profile entry feature is enabled.
        /// </summary>
        [Parameter] public bool ProfileEntry { get; set; }

        /// <summary>
        /// Handles changes to the cover image by updating the image source and preparing the image for upload.
        /// </summary>
        /// <param name="coverImage">The updated cover image, represented as a <see cref="MudCropperResponse"/> containing the image data in
        /// Base64 format.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            _coverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Saves the current listing asynchronously by either creating a new listing or updating an existing one.
        /// </summary>
        /// <remarks>If <see cref="ListingId"/> is null or empty, a new listing is created. Otherwise, the
        /// existing listing  identified by <see cref="ListingId"/> is updated.</remarks>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        private async Task SaveAsync()
        {
            if (string.IsNullOrEmpty(ListingId))
            {
                await CreateAsync();
            }
            else
            {
                await UpdateAsync();
            }
        }

        /// <summary>
        /// Updates the current business listing, including its details, images, products, and services.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet"> <item>Updates the
        /// business listing details on the server.</item> <item>Uploads a new cover image for the listing, if provided,
        /// and replaces the existing one.</item> <item>Uploads gallery images for the listing, if any are
        /// provided.</item> <item>Processes and updates cover images for each product and service associated with the
        /// listing, if applicable.</item> <item>Displays success or error messages in the UI based on the operation
        /// results.</item> </list> The method navigates to the business listings page upon successful
        /// completion.</remarks>
        /// <returns></returns>
        private async Task CreateAsync()
        {
            var result = await BusinessDirectoryCommandService.CreateAsync(_listing.ToDto() with { UserId = _userId});
            result.ProcessResponseForDisplay(SnackBar, async () =>
            {
                if (!string.IsNullOrEmpty(_coverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = $"{_listing.Heading} Logo",
                        ImageType = UploadType.Cover,
                        Base64String = _coverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        if (_listing.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await BusinessDirectoryCommandService.RemoveImage(_listing.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BusinessDirectoryCommandService.AddImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                foreach (var galleryImage in _galleryImageToUpload)
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = Guid.NewGuid().ToString(),
                        ImageType = UploadType.Image,
                        Base64String = galleryImage
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        var additionResult = await BusinessDirectoryCommandService.AddImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                foreach (var product in _listing.Products)
                {
                    if (string.IsNullOrEmpty(product.CoverImageToUpload))
                    {
                        var request = new Base64ImageUploadRequest()
                        {
                            Name = $"{product.Name} Cover",
                            ImageType = UploadType.Cover,
                            Base64String = _coverImageToUpload
                        };
                        var imageUploadResult = await ImageProcessingService.UploadImage(request);
                        if (imageUploadResult.Succeeded)
                        {
                            if (product.Images.Any(c => c.ImageType == UploadType.Cover))
                            {
                                var removalResult = await BusinessDirectoryCommandService.RemoveListingProductImage(product.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                            }
                            var additionResult = await BusinessDirectoryCommandService.AddListingProductImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                            if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                        }
                    }
                }

                foreach (var service in _listing.Services)
                {
                    if (string.IsNullOrEmpty(service.CoverImageToUpload))
                    {
                        var request = new Base64ImageUploadRequest()
                        {
                            Name = $"{service.Name} Cover",
                            ImageType = UploadType.Cover,
                            Base64String = _coverImageToUpload
                        };
                        var imageUploadResult = await ImageProcessingService.UploadImage(request);
                        if (imageUploadResult.Succeeded)
                        {
                            if (service.Images.Any(c => c.ImageType == UploadType.Cover))
                            {
                                var removalResult = await BusinessDirectoryCommandService.RemoveListingServiceImage(service.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                            }
                            var additionResult = await BusinessDirectoryCommandService.AddListingServiceImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                            if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                        }
                    }
                }

                await Upload();

                SnackBar.AddSuccess($"{_listing.Heading} was created successfully");

                if (_selectedListingTier.Id != "-1")
                {
                    var result = await ShoppingCartService.AddToCartAsync(_listing.Id, _selectedListingTier);
                    if (!result.Succeeded)
                    {
                        SnackBar.AddErrors(result.Messages);
                    }
                    else
                    {
                        SnackBar.AddSuccess($"Package '{_selectedListingTier.Name} successfully added to cart'");
                    }
                }

                NavigationManager.NavigateTo("/profile");
            });
        }

        /// <summary>
        /// Updates the current business listing, including its details, images, products, and services.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet">
        /// <item><description>Updates the business listing details on the server.</description></item>
        /// <item><description>Uploads a new cover image for the listing, if provided, and replaces the existing
        /// one.</description></item> <item><description>Uploads gallery images for the listing, if any, and associates
        /// them with the listing.</description></item> <item><description>Uploads and updates cover images for each
        /// product in the listing, if applicable.</description></item> <item><description>Uploads and updates cover
        /// images for each service in the listing, if applicable.</description></item> </list> After successfully
        /// completing the update, the method navigates to the profile page and displays a success message.</remarks>
        /// <returns></returns>
        private async Task UpdateAsync()
        {
            var result = await BusinessDirectoryCommandService.CreateAsync(_listing.ToDto() with {UserId = _userId});
            result.ProcessResponseForDisplay(SnackBar, async () =>
            {
                if (!string.IsNullOrEmpty(_coverImageToUpload))
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = $"{_listing.Heading} Logo",
                        ImageType = UploadType.Cover,
                        Base64String = _coverImageToUpload
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        if (_listing.Images.Any(c => c.ImageType == UploadType.Cover))
                        {
                            var removalResult = await BusinessDirectoryCommandService.RemoveImage(_listing.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                            if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        }
                        var additionResult = await BusinessDirectoryCommandService.AddImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                foreach (var galleryImage in _galleryImageToUpload)
                {
                    var request = new Base64ImageUploadRequest()
                    {
                        Name = Guid.NewGuid().ToString(),
                        ImageType = UploadType.Image,
                        Base64String = galleryImage
                    };
                    var imageUploadResult = await ImageProcessingService.UploadImage(request);
                    if (imageUploadResult.Succeeded)
                    {
                        var additionResult = await BusinessDirectoryCommandService.AddImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                        if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    }
                }

                foreach (var product in _listing.Products)
                {
                    if (string.IsNullOrEmpty(product.CoverImageToUpload))
                    {
                        var request = new Base64ImageUploadRequest()
                        {
                            Name = $"{product.Name} Cover",
                            ImageType = UploadType.Cover,
                            Base64String = _coverImageToUpload
                        };
                        var imageUploadResult = await ImageProcessingService.UploadImage(request);
                        if (imageUploadResult.Succeeded)
                        {
                            if (product.Images.Any(c => c.ImageType == UploadType.Cover))
                            {
                                var removalResult = await BusinessDirectoryCommandService.RemoveListingProductImage(product.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                            }
                            var additionResult = await BusinessDirectoryCommandService.AddListingProductImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                            if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                        }
                    }
                }

                foreach (var service in _listing.Services)
                {
                    if (string.IsNullOrEmpty(service.CoverImageToUpload))
                    {
                        var request = new Base64ImageUploadRequest()
                        {
                            Name = $"{service.Name} Cover",
                            ImageType = UploadType.Cover,
                            Base64String = _coverImageToUpload
                        };
                        var imageUploadResult = await ImageProcessingService.UploadImage(request);
                        if (imageUploadResult.Succeeded)
                        {
                            if (service.Images.Any(c => c.ImageType == UploadType.Cover))
                            {
                                var removalResult = await BusinessDirectoryCommandService.RemoveListingServiceImage(service.Images.FirstOrDefault(c => c.ImageType == UploadType.Cover).Id);
                                if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                            }
                            var additionResult = await BusinessDirectoryCommandService.AddListingServiceImage(new AddEntityImageRequest() { EntityId = _listing.Id, ImageId = imageUploadResult.Data.Id });
                            if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                        }
                    }
                }

                await Upload();

                SnackBar.AddSuccess($"{_listing.Heading} was created successfully");

                if (_selectedListingTier?.Order > _listing.Tier?.Order)
                {
                    var result = await ShoppingCartService.AddToCartAsync(_listing.Id, _selectedListingTier);
                    if (!result.Succeeded)
                    {
                        SnackBar.AddErrors(result.Messages);
                    }
                    else
                    {
                        SnackBar.AddSuccess($"Package '{_selectedListingTier.Name} successfully added to cart'");
                    }
                }

                NavigationManager.NavigateTo("/profile");
            });
        }

        /// <summary>
        /// Cancels the current operation and navigates to the business listings page.
        /// </summary>
        /// <remarks>This method redirects the user to the "/businesslistings" route. Ensure that the     
        /// navigation context is valid when calling this method.</remarks>
        private void Cancel()
        {
            NavigationManager.NavigateTo("/profile");
        }

        #region Listing Services

        /// <summary>
        /// Opens a dialog to create a new listing service and adds it to the current listing if the operation succeeds.
        /// </summary>
        /// <remarks>This method displays a modal dialog for creating a new listing service. If the user
        /// confirms the dialog,  the resulting data is sent to the server to add the listing service. Upon a successful
        /// server response,  the new listing service is added to the local collection. If the operation fails, error
        /// messages are displayed.</remarks>
        /// <returns></returns>
        private async Task AddListingService()
        {
            var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };
            var dialog = await DialogService.ShowAsync<AddBusinessListingServiceModal>("Create Listing Service", options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var vm = (ListingServiceViewModel)result.Data;
                var model = vm.ToDto();
                var addResult = await BusinessDirectoryCommandService.AddListingService(model);
                if (addResult.Succeeded)
                {
                    _listing.Services.Add(model);
                }
                else
                {
                    SnackBar.AddErrors(addResult.Messages);
                }
            }
        }

        /// <summary>
        /// Opens a dialog to edit the details of a specified listing service and updates the service if changes are
        /// confirmed.
        /// </summary>
        /// <remarks>This method displays a dialog allowing the user to modify the details of the
        /// specified listing service.  If the user confirms the changes, the updated service is sent to the server for
        /// persistence.  On a successful update, the local listing service collection is updated with the new details. 
        /// If the update fails, error messages are displayed to the user.</remarks>
        /// <param name="listingService">The listing service to be edited. Must be an existing service in the current listing.</param>
        /// <returns></returns>
        private async Task EditListingService(ListingServiceDto listingService)
        {
            var index = _listing.Services.IndexOf(listingService);

            var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };
            var parameters = new DialogParameters<AddBusinessListingServiceModal>
        {
            { x => x.ListingService, new ListingServiceViewModel(listingService) }
        };

            var dialog = await DialogService.ShowAsync<AddBusinessListingServiceModal>("Confirm", parameters, options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var model = ((ListingServiceViewModel)result.Data!).ToDto();
                var updateResult = await BusinessDirectoryCommandService.UpdateListingService(model);
                if (updateResult.Succeeded)
                {
                    _listing.Services[index] = model;
                }
                else
                {
                    SnackBar.AddErrors(updateResult.Messages);
                }
            }
        }

        /// <summary>
        /// Removes a specified service from the current listing after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// removal.  If the user confirms, the service is removed from the listing and the backend is updated.  If the
        /// backend operation fails, error messages are displayed to the user.</remarks>
        /// <param name="listingServiceId">The unique identifier of the service to be removed from the listing. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <returns></returns>
        private async Task RemoveListingService(string listingServiceId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this service from this listing?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deleteResult = await BusinessDirectoryCommandService.RemoveListingService(listingServiceId);
                if (deleteResult.Succeeded)
                {
                    _listing.Services.Remove(_listing.Services.FirstOrDefault(c => c.Id == listingServiceId));
                }
                else
                {
                    SnackBar.AddErrors(deleteResult.Messages);
                }
            }
        }

        #endregion

        #region Listing Products

        /// <summary>
        /// Opens a dialog to create a new listing product and adds it to the current listing if the operation succeeds.
        /// </summary>
        /// <remarks>This method displays a modal dialog for creating a new listing product. If the user
        /// confirms the dialog,  the product is sent to the server for addition. Upon a successful server response, the
        /// product is added  to the local listing. If the operation fails, error messages are displayed in a snackbar
        /// notification.</remarks>
        /// <returns></returns>
        private async Task AddListingProduct()
        {
            var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };
            var parameters = new DialogParameters<AddBusinessListingProductModal>
        {
            { x => x.ListingProduct, new ListingProductViewModel() }
        };

            var dialog = await DialogService.ShowAsync<AddBusinessListingProductModal>("Create Listing Product", parameters, options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var model = ((ListingProductViewModel)result.Data!).ToDto();

                var addResult = await BusinessDirectoryCommandService.AddListingProduct(model);
                if (addResult.Succeeded)
                {
                    _listing.Products.Add(model);
                }
                else
                {
                    SnackBar.AddErrors(addResult.Messages);
                }
            }
        }

        /// <summary>
        /// Opens a dialog to edit the details of a specified listing product and updates the product if changes are
        /// confirmed.
        /// </summary>
        /// <remarks>This method displays a modal dialog allowing the user to modify the details of the
        /// specified product.  If the user confirms the changes, the updated product is sent to the server for
        /// persistence.  Upon a successful update, the product in the local listing is replaced with the updated
        /// version. If the update fails, error messages are displayed using the snack bar.</remarks>
        /// <param name="listingProduct">The product to be edited, represented as a <see cref="ListingProductDto"/>.</param>
        /// <returns></returns>
        private async Task EditListingProduct(ListingProductDto listingProduct)
        {
            var index = _listing.Products.IndexOf(listingProduct);

            var options = new DialogOptions()
            {
                MaxWidth = MaxWidth.Large,
                FullWidth = true
            };

            var parameters = new DialogParameters<AddBusinessListingProductModal>
        {
            { x => x.ListingProduct, new ListingProductViewModel(listingProduct) }
        };

            var dialog = await DialogService.ShowAsync<AddBusinessListingProductModal>("Confirm", parameters, options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var model = ((ListingProductViewModel)result.Data!).ToDto();
                var updateResult = await BusinessDirectoryCommandService.UpdateListingProduct(model);
                if (updateResult.Succeeded)
                {
                    _listing.Products[index] = model;
                }
                else
                {
                    SnackBar.AddErrors(updateResult.Messages);
                }
            }
        }

        /// <summary>
        /// Removes a product from the current listing after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// removal. If the user confirms, the product is removed from the listing and the backend service is updated.
        /// If the removal operation fails, error messages are displayed to the user.</remarks>
        /// <param name="listingProductId">The unique identifier of the product to be removed from the listing. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task RemoveListingProduct(string listingProductId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
        {
            { x => x.ContentText, "Are you sure you want to remove this product from this listing?" },
            { x => x.ButtonText, "Yes" },
            { x => x.Color, Color.Success }
        };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deleteResult = await BusinessDirectoryCommandService.RemoveListingProduct(listingProductId);
                if (deleteResult.Succeeded)
                {
                    _listing.Products.Remove(_listing.Products.FirstOrDefault(c => c.Id == listingProductId));
                }
                else
                {
                    SnackBar.AddErrors(deleteResult.Messages);
                }
            }
        }

        #endregion

        /// <summary>
        /// Asynchronously initializes the component by retrieving and processing business listing and category data.
        /// </summary>
        /// <remarks>This method fetches active business listings and categories from the data provider.
        /// If the data retrieval  is successful, the relevant data is processed and stored for use within the
        /// component. Any error messages  encountered during the data retrieval are displayed using the snack
        /// bar.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _userId = authState.User.GetUserId();

            var result = await BusinessDirectoryQueryService.ActiveListingsAsync();
            if (result.Succeeded)
            {
                var categoryListResult = await BusinessDirectoryCategoryService.CategoriesAsync();
                if (categoryListResult.Succeeded)
                {
                    _availableCategories = categoryListResult.Data.ToList();
                }

                _availableListingTiers.Add(new ListingTierDto() { Id = "-1", Name = "Free Tier" });
                var listingTierResult = await ListingTierQueryService.AllListingTiersAsync();
                if (listingTierResult.Succeeded)
                {
                    _availableListingTiers.AddRange(listingTierResult.Data);
                }
                if (string.IsNullOrEmpty(ListingId))
                {
                    _selectedListingTier = _availableListingTiers.FirstOrDefault();
                }

                var dto = result.Data.FirstOrDefault(l => l.Id == ListingId);
                if (dto != null) _listing = new BusinessListingViewModel(dto);
            }
            SnackBar.AddErrors(result.Messages);
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Uploads a collection of videos asynchronously, tracking progress for each file and handling the upload
        /// results.
        /// </summary>
        /// <remarks>This method uploads videos in parallel, reporting progress for each file and updating
        /// the upload status. If an upload succeeds, the video is associated with the specified entity. If an upload
        /// fails, an error message is displayed.</remarks>
        /// <returns></returns>
        private async Task Upload()
        {
            uploadInProgress = true;

            await Parallel.ForEachAsync(_videosToUpload.ToList(), async (file, cancellationToken) =>
            {
                var uploadResult = new UploadResult
                {
                    Progress = 0,
                    TotalBytes = file.Size
                };

                _filesBusyUploading.Add(uploadResult);

                await using var stream = file.OpenReadStream(500 * 1024 * 1024); // limit server-side too
                using var content = new MultipartFormDataContent();
                var streamContent = new ProgressableStreamContent(stream, 64 * 1024, (uploaded) => uploadResult.Progress = (int)(uploaded * 100 / uploadResult.TotalBytes));
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(streamContent, "file", file.Name);

                var response = await VideoProcessingService.UploadVideoAsync(content, cancellationToken);
                if (response.Succeeded)
                {
                    SnackBar.Add("Upload successful 🎉", Severity.Success);

                    var additionResult = await BusinessDirectoryCommandService.AddVideo(new AddEntityVideoRequest() { EntityId = _listing.Id, VideoId = response.Data.VideoId }, cancellationToken);
                    if (!additionResult.Succeeded) SnackBar.AddErrors(additionResult.Messages);
                    _videosToUpload.Remove(file);
                }

                else
                    SnackBar.Add($"Upload failed: {string.Join(",", response.Messages)}", Severity.Error);

                _filesBusyUploading.Remove(uploadResult);
            });

            uploadInProgress = false;
        }
    }
}
