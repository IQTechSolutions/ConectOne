using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;

namespace ConectOne.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a contact number, providing properties to manage and display contact number details
    /// such as international code, area code, and the number itself.
    /// </summary>
    /// <remarks>This class is typically used to transfer contact number data between the UI and the
    /// application logic. It includes properties for the contact number's unique identifier, parent entity identifier,
    /// and whether the contact number is the default for the associated entity.</remarks>
    public class ContactNumberViewModel : ModalModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumberViewModel"/> class.
        /// </summary>
        public ContactNumberViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumberViewModel"/> class using the specified contact
        /// number data transfer object.
        /// </summary>
        /// <param name="contactNumber">The data transfer object containing contact number details. Cannot be <see langword="null"/>.</param>
        public ContactNumberViewModel(ContactNumberDto contactNumber)
        {
            ContactNrId = contactNumber.ContactNumberId;
            ParentId = contactNumber.ParentId;
            InternationalCode = contactNumber.InternationalCode;
            AreaCode = contactNumber.AreaCode;
            Number = contactNumber.Number;
            Default = contactNumber.Default;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the contact number.
        /// </summary>
        public string? ContactNrId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the international code associated with the entity.
        /// </summary>
        public string? InternationalCode { get; set; }

        /// <summary>
        /// Gets or sets the area code associated with a phone number.
        /// </summary>
        public string? AreaCode { get; set; }

        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        public string Number { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the default behavior is enabled.
        /// </summary>
        public bool Default { get; set; }

        #region Methods

        /// <summary>
        /// Converts the current contact number entity to a <see cref="ContactNumberDto"/>.
        /// </summary>
        /// <returns>A <see cref="ContactNumberDto"/> representing the current contact number, including its ID, international
        /// code, area code, number, and default status.</returns>
        public ContactNumberDto ToDto()
        {
            return new ContactNumberDto
            {
                ContactNumberId = ContactNrId,
                InternationalCode = InternationalCode,
                AreaCode = AreaCode,
                Number = Number,
                ParentId = ParentId,
                Default = Default
            };
        }

        #endregion
    }
}
