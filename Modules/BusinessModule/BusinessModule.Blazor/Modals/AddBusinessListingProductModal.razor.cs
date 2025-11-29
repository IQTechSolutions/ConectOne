using BusinessModule.Application.ViewModel;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace BusinessModule.Blazor.Modals
{
    /// <summary>
    /// The AddEventToActivityCategoryModal component is responsible for selecting a category
    /// to which an event will be added. It provides a list of categories and handles the selection process.
    /// </summary>
    public partial class AddBusinessListingProductModal
    {
        private bool _loaded = false;
        private string _imageSource = "_content/BusinessModule.Blazor/background.png";
        private string? _coverImageToUpload;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>The configuration data can include settings from various sources such as
        /// appsettings.json, environment variables, or user secrets. Ensure that the configuration is properly
        /// initialized before accessing its values.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// The parent category for which the event is being added.
        /// </summary>
        [Parameter] public ListingProductViewModel? ListingProduct { get; set; } = new();

        /// <summary>
        /// The MudDialog instance for managing the dialog state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Updates the cover image based on the provided cropper response.
        /// </summary>
        /// <remarks>This method updates the internal state with the new cover image data, preparing it for
        /// further processing or upload.</remarks>
        /// <param name="coverImage">The response from the cropper containing the updated cover image data. The <see
        /// cref="MudCropperResponse.Base64String"/> property must contain the image data in Base64 format.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            ListingProduct.CoverImageToUpload = _imageSource;
        }

        /// <summary>
        /// Submits the selected category and closes the dialog.
        /// </summary>
        public void SubmitAsync()
        {
            MudDialog.Close(ListingProduct);
        }

        /// <summary>
        /// Cancels the selection process and closes the dialog.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            if(ListingProduct?.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath is not null)
                _imageSource = $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{ListingProduct?.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover)?.RelativePath}";
            await base.OnInitializedAsync();
        }
    }
}

