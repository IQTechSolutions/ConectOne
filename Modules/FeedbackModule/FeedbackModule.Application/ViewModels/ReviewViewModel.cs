// ReSharper disable MustUseReturnValue

using System.ComponentModel.DataAnnotations;
using FeedbackModule.Domain.DataTransferObjects;
using FilingModule.Domain.DataTransferObjects;

namespace FeedbackModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel representing a reference, including profile image, company logo, and review details.
    /// </summary>
    public class ReviewViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor for creating a new instance of <see cref="ReviewViewModel"/>.
        /// </summary>
        public ReviewViewModel() { }

        /// <summary>
        /// Initializes a new instance of <see cref="ReviewViewModel"/> using a <see cref="ReviewDto"/>.
        /// </summary>
        /// <param name="reference">The DTO containing reference details.</param>
        public ReviewViewModel(ReviewDto reference)
        {
            Id = reference.Id;
            Name = reference.Name;
            JobTitle = reference.JobTitle;
            CompanyName = reference.CompanyName;
            Location = reference.Location;
            ReviewText = reference.ReviewText;
            EntityId = reference.EntityId;
            Images = reference.Images;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the name of the person providing the reference.
        /// </summary>
        [Required] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the job title of the person providing the reference.
        /// </summary>
        public string? JobTitle { get; set; } 

        /// <summary>
        /// Gets or sets the name of the company associated with the reference.
        /// </summary>
        public string? CompanyName { get; set; } 

        /// <summary>
        /// Gets or sets the location of the person providing the reference.
        /// </summary>
        public string? Location { get; set; } 

        /// <summary>
        /// Gets or sets the review text provided by the person.
        /// </summary>
        [Required] public string ReviewText { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public List<ImageDto> Images { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current review entity to a <see cref="ReviewDto"/> object.
        /// </summary>
        /// <returns>A <see cref="ReviewDto"/> object containing the data from the current review entity.</returns>
        public ReviewDto ToDto()
        {
            return new ReviewDto
            {
                Id = Id,
                Name = Name,
                JobTitle = JobTitle,
                CompanyName = CompanyName,
                Location = Location,
                ReviewText = ReviewText,
                Images = Images
            };
        }

        #endregion
    }
}
