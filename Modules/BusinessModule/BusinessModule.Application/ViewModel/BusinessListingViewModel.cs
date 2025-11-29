using System.ComponentModel.DataAnnotations;
using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Entities;
using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.DataTransferObjects;

namespace BusinessModule.Application.ViewModel;

/// <summary>
/// View model used for creating and editing business listings.
/// </summary>
public class BusinessListingViewModel
{
    #region Constructors

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BusinessListingViewModel() { }

    /// <summary>
    /// Creates a view model from a DTO instance.
    /// </summary>
    public BusinessListingViewModel(BusinessListingDto dto)
    {
        Id = dto.Id;
        Heading = dto.Heading;
        Slogan = dto.Slogan;
        ActiveUntil = dto.ActiveUntil;
        Address = dto.Address;
        PhoneNumber = dto.PhoneNumber;
        Email = dto.Email;
        Description = dto.Description;
        Categories = dto.Categories;
        WebsiteUrl = dto.WebsiteUrl;
        Tier = dto.Tier;
        Status = dto.Status;
        Images = dto.Images;
        Videos = dto.Videos;
        Services = dto.Services;
        Products = dto.Products;
        UserId = dto.UserId;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the heading text for the content.
    /// </summary>
    [Required(ErrorMessage = "Heading is required")] public string Heading { get; set; }
    /// <summary>
    /// Gets or sets the slogan associated with the entity.
    /// </summary>
    public string? Slogan { get; set; }

    /// <summary>
    /// Gets or sets the date and time until which the entity remains active.
    /// </summary>
    public DateTime? ActiveUntil { get; set; }

    /// <summary>
    /// Gets or sets the address associated with the entity.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// </summary>
    [EmailAddress, Required(ErrorMessage = "Email Address is required")] public string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number associated with the entity.
    /// </summary>
    [Required(ErrorMessage = "Phone Nr is required"), MinLength(11)] public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the URL of the website associated with this entity.
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// Gets or sets the tier of the listing, which determines the features and limitations available.
    /// </summary>
    public ListingTierDto? Tier { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item has been approved.
    /// </summary>
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

    /// <summary>
    /// Gets or sets the collection of images associated with the entity.
    /// </summary>
    public List<ImageDto> Images { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of video files to be displayed.
    /// </summary>
    public List<VideoDto> Videos { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of categories associated with the entity.
    /// </summary>
    public IEnumerable<CategoryDto> Categories { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of services associated with the listing.
    /// </summary>
    public List<ListingServiceDto> Services { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of products associated with the listing.
    /// </summary>
    public List<ListingProductDto> Products { get; set; } = [];

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current <see cref="BusinessListing"/> instance to a <see cref="BusinessListingDto"/>.
    /// </summary>
    /// <remarks>This method creates a new <see cref="BusinessListingDto"/> object and populates it with the
    /// values of the corresponding properties from the current <see cref="BusinessListing"/> instance.</remarks>
    /// <returns>A <see cref="BusinessListingDto"/> that represents the current <see cref="BusinessListing"/> instance, including
    /// its associated properties such as <see cref="Id"/>, <see cref="Heading"/>, <see cref="Slogan"/>, and other
    /// related details.</returns>
    public BusinessListingDto ToDto()
    {
        return new BusinessListingDto
        {
            Id = Id,
            Heading = Heading,
            Slogan = Slogan,
            ActiveUntil = ActiveUntil,
            Address = Address,
            PhoneNumber = PhoneNumber,
            Email = Email,
            Description = Description,
            Categories = Categories,
            WebsiteUrl = WebsiteUrl,
            Tier = Tier,
            Status = Status,
            Images = Images,
            Videos = Videos,
            Services = Services,
            Products = Products,
            UserId = UserId
        };
    }

    #endregion
}
