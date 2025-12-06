using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a featured image, providing details such as its identifier, URL, and cover image
    /// status.
    /// </summary>
    /// <remarks>This class is designed to encapsulate the data required to display and manage a featured
    /// image in a user interface. It supports initialization from different data sources, such as a DTO or an image
    /// category collection.</remarks>
    public class FeaturedImageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturedImageViewModel"/> class.
        /// </summary>
        public FeaturedImageViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturedImageViewModel"/> class using the specified featured
        /// image data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="FeaturedImageDto"/>
        /// to the corresponding properties of the <see cref="FeaturedImageViewModel"/> instance.</remarks>
        /// <param name="image">The data transfer object containing information about the featured image. Must not be <see
        /// langword="null"/>.</param>
        public FeaturedImageViewModel(FeaturedImageDto image)
        {
            FeaturedImageId = image.FeaturedImageId;
            IsCoverImage = image.IsCoverImage;
            ImageUrl = image.ImageUrl;
        }

        /// <summary>
        /// Gets or sets the identifier of the featured image.
        /// </summary>
        public string? FeaturedImageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image is designated as the cover image.
        /// </summary>
        public bool IsCoverImage { get; set; }

        /// <summary>
        /// Gets or sets the URL of the image associated with this object.
        /// </summary>
        public string ImageUrl { get; set; } = null!;

        /// <summary>
        /// Gets a value indicating whether the "Add to Featured" option should be displayed.
        /// </summary>
        /// <remarks>The "Add to Featured" option is displayed only when the item is not a cover
        /// image.</remarks>
        public bool DisplayAddToFeatured => !IsCoverImage;
    }
}
