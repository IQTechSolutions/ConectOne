using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// ViewModel for representing meal addition templates.
/// </summary>
public class MealAdditionTemplateViewModel
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MealAdditionTemplateViewModel"/> class.
    /// </summary>
    public MealAdditionTemplateViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MealAdditionTemplateViewModel"/> class using a DTO.
    /// </summary>
    /// <param name="dto">The source <see cref="MealAdditionTemplateDto"/>.</param>
    public MealAdditionTemplateViewModel(MealAdditionTemplateDto dto)
    {
        Id = dto.Id;
        Restaurant = dto.Restaurant;
        GuestType = dto.GuestType;
        MealType = dto.MealType;
        Notes = dto.Notes;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the restaurant associated with this template.
    /// </summary>
    public RestaurantDto Restaurant { get; set; } = new RestaurantDto();

    /// <summary>
    /// Gets or sets the guest type for the meal addition.
    /// </summary>
    public GuestType GuestType { get; set; }

    /// <summary>
    /// Gets or sets the meal type for the meal addition.
    /// </summary>
    public MealType MealType { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    public string? Notes { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current instance of <see cref="MealAdditionTemplate"/> to a <see cref="MealAdditionTemplateDto"/>.
    /// </summary>
    /// <returns>A <see cref="MealAdditionTemplateDto"/> that represents the current instance, including its <see cref="Id"/>,
    /// <see cref="Restaurant"/>, <see cref="GuestType"/>, <see cref="MealType"/>, and <see cref="Notes"/>.</returns>
    public MealAdditionTemplateDto ToDto()
    {
        return new MealAdditionTemplateDto
        {
            Id = this.Id,
            Restaurant = this.Restaurant,
            GuestType = this.GuestType,
            MealType = this.MealType,
            Notes = this.Notes
        };
    }

    #endregion
}
