using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using GroupingModule.Domain.DataTransferObjects;
using LocationModule.Domain.DataTransferObjects;
using ProductsModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a Data Transfer Object (DTO) for a lodging entity, commonly used to transfer lodging-related 
/// data between layers of the application (e.g., from database entities to the presentation layer).
/// This DTO includes various details about a lodging, such as pricing, location, policies, amenities, 
/// categories, and images.
/// </summary>
public class LodgingDto
{
    #region Constructors

    /// <summary>
    /// Default parameterless constructor, often required for serialization or manual property assignment.
    /// </summary>
    public LodgingDto() { }

    /// <summary>
    /// Constructs a <see cref="LodgingDto"/> from a <see cref="Lodging"/> entity and associated pricing information.
    /// This constructor attempts to map all relevant fields from the Lodging domain model to this DTO, 
    /// including images, categories, policies, and related collections.
    /// </summary>
    /// <param name="lodging">The domain entity from which to construct the DTO.</param>
    /// <param name="pricing">A <see cref="PricingDto"/> containing calculated or given pricing details.</param>
    public LodgingDto(Lodging lodging, PricingDto pricing)
    {
        try
        {
            // Basic product details
            ProductId = lodging.Id;
            Name = lodging.Name;
            TeaserText = lodging.Teaser;
            Description = lodging.Description;
            OnlineDescription = lodging.OnlineDescription;
            RoomInformation = lodging.RoomInformation;
            TermsAndConditions = lodging.TermsAndConditions;
            Attractions = lodging.Attractions;
            Grading = lodging.Grading;
            Rating = lodging.Rating;
            Facilities = lodging.Facilities;

            Images = lodging?.Images == null ? [] : lodging.Images.Select(c => ImageDto.ToDto(c)).ToList();
            Videos = lodging?.Videos == null ? [] : lodging.Videos.Select(c => VideoDto.ToDto(c)).ToList();
            // Pricing details
            DefaultRateScheme = lodging.DefaultRateScheme;
            CustomRate = lodging.Rate;
            Pricing = pricing;

            // Guest age cutoff policies
            LowestGuestAgeCutOff = lodging.LowestGuestAgeCutOff;
            MiddleGuestAgeCutOff = lodging.MiddleGuestAgeCutOff;
            HighestGuestAgeCutOff = lodging.HighestGuestAgeCutOff;
            TermsAndConditions = lodging.TermsAndConditions;
            CancellationPolicyDescription = lodging.CancellationPolicy;
            DepositPolicy = lodging.DepositPolicy;
            ChildPolicy = lodging.ChildPolicy;

            // Contact details
            Contacts = lodging.Contacts;
            PhoneNr = lodging.PhoneNr;
            CellNr = lodging.CellNr;
            Email = lodging.Email;
            Website = lodging.Website;

            // Lodging-specific settings
            if (lodging?.Settings is not null)
                Settings = new LodgingSettingsDto(lodging.Settings);

            // Partner / Integration details
            UniqueProductPartnerId = lodging!.UniquePartnerId;

            // SEO / Page details
            PageTitle = lodging.PageTitle;
            MetaKeys = lodging.MetaKeys;
            MetaDescription = lodging.MeteDescription;

            AreaInfo = lodging.AreaInfo;
            Address = lodging.Address;
            Suburb = lodging.Suburb;
            City = lodging.City;
            Lat = lodging.Lat;
            Lng = lodging.Lng;
            Directions = lodging.Directions;
            LodgingType = lodging.LodgingType is null ? null : new LodgingTypeDto(lodging.LodgingType.Id, lodging.LodgingType.Name, lodging.LodgingType.Description);
            Country = lodging.Country is null ? null : new CountryDto(lodging.Country);

            SelectedDestinations = lodging.Destinations?.Select(c => new DestinationDto(c.Destination)).ToList() ?? [];

            // Province and Packages
            ProvinceId = lodging.ProvinceId;
            Packages = lodging.AccountTypes?
                .Where(c => !c.Deleted)
                .Select(c => new LodgingPackageDto(
                    c.Id,
                    c.ShortDescription,
                    c.LongDescription,
                    c.LodgingId,
                    c.AvailablePartnerUid,
                    c.SpecialRateId,
                    c.Rooms,
                    false))
                .ToList();

            // Categories associated with the lodging
            Categories = lodging.Categories?.Select(c => c.Category is null
                ? new CategoryDto()
                : new CategoryDto(
                    c.Category.Id,
                    c.Category.ParentCategoryId,
                    c.Category.Name,
                    c.Category.Description,
                    c.Category.Slogan,
                    c.Category.SubSlogan,
                    c.Category.Images?.FirstOrDefault(x => x.Image.ImageType == UploadType.Cover)?.Image.RelativePath,
                    c.Category.Images.FirstOrDefault(x => x.Image.ImageType == UploadType.Banner)?.Image.RelativePath,
                    c.Category.Images.FirstOrDefault(x => x.Image.ImageType == UploadType.Icon)?.Image.RelativePath,
                    c.Category.EntityCollection.Count,
                    c.Category.SubCategories.Count,
                    c.Category.Active,
                    c.Category.Featured,
                    c.Category.DisplayCategoryInMainManu,
                    c.Category.DisplayAsSliderItem));

            // Amenities associated with the lodging
            Amenities = lodging.Amneties?.Select(c => c.Amenity is null
                ? new AmenityDto()
                : new AmenityDto()
                {
                    AmenityId = c.AmenityId.ToString(),
                    Description = c.Amenity.Description,
                    Name = c.Amenity.Name,
                    Icon = c.Amenity.IconClass
                });

            // Services associated with the lodging
            Services = lodging.Services?.Select(c => new LodgingServiceDto(c));

            // Cancellation rules
            CancellationRules = lodging.CancellationRules?.Select(c => new CancellationRuleDto(c)).ToList();

            Rooms = lodging.Rooms?.Select(c => new RoomDto(c));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion

    #region ProductDetails

    /// <summary>
    /// A unique partner ID used for integration or reference with external systems.
    /// </summary>
    public string? UniqueProductPartnerId { get; init; }

    /// <summary>
    /// The unique ID of the lodging (often the primary key in the database).
    /// </summary>
    public string? ProductId { get; init; }

    /// <summary>
    /// The name of the lodging (e.g., hotel name, guesthouse name).
    /// </summary>
    [Required] public string? Name { get; init; }

    /// <summary>
    /// A short teaser text describing the lodging, often used as a brief introduction.
    /// </summary>
    public string? TeaserText { get; init; }

    /// <summary>
    /// A more detailed description of the lodging, possibly including amenities, style, and atmosphere.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the online description associated with the entity.
    /// </summary>
    public string? OnlineDescription { get; init; }

    /// <summary>
    /// Information specifically about the rooms offered at the lodging.
    /// </summary>
    public string? RoomInformation { get; init; }

    /// <summary>
    /// Nearby attractions or points of interest that might appeal to guests.
    /// </summary>
    public string? Attractions { get; init; }

    /// <summary>
    /// Facilities or amenities available at the lodging (e.g., pool, spa, parking).
    /// </summary>
    public string? Facilities { get; init; }

    /// <summary>
    /// Gets the grading value associated with the current instance.
    /// </summary>
    public int Grading { get; init; }

    /// <summary>
    /// Gets the rating value associated with the entity.
    /// </summary>
    public int Rating { get; init; }

    #endregion

    #region Pricing

    /// <summary>
    /// Indicates if the price has changed since initial load or calculation.
    /// Useful for UI scenarios where a recalculation or confirmation might be needed.
    /// </summary>
    public bool PriceHasChanged { get; set; }

    /// <summary>
    /// The default rate scheme (e.g., per person sharing, per room, etc.).
    /// </summary>
    public RateScheme DefaultRateScheme { get; init; } = RateScheme.PerPersonSharing;

    /// <summary>
    /// A custom rate that may override default pricing logic or represent a special offer.
    /// </summary>
    public double CustomRate { get; set; } = 0;

    /// <summary>
    /// The pricing details associated with this lodging, encapsulated in a <see cref="PricingDto"/>.
    /// </summary>
    public PricingDto? Pricing { get; init; }

    #endregion

    #region Policies

    /// <summary>
    /// The lowest age at which a guest is considered above the first age cutoff (e.g., child vs. adult).
    /// </summary>
    public int LowestGuestAgeCutOff { get; init; }

    /// <summary>
    /// The middle age cutoff used to differentiate pricing or policies between different age groups.
    /// </summary>
    public int MiddleGuestAgeCutOff { get; init; }

    /// <summary>
    /// The highest age cutoff defining the oldest guest category.
    /// </summary>
    public int HighestGuestAgeCutOff { get; init; }

    public string? ChildPolicy { get; set; }

    /// <summary>
    /// Describes the deposit policy (e.g., amount or percentage required upfront).
    /// </summary>
    public string? DepositPolicy { get; set; }

    /// <summary>
    /// Terms and conditions associated with staying at the lodging.
    /// </summary>
    public string? TermsAndConditions { get; init; }

    /// <summary>
    /// A textual description of the cancellation policy.
    /// </summary>
    public string? CancellationPolicyDescription { get; set; }

    /// <summary>
    /// A collection of detailed cancellation rules, allowing for more granular conditions than a single description.
    /// </summary>
    public IEnumerable<CancellationRuleDto>? CancellationRules { get; set; } = new List<CancellationRuleDto>();

    #endregion

    #region Contact Information

    /// <summary>
    /// Any additional contact information (e.g., multiple contact persons or notes).
    /// </summary>
    public string? Contacts { get; init; }

    /// <summary>
    /// Primary phone number of the lodging.
    /// </summary>
    public string? PhoneNr { get; init; }

    /// <summary>
    /// Fax number of the lodging (if applicable).
    /// </summary>
    public string? FaxNr { get; init; }

    /// <summary>
    /// Cellphone number of the lodging or its representative.
    /// </summary>
    public string? CellNr { get; init; }

    /// <summary>
    /// Email address for inquiries or reservations.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Website URL for the lodging, if available.
    /// </summary>
    public string? Website { get; init; }

    #endregion

    /// <summary>
    /// Gets the collection of selected destinations.
    /// </summary>
    public IEnumerable<DestinationDto>? SelectedDestinations { get; init; } = [];

    /// <summary>
    /// Gets the type of lodging associated with this entity.
    /// </summary>
    public LodgingTypeDto? LodgingType { get; init; } = null!;

    /// <summary>
    /// Settings that configure the behavior, availability, or features of this lodging.
    /// </summary>
    public LodgingSettingsDto? Settings { get; init; } = null!;

    #region Location

    /// <summary>
    /// A location ID that might refer to a broader region or location record.
    /// </summary>
    public int LocationId { get; set; }

    /// <summary>
    /// The ID of the area in which the lodging is located.
    /// </summary>
    public int AreaId { get; set; }

    /// <summary>
    /// Additional information about the area where the lodging is located (e.g., neighborhood details).
    /// </summary>
    public string? AreaInfo { get; set; }

    /// <summary>
    /// The street address or descriptive address of the lodging.
    /// </summary>
    public string? Address { get; init; }

    /// <summary>
    /// The suburb in which the lodging is located.
    /// </summary>
    public string? Suburb { get; set; }

    /// <summary>
    /// The city in which the lodging is located.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// The latitude coordinate of the lodging for mapping purposes.
    /// </summary>
    public double Lat { get; init; }

    /// <summary>
    /// The longitude coordinate of the lodging for mapping purposes.
    /// </summary>
    public double Lng { get; init; }

    /// <summary>
    /// Gets the country information associated with the entity.
    /// </summary>
    public CountryDto? Country { get; init; }

    /// <summary>
    /// A link to a map (e.g., Google Maps URL) for quick directions to the lodging.
    /// </summary>
    public string? MapLink { get; init; }

    /// <summary>
    /// Driving or walking directions to help guests find the lodging.
    /// </summary>
    public string? Directions { get; set; }

    #endregion

    #region Page Details

    /// <summary>
    /// The page title, often used for SEO or display in the browser title bar.
    /// </summary>
    public string? PageTitle { get; init; }

    /// <summary>
    /// Meta keywords for SEO.
    /// </summary>
    public string? MetaKeys { get; init; }

    /// <summary>
    /// Meta description for SEO, describing the content of the page.
    /// </summary>
    public string? MetaDescription { get; init; }

    #endregion

    public AreaDto? Area { get; set; }

    /// <summary>
    /// The date the lodging record was created (if available).
    /// </summary>
    public DateTime? CreatedDate { get; init; }

    /// <summary>
    /// The date the lodging record was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; init; }

    /// <summary>
    /// The product category ID associated with the lodging, often for classification.
    /// </summary>
    public int? ProductCategoryId { get; init; }

    /// <summary>
    /// The province ID where the lodging is located.
    /// </summary>
    public int? ProvinceId { get; init; }

    /// <summary>
    /// A collection of categories (like tags or classifications) that the lodging belongs to.
    /// </summary>
    public IEnumerable<CategoryDto> Categories { get; init; } = [];

    /// <summary>
    /// A collection of packages (rate plans, deals, special offers) available for the lodging.
    /// </summary>
    public IEnumerable<LodgingPackageDto> Packages { get; init; } = [];

    /// <summary>
    /// A collection of amenities offered by the lodging (e.g., Wi-Fi, breakfast, air conditioning).
    /// </summary>
    public IEnumerable<AmenityDto> Amenities { get; init; } = [];

    /// <summary>
    /// A collection of services provided by the lodging (e.g., room service, shuttle bus).
    /// </summary>
    public IEnumerable<LodgingServiceDto> Services { get; init; } = [];

    /// <summary>
    /// Gets the collection of rooms available in the system.
    /// </summary>
    public IEnumerable<RoomDto> Rooms { get; init; } = [];

    #region Videos & Videos

    /// <summary>
    /// Gets or sets the collection of images associated with the entity.
    /// </summary>
    public ICollection<ImageDto> Images { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of video files to be displayed.
    /// </summary>
    public List<VideoDto> Videos { get; set; } = [];

    #endregion

    #region Properties

    /// <summary>
    /// Creates a new <see cref="Lodging"/> domain entity based on the current DTO's properties.
    /// This method is useful for persisting a lodging object back into the database.
    /// </summary>
    /// <returns>A new <see cref="Lodging"/> domain entity.</returns>
    public Lodging Create()
    {
        return new Lodging()
        {
            Id = ProductId,
            Name = this.Name,
            Teaser = this.TeaserText,
            Description = this.Description,
            OnlineDescription = this.OnlineDescription,
            RoomInformation = this.RoomInformation,
            TermsAndConditions = this.TermsAndConditions,
            UniquePartnerId = this.UniqueProductPartnerId,
            DepositPolicy = this.DepositPolicy,
            ChildPolicy = ChildPolicy,
            Attractions = this.Attractions,
            Grading = Grading,
            Rating = Rating,
            Facilities = this.Facilities,

            LodgingTypeId = LodgingType.Id,
            CountryId = Country.CountryId,
            Rate = this.CustomRate,

            LowestGuestAgeCutOff = this.LowestGuestAgeCutOff,
            MiddleGuestAgeCutOff = this.MiddleGuestAgeCutOff,
            HighestGuestAgeCutOff = this.HighestGuestAgeCutOff,

            Contacts = this.Contacts,
            PhoneNr = this.PhoneNr,
            CellNr = this.CellNr,
            Email = this.Email,
            Website = this.Website,

            Settings = LodgingSettingsDto.ToLodgingSettings(this.Settings),
            CancellationRules = this.CancellationRules?.Select(c => c.ToCancellationRule())?.ToList(),

            Destinations = SelectedDestinations?.Select(c => new LodgingDestination() { DestinationId = c.DestinationId })?.ToList(),

            PageTitle = this.Name,

            AreaInfo = this.AreaInfo,
            Address = this.Address,
            ProvinceId = this.ProvinceId,
            Suburb = this.Suburb,
            City = this.City,
            Lat = this.Lat,
            Lng = this.Lng,
            Directions = this.Directions,
        };
    }

    #endregion
}
