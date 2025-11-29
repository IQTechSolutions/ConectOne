using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;

namespace ConectOne.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for an email address, including its unique identifier, parent entity identifier,  email
    /// address value, and default status.
    /// </summary>
    /// <remarks>This class is typically used to transfer email address data between the UI and the
    /// application layer.  It provides properties for the email address details and methods for converting to a data
    /// transfer object (DTO).</remarks>
    public class EmailAddressViewModel : ModalModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressViewModel"/> class.
        /// </summary>
        public EmailAddressViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressViewModel"/> class using the specified email
        /// address data transfer object.
        /// </summary>
        /// <remarks>The properties of the <see cref="EmailAddressViewModel"/> are populated based on the
        /// corresponding values in the provided <see cref="EmailAddressDto"/>.</remarks>
        /// <param name="emailAddress">An <see cref="EmailAddressDto"/> containing the email address details to initialize the view model.</param>
        public EmailAddressViewModel(EmailAddressDto emailAddress)
        {
            EmailAddressId= emailAddress.EmailAddressId;
            ParentId= emailAddress.ParentId;
            EmailAddress= emailAddress.EmailAddress;
            Default = emailAddress.Default;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the email address.
        /// </summary>
        public string? EmailAddressId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the entity.
        /// </summary>
        [DisplayName("Email"), EmailAddress, Required] public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the default behavior is enabled.
        /// </summary>
        public bool Default { get; set; }

        #region Methods

        /// <summary>
        /// Converts the current instance of <see cref="EmailAddress"/> to an <see cref="EmailAddressDto"/>.
        /// </summary>
        /// <returns>An <see cref="EmailAddressDto"/> that represents the current instance, including its <see
        /// cref="EmailAddressId"/>, <see cref="ParentId"/>, <see cref="EmailAddress"/>, and <see cref="Default"/>
        /// values.</returns>
        public EmailAddressDto ToDto()
        {
            return new EmailAddressDto
            {
                EmailAddressId= EmailAddressId,
                ParentId= ParentId,
                EmailAddress= EmailAddress,
                Default = Default
            };
        }

        #endregion
    }
}
