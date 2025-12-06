using ConectOne.Domain.RequestFeatures;

namespace FeedbackModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents parameters for filtering and retrieving review related data.
    /// Inherits pagination, sorting, and searching capabilities from RequestParameters.
    /// </summary>
    public class ReviewPageParameters : RequestParameters
    {

        /// <summary>
        /// Gets or sets the unique identifier for the review entity.
        /// Can be null if filtering by vacation ID is not required.
        /// </summary>
        public string? EntityId { get; set; }

    }
}
