using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// View model used to create or edit <see cref="GiftDto"/> instances.
/// </summary>
public class GiftViewModel
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GiftViewModel"/> class.
    /// </summary>
    public GiftViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GiftViewModel"/> class using the specified DTO.
    /// </summary>
    /// <param name="dto">DTO containing gift information.</param>
    public GiftViewModel(GiftDto dto)
    {
        GiftId = dto.GiftId;
        Name = dto.Name;
        Description = dto.Description;
        GuestType = (int)dto.GuestType;
        TimeDescription = dto.TimeDescription;
        GiftType = (int)dto.GiftType;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the gift identifier.
    /// </summary>
    public string GiftId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the unique identifier for the gift.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the gift description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the guest type value.
    /// </summary>
    public int GuestType { get; set; } = (int)Domain.Enums.GuestType.All;

    /// <summary>
    /// Gets or sets the time description.
    /// </summary>
    public string? TimeDescription { get; set; }

    /// <summary>
    /// Gets or sets the type of gift.
    /// </summary>
    public int GiftType { get; set; } = (int)Domain.Enums.GiftType.Room;

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current <see cref="Gift"/> instance to a <see cref="GiftDto"/> representation.
    /// </summary>
    /// <remarks>This method maps the properties of the <see cref="Gift"/> instance to their corresponding
    /// properties in the <see cref="GiftDto"/> object. Enum properties are cast to their respective types in the <see
    /// cref="Domain.Enums"/> namespace.</remarks>
    /// <returns>A <see cref="GiftDto"/> object containing the data from the current <see cref="Gift"/> instance.</returns>
    public GiftDto ToDto()
    {
        return new GiftDto
        {
            GiftId = this.GiftId,
            Name = this.Name,
            Description = this.Description,
            GuestType = (Domain.Enums.GuestType)this.GuestType,
            TimeDescription = this.TimeDescription,
            GiftType = (Domain.Enums.GiftType)this.GiftType
        };
    }

    #endregion
}
