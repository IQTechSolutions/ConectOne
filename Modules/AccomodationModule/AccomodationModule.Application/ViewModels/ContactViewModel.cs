using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// Represents the view model for a guide.
/// </summary>
public class ContactViewModel
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactViewModel"/> class.
    /// </summary>
    public ContactViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactViewModel"/> class based on a <see cref="ContactDto"/>.
    /// </summary>
    /// <param name="contact">The <see cref="ContactDto"/> to create the view model from.</param>
    public ContactViewModel(ContactDto contact)
    {
        ContactId = contact.ContactId;
        ProfileImageUrl = contact.ProfileImageUrl;
        Name = contact.Name;
        Surname = contact.Surname;
        Phone = contact.Phone;
        Email = contact.Email;
        Bio = contact.Bio;
        ContactType = contact.ContactType;
        Featured = contact.Featured;
        Order = contact.Order;
        Selector = contact.Selector;
        Images = contact.Images.ToList();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the contact ID.
    /// </summary>
    public string ContactId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the URL of the contact's profile image.
    /// </summary>
    public string? ProfileImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the name of the contact.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the surname of the contact.
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// Gets or sets the phone number of the contact.
    /// </summary>
    public string? Phone { get; set; } 

    /// <summary>
    /// Gets or sets the email address of the contact.
    /// </summary>
    public string? Email { get; set; } 

    /// <summary>
    /// Gets or sets the biography of the contact.
    /// </summary>
    public string? Bio { get; set; } 

    /// <summary>
    /// Gets or sets the type of contact. This property is required.
    /// </summary>
    public ContactType ContactType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is featured.
    /// </summary>
    public bool Featured { get; set; }

    /// <summary>
    /// Gets or sets the selector for the vacation price.
    /// </summary>
    [MaxLength(1000)] public string? Selector { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the order of the vacation price.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the collection of image data transfer objects.
    /// </summary>
    public ICollection<ImageDto> Images { get; set; } = [];

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current contact instance to a <see cref="ContactDto"/> object.
    /// </summary>
    /// <remarks>This method creates a new <see cref="ContactDto"/> instance and populates its properties
    /// based on the current contact's data. If the <c>ContactId</c> is null or empty, a new GUID is generated and used
    /// as the <c>ContactId</c> in the resulting DTO.</remarks>
    /// <returns>A <see cref="ContactDto"/> object containing the data from the current contact instance.</returns>
    public ContactDto ToDto()
    {
        return new ContactDto()
        {
            ContactId = string.IsNullOrEmpty(ContactId) ? Guid.NewGuid().ToString() : ContactId,
            ProfileImageUrl = ProfileImageUrl,
            Name = Name,
            Surname = Surname,
            Phone = Phone,
            Email = Email,
            Bio = Bio,
            ContactType = ContactType,
            Featured = Featured,
            Order = Order,
            Selector = Selector,
        };
    }

    #endregion
}