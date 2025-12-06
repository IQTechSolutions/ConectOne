using AccomodationModule.Application.ViewModels;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace Accomodation.Blazor.Components.Lodgings
{
    /// <summary>
    /// Represents the details of a lodging, including its view model and configuration settings.
    /// </summary>
    /// <remarks>This class is designed to manage and display lodging-related information, such as the
    /// lodging's view model and the state of specific features. It also handles initialization logic and updates to the
    /// cover image source.</remarks>
    public partial class LodgingDetails
    {
        /// <summary>
        /// The source URL of the cover image.
        /// </summary>
        private string _imageSource = "_content/Accomodation.Blazor/images/NoImage.jpg";

        #region Parameters

        /// <summary>
        /// The lodging view model containing details of the lodging.
        /// </summary>
        [Parameter, EditorRequired] public LodgingViewModel Lodging { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the Kuburi feature is enabled.
        /// </summary>
        [Parameter]public bool KuburiPropery { get; set; } = false;

        /// <summary>
        /// Gets or sets the callback that is invoked when the cover image is changed by the user.
        /// </summary>
        /// <remarks>The callback receives a <see cref="MudCropperResponse"/> containing the updated image
        /// data. Use this event to handle cover image updates, such as saving the new image or updating the
        /// UI.</remarks>
        [Parameter] public EventCallback<MudCropperResponse> OnCoverImageChanged { get; set; }

        #endregion

        #region Injections

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>Use this property to access configuration values such as connection strings,
        /// application options, or environment-specific settings. The property is typically injected by the dependency
        /// injection framework.</remarks>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Updates the cover image source URL when the cover image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private async Task CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
            await OnCoverImageChanged.InvokeAsync(coverImage);
        }

        /// <summary>
        /// Initializes the component and sets the initial cover image source URL.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _imageSource = Lodging.Details.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover) is null ? "_content/Accomodation.Blazor/images/NoImage.jpg" : $"{Configuration["ApiConfiguration:BaseApiAddress"]}\\{Lodging.Details.Images?.FirstOrDefault(c => c.ImageType == UploadType.Cover).RelativePath.TrimStart('/')}";
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
