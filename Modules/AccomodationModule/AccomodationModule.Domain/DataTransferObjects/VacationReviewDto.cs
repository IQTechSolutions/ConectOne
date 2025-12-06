using AccomodationModule.Domain.Entities;
using FeedbackModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a data transfer object (DTO) for a vacation review, encapsulating the review details and associated
/// vacation entity information.
/// </summary>
/// <remarks>This DTO is designed to facilitate the transfer of vacation review data between different layers of
/// the application. It includes the review details and the identifier of the associated vacation entity.</remarks>
public record VacationReviewDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VacationReviewDto"/> class.
    /// </summary>
    public VacationReviewDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="VacationReviewDto"/> class using the specified <see
    /// cref="VacationReview"/> model.
    /// </summary>
    /// <remarks>This constructor maps the properties of the provided <see cref="VacationReview"/> model to
    /// the corresponding properties of the DTO. If the <paramref name="model"/> contains a <c>null</c> review, a
    /// default <see cref="ReviewDto"/> instance is used.</remarks>
    /// <param name="model">The <see cref="VacationReview"/> model containing the data to initialize the DTO. Cannot be <c>null</c>.</param>
    public VacationReviewDto(VacationReview model)
    {
        Id = model.Id;
        EntityId = model.VacationId;
        Review = model.Review == null ? new ReviewDto() : ReviewDto.ToReviewDto(model.Review);
    }

    /// <summary>
    /// Gets the unique identifier for the object.
    /// </summary>
    public string Id { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public string EntityId { get; init; }

    /// <summary>
    /// Gets the review details associated with the current entity.
    /// </summary>
    public ReviewDto Review { get; init; } = new ReviewDto();
}
