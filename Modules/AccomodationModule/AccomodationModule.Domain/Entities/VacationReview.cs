using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using FeedbackModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a review associated with a vacation, linking a specific vacation to its review details.
    /// </summary>
    /// <remarks>This class serves as a junction between a vacation and its review, allowing for the
    /// association of reviews with specific vacation entries. It includes foreign key properties to establish
    /// relationships with the <see cref="Vacation"/> and <see cref="Review{T}"/> entities.</remarks>
    public class VacationReview : EntityBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VacationReview"/> class.
        /// </summary>
        public VacationReview() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationReview"/> class with the specified vacation and review
        /// identifiers.
        /// </summary>
        /// <remarks>This constructor sets the identifiers for a vacation and its associated review,
        /// allowing for the creation of a relationship between the two entities.</remarks>
        /// <param name="vacationId">The unique identifier for the vacation. Cannot be null or empty.</param>
        /// <param name="reviewId">The unique identifier for the review. Cannot be null or empty.</param>
        public VacationReview(string vacationId, string reviewId)
        {
            VacationId = vacationId;
            ReviewId = reviewId;
        }

        /// <summary>
        /// Gets or sets the identifier for the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; } 

        /// <summary>
        /// Gets or sets the vacation details for the current user.
        /// </summary>
        public Vacation? Vacation { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated review.
        /// </summary>
        [ForeignKey(nameof(Review))] public string? ReviewId { get; set; }

        /// <summary>
        /// Gets or sets the review associated with a vacation.
        /// </summary>
        public Review<Vacation>? Review { get; set; }
    }
}
