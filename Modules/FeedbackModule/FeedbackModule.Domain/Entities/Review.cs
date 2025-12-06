using FilingModule.Domain.Entities;

namespace FeedbackModule.Domain.Entities
{
    /// <summary>
    /// Represents a review entity, including details such as name, job title, company name, location, and review text.
    /// Inherits from <see cref="FileCollection{TEntity,TId}"/> to manage associated images.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being reviewed.</typeparam>
    public class Review<TEntity> : FileCollection<Review<TEntity>, string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the person providing the review.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the job title of the person providing the review.
        /// </summary>
        public string? JobTitle { get; set; } 

        /// <summary>
        /// Gets or sets the name of the company associated with the review.
        /// </summary>
        public string? CompanyName { get; set; } 
        
        /// <summary>
        /// Gets or sets the location of the person providing the review.
        /// </summary>
        public string? Location { get; set; } 

        /// <summary>
        /// Gets or sets the review text provided by the person.
        /// </summary>
        public string ReviewText { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the entity being reviewed.
        /// </summary>
        public string? EntityId { get; set; } 

        /// <summary>
        /// Navigation property to the entity being reviewed.
        /// </summary>
        public TEntity? Entity { get; set; } = default!;

        #endregion
    }
}
