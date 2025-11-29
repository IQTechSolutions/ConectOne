using AdvertisingModule.Domain.Entities;
using AdvertisingModule.Domain.Enums;
using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;

namespace AdvertisingModule.Domain.DataTransferObjects;

/// <summary>
/// Represents an advertisement with details such as title, description, URL, status, and tier.
/// </summary>
/// <remarks>This data transfer object (DTO) is used to encapsulate advertisement information for communication 
/// between different layers of the application. It includes properties for the advertisement's  metadata, status, and
/// associated resources.</remarks>
public record AdvertisementDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AdvertisementDto"/> class using the specified advertisement.
    /// </summary>
    /// <param name="ad">The advertisement from which to populate the DTO. Cannot be <see langword="null"/>.</param>
    public AdvertisementDto(Advertisement ad)
    {
        Id = ad.Id;
        Title = ad.Title;
        Description = ad.Description;
        Url = ad.Url;
        StartDate = ad.StartDate;
        EndDate = ad.EndDate;
        Active = ad.Active;
        Status = ad.Status;
        AdvertisementType = ad.AdvertisementType;
        SetupCompleted = ad.SetupCompleted;
        UserId = ad.UserId;
        AdvertisementTier = ad.AdvertisementTier is not null ?  new AdvertisementTierDto(ad.AdvertisementTier) : null;
        Images = ad.Images is null ? [] : ad.Images.Select(ImageDto.ToDto).ToList();
    }

    #endregion

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public string? Id { get; init; }

    /// <summary>
    /// Gets or sets the title associated with the object.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the URL associated with the current instance.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// Gets or sets the start date of the event or process.
    /// </summary>
    public DateTime? StartDate { get; init; }

    /// <summary>
    /// Gets or sets the end date of the event or process.
    /// </summary>
    public DateTime? EndDate { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the setup process has been completed.
    /// </summary>
    public bool SetupCompleted { get; init; }

    /// <summary>
    /// Gets or sets the identifier of the user that owns the advertisement.
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is active.
    /// </summary>
    public bool Active { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the item has been approved.
    /// </summary>
    public ReviewStatus Status { get; init; }

    /// <summary>
    /// Gets or sets the type of advertisement.
    /// </summary>
    public AdvertisementType AdvertisementType { get; set; }

    /// <summary>
    /// Gets or sets the advertisement tier for the current advertisement.
    /// </summary>
    public AdvertisementTierDto? AdvertisementTier { get; init; }

    /// <summary>
    /// Gets or sets the collection of images associated with the entity.
    /// </summary>
    public ICollection<ImageDto>? Images { get; init; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AdvertisementDto"/> class.
    /// </summary>
    public AdvertisementDto() { }
}