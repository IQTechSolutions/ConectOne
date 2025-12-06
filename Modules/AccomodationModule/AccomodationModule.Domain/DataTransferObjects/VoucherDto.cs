using AccomodationModule.Domain.Entities;
using ProductsModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a Data Transfer Object (DTO) for a lodging voucher. 
/// A voucher typically grants certain benefits or discounts for lodging services. 
/// This DTO is used to transfer voucher-related data between layers of the application.
/// </summary>
public record VoucherDto
{
    #region Constructor

    /// <summary>
    /// Default parameterless constructor, often required for serialization or manual instantiation.
    /// </summary>
    public VoucherDto() { }

    /// <summary>
    /// Constructs a new <see cref="VoucherDto"/> instance from a given <see cref="Voucher"/> entity.
    /// Maps the properties of the domain model (Voucher) into the DTO format, 
    /// making it suitable for passing to the presentation or client layers.
    /// </summary>
    /// <param name="voucher">A domain entity representing the voucher.</param>
    public VoucherDto(Voucher voucher)
    {
        // Basic voucher details
        VoucherId = voucher.Id;
        Title = voucher.Title;
        LodgingId = voucher.LodgingId!;
        ShortDescription = voucher.ShortDescription;
        LongDescription = voucher.LongDescription;
        Rate = voucher.Rate;

        // Financial details
        MarkupPercentage = voucher.MarkupPercentage;
        Commission = voucher.Commission;

        // Additional voucher info
        Features = voucher.Features;
        Terms = voucher.Terms;
        Active = voucher.Active;
        Featured = voucher.Featured;

        // Lodging and room details associated with the voucher
        // The LodgingDto constructor takes the Lodging object and a PricingDto 
        // (constructed from the Lodging itself), providing pricing context for the lodging.
        Lodging = new LodgingDto(voucher.Lodging!, new PricingDto());
        Rooms = voucher.Rooms.Select(c => new RoomDto(c)).ToList();
    }

    #endregion

    /// <summary>
    /// Unique identifier for the voucher (primary key).
    /// </summary>
    public int? VoucherId { get; init; }

    /// <summary>
    /// Title of the voucher, generally a short text used to identify the voucher.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// A brief description of the voucher, often displayed to users as an overview.
    /// </summary>
    public string ShortDescription { get; init; } = null!;

    /// <summary>
    /// A detailed, longer description of the voucher, including any terms, conditions, or highlights.
    /// </summary>
    public string LongDescription { get; init; } = null!;

    /// <summary>
    /// The base rate or price for the voucher, representing its cost to the end user.
    /// </summary>
    public double Rate { get; init; }

    /// <summary>
    /// The markup percentage added to the voucher's base rate, often used to add profit margins.
    /// </summary>
    public double MarkupPercentage { get; set; }

    /// <summary>
    /// The commission associated with the voucher, possibly to be paid to a reseller or partner.
    /// </summary>
    public double Commission { get; set; }

    /// <summary>
    /// Additional features or benefits included in the voucher, presented as text.
    /// </summary>
    public string Features { get; set; } = null!;

    /// <summary>
    /// The terms of usage for the voucher, detailing any restrictions, expiration dates, or usage conditions.
    /// </summary>
    public string Terms { get; set; } = null!;

    /// <summary>
    /// Indicates whether the voucher is currently active and available for use or purchase.
    /// </summary>
    public bool Active { get; init; }

    /// <summary>
    /// Indicates whether the voucher is featured, potentially highlighting it to customers as a special offer.
    /// </summary>
    public bool Featured { get; init; }

    /// <summary>
    /// The unique ID of the associated lodging entity for which this voucher applies.
    /// </summary>
    public string? LodgingId { get; init; } 

    /// <summary>
    /// A <see cref="LodgingDto"/> representing the lodging this voucher applies to,
    /// providing additional context such as lodging details and pricing.
    /// </summary>
    public LodgingDto? Lodging { get; set; }

    /// <summary>
    /// A collection of <see cref="RoomDto"/> objects that represent the rooms included in or applicable to this voucher.
    /// This may help customers understand what type of accommodations the voucher covers.
    /// </summary>
    public ICollection<RoomDto> Rooms { get; set; } = new List<RoomDto>();

    /// <summary>
    /// Converts this DTO back into a <see cref="Voucher"/> domain entity.
    /// This method is useful when the client or UI sends updated voucher data and the server needs to persist it as a domain object.
    /// </summary>
    /// <returns>A new <see cref="Voucher"/> object populated with data from this DTO.</returns>
    public Voucher ToVoucher()
    {
        return new Voucher
        {
            Title = this.Title,
            ShortDescription = this.ShortDescription,
            LongDescription = this.LongDescription,
            Rate = this.Rate,
            MarkupPercentage = this.MarkupPercentage,
            Commission = this.Commission,
            Features = this.Features,
            Terms = this.Terms,
            Active = this.Active,
            Featured = this.Featured,
            LodgingId = this.LodgingId
        };
    }
}
