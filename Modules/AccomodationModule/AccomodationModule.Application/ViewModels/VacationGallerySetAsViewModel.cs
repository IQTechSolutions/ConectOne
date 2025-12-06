using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for setting a vacation gallery image.
    /// </summary>
    /// <remarks>This view model is used to specify the vacation and the type of image to be set in the
    /// gallery.</remarks>
    public class VacationGallerySetAsViewModel
    {
        /// <summary>
        /// Gets or sets the vacation details for the user.
        /// </summary>
        public VacationDto Vacation { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of the image.
        /// </summary>
        public string ImageType { get; set; } = null!;
    }
}
