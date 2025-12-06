// ReSharper disable MustUseReturnValue

using FeedbackModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace FeedbackModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for a review entity.
    /// This DTO is used to transfer review data between different layers of the application.
    /// </summary>
    public record ReviewDto
    {
        #region Properties

        /// <summary>
        /// Gets or initializes the ID of the review.
        /// </summary>
        public string Id { get; init; } 

        /// <summary>
        /// Gets or initializes the name of the person providing the review.
        /// </summary>
        public string Name { get; init; } 

        /// <summary>
        /// Gets or initializes the job title of the person providing the review.
        /// </summary>
        public string? JobTitle { get; init; } 

        /// <summary>
        /// Gets or initializes the name of the company associated with the review.
        /// </summary>
        public string? CompanyName { get; init; } 

        /// <summary>
        /// Gets or initializes the location of the person providing the review.
        /// </summary>
        public string? Location { get; init; } 

        /// <summary>
        /// Gets or initializes the review text provided by the person.
        /// </summary>
        public string ReviewText { get; init; }

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public string? EntityId { get; init; } 

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public List<ImageDto> Images { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance to a <see cref="Review{TItem}"/> object.
        /// </summary>
        /// <typeparam name="TItem">The type of the item associated with the review.</typeparam>
        /// <returns>A new <see cref="Review{TItem}"/> object containing the data from the current instance.</returns>
        public Review<TItem> ToReview<TItem>()
        {
            var review = new Review<TItem>
            {
                Id = Id,
                Name = Name,
                JobTitle = JobTitle,
                CompanyName = CompanyName,
                Location = Location,
                ReviewText = ReviewText
            };

            return review;
        }
        
        /// <summary>
        /// Converts a <see cref="Review{TItem}"/> entity to a <see cref="ReviewDto"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the entity being reviewed.</typeparam>
        /// <param name="review">The review entity to convert.</param>
        /// <returns>A <see cref="ReviewDto"/> with the entity's data.</returns>
        public static ReviewDto ToReviewDto<TItem>(Review<TItem> review)
        {
            var dto = new ReviewDto
            {
                Id = review.Id,
                Name = review.Name,
                JobTitle = review.JobTitle,
                CompanyName = review.CompanyName,
                Location = review.Location,
                ReviewText = review.ReviewText,
                Images = review.Images.Select(ImageDto.ToDto).ToList()
            };

            return dto;
        }

        #endregion
    }
}
