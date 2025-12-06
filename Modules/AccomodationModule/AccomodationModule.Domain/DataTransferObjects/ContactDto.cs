using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Extensions;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for Guide.
/// </summary>
public record ContactDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactDto"/> class.
    /// </summary>
    public ContactDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactDto"/> class based on a <see cref="Contact"/> entity.
    /// </summary>
    /// <param name="contact">The <see cref="Contact"/> entity to create the DTO from.</param>
    public ContactDto(Contact contact)
    {
        ContactId = contact.Id;
        ProfileImageUrl = contact.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Profile)?.Image.RelativePath;
        Name = contact.Name;
        Surname = contact.Surname;
        Phone = contact.Phone;
        Email = contact.Email;
        Bio = contact.Bio;
        ContactType = contact.ContactType;
        Featured = contact.Featured;
        Selector = contact.Selector;
        Order = contact.Order;
        Images = contact.Images == null ? [] : contact.Images.Select(c => ImageDto.ToDto(c)).ToList();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the contact ID.
    /// </summary>
    public string ContactId { get; init; } = null!;

    /// <summary>
    /// Gets or sets the URL of the contact's profile image.
    /// </summary>
    public string? ProfileImageUrl { get; init; } = "_content/Accomodation.Blazor/images/profileImage128x128.png";

    /// <summary>
    /// Gets or sets the name of the contact.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets or sets the surname of the contact.
    /// </summary>
    public string Surname { get; init; } = null!;

    public string DisplayName => Name + " " + Surname;

    /// <summary>
    /// Gets or sets the phone number of the contact.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Gets or sets the email address of the contact.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Gets or sets the biography of the contact.
    /// </summary>
    public string? Bio { get; init; } 

    /// <summary>
    /// Gets or sets the type of contact. This property is required.
    /// </summary>
    public ContactType ContactType { get; init; }

    public string ContactTypeDescription => ContactType.GetDescription();

    /// <summary>
    /// Gets or sets a value indicating whether the item is featured.
    /// </summary>
    public bool Featured { get; init; }

    /// <summary>
    /// Gets or sets the selector for the vacation price.
    /// </summary>
    [MaxLength(1000)] public string? Selector { get; set; }

    /// <summary>
    /// Gets or sets the order of the vacation price.
    /// </summary>
    public int Order { get; set; }

    #endregion

    #region Images

    /// <summary>
    /// Gets the collection of images associated with the current entity.
    /// </summary>
    public ICollection<ImageDto> Images { get; init; } = [];

    #endregion

    #region Helpers

    /// <summary>
    /// Converts this DTO to a <see cref="Contact"/> entity.
    /// </summary>
    /// <returns>A new <see cref="Contact"/> entity.</returns>
    public Contact ToGuide()
    {
        return new Contact()
        {
            Id = ContactId,
            Name = Name,
            Surname = Surname,
            Phone = Phone,
            Email = Email,
            Bio = Bio,
            ContactType = ContactType,
            Featured = Featured,
            Order = Order,
            Selector = Selector
        };
    }
    
    #endregion
}
