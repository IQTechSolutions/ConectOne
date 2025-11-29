using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Attributes;
using ConectOne.Domain.Enums;

namespace AdvertisingModule.Application.ViewModels;

/// <summary>
/// View model used to capture advertisement data from the UI.
/// </summary>
public class AdvertisementViewModel
{
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AdvertisementViewModel"/> class.
    /// </summary>
    public AdvertisementViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AdvertisementViewModel"/> class using the specified advertisement
    /// data transfer object.
    /// </summary>
    /// <param name="dto">The data transfer object containing advertisement details. Cannot be <see langword="null"/>.</param>
    public AdvertisementViewModel(AdvertisementDto dto)
    {
        Id = dto.Id;
        Title = dto.Title;
        Description = dto.Description;
        Url = dto.Url;
        StartDate = dto.StartDate;
        EndDate = dto.EndDate;
        Active = dto.Active;
        Status = dto.Status;
        AdvertisementType = dto.AdvertisementType;
        SetupCompleted = dto.SetupCompleted;
        AdvertisementTier = dto.AdvertisementTier;
        Images = dto.Images ?? [];
        UserId = dto.UserId;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the entity.
    /// </summary>
    [Required] public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the URL associated with the current instance.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the start date of the event or process.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the event or process.
    /// </summary>
    [DateGreaterThan("StartDate", ErrorMessage = "End Date must be after Start Date.")]
    [RequiredIfOtherPropertyHasValueAttribute(nameof(StartDate), ErrorMessage = "This field is required if start date has a value")]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is active.
    /// </summary>
    public bool Active { get; }

    /// <summary>
    /// Indicates whether the advertisement has been approved for display.
    /// </summary>
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

    /// <summary>
    /// Gets or sets a value indicating whether the setup process has been completed.
    /// </summary>
    public bool SetupCompleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the type of advertisement.
    /// </summary>
    public AdvertisementType AdvertisementType { get; set; }

    /// <summary>
    /// Gets or sets the advertisement tier for the current advertisement.
    /// </summary>
    public AdvertisementTierDto AdvertisementTier { get; set; }

    /// <summary>
    /// Gets or sets the user identifier associated with the advertisement.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the collection of images associated with the entity.
    /// </summary>
    public ICollection<ImageDto> Images { get; set; } = [];

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current advertisement entity to its corresponding data transfer object (DTO).
    /// </summary>
    /// <returns>An <see cref="AdvertisementDto"/> instance containing the data from the current advertisement.</returns>
    public AdvertisementDto ToDto()
    {
        return new AdvertisementDto
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Url = Url,
            StartDate = StartDate,
            EndDate = EndDate,
            Active = Active,
            Status = Status,
            AdvertisementType = AdvertisementType,
            SetupCompleted = SetupCompleted,
            UserId = UserId,
            AdvertisementTier = AdvertisementTier,
            Images = Images
        };
    }

    #endregion
}