using AccomodationModule.Domain.DataTransferObjects;
using FeedbackModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// Represents a view model for vacation reviews, providing data binding between the UI and the underlying data transfer
/// object (DTO).
/// </summary>
/// <remarks>This class is designed to facilitate the transfer of vacation review data between the application
/// layers. It includes properties for the review's unique identifier, the associated entity identifier, and the review
/// details.</remarks>
public class VacationReviewViewModel
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="VacationReviewViewModel"/> class.
    /// </summary>
    public VacationReviewViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="VacationReviewViewModel"/> class using the specified data transfer
    /// object.
    /// </summary>
    /// <param name="dto">The data transfer object containing the vacation review details. Cannot be <see langword="null"/>.</param>
    public VacationReviewViewModel(VacationReviewDto dto)
    {
        Id = dto.Id;
        EntityId = dto.EntityId;
        Review = dto.Review;
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
    public string EntityId { get; set; }

    /// <summary>
    /// Gets or sets the review details associated with the current entity.
    /// </summary>
    public ReviewDto Review { get; set; } = new ReviewDto();

    #endregion

    #region Methods    

    /// <summary>
    /// Converts the current instance of the vacation review to a data transfer object (DTO).
    /// </summary>
    /// <returns>A <see cref="VacationReviewDto"/> representing the current vacation review,  including its identifier,
    /// associated entity identifier, and review content.</returns>
    public VacationReviewDto ToDto()
    {
        return new VacationReviewDto
        {
            Id = Id,
            EntityId = EntityId,
            Review = Review
        };
    }

    #endregion
    
}
