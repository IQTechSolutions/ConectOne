using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="MealAdditionTemplate"/>.
/// </summary>
public record MealAdditionTemplateDto
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public MealAdditionTemplateDto() {}

    /// <summary>
    /// Initialize from entity
    /// </summary>
    /// <param name="entity">Entity to map from</param>
    public MealAdditionTemplateDto(MealAdditionTemplate entity)
    {
        Id = entity.Id;
        Restaurant = entity.Restaurant == null ? new RestaurantDto() : new RestaurantDto(entity.Restaurant);
        GuestType = entity.GuestType;
        MealType = entity.MealType;
        Notes = entity.Notes;
    }

    #endregion

    /// <summary>
    /// Gets the identifier
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Gets the restaurant
    /// </summary>
    public RestaurantDto Restaurant { get; init; } = null!;

    /// <summary>
    /// Gets the guest type
    /// </summary>
    public GuestType GuestType { get; init; }

    /// <summary>
    /// Gets the meal type
    /// </summary>
    public MealType MealType { get; init; }

    /// <summary>
    /// Additional notes
    /// </summary>
    public string? Notes { get; init; }
}
