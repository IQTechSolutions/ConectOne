using ConectOne.Domain.Entities;
using ConectOne.Domain.Interfaces;

namespace ConectOne.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for an email address, including its identifier,  associated parent
    /// entity, and whether it is marked as the default email address.
    /// </summary>
    /// <remarks>This DTO is designed to facilitate the transfer of email address data between different 
    /// application layers. It supports initialization from various sources, such as domain models  or view models, and
    /// provides a static factory method for creating instances from generic  email address entities.</remarks>
    public record EmailAddressDto : IDefaultEntity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressDto"/> class.
        /// </summary>
        public EmailAddressDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressDto"/> class using the specified email address.
        /// </summary>
        /// <param name="emailAddress">The <see cref="EmailAddress"/> object to initialize the DTO from. If <paramref name="emailAddress"/> is <see
        /// langword="null"/>, the properties of the DTO will remain uninitialized.</param>
        public EmailAddressDto(EmailAddress? emailAddress)
        {
            if(emailAddress is null) return;
            EmailAddressId = emailAddress.Id;
            EmailAddress = emailAddress.Email;
            Default = emailAddress.Default;
        }

        #endregion

        /// <summary>
        /// Gets the unique identifier for the email address.
        /// </summary>
        public string? EmailAddressId { get; init; }

        /// <summary>
        /// Gets the identifier of the parent entity, if one exists.
        /// </summary>
        public string? ParentId { get; init; }

        /// <summary>
        /// Gets the email address associated with the entity.
        /// </summary>
        public string? EmailAddress { get; init; } 

        /// <summary>
        /// Gets a value indicating whether the default configuration is enabled.
        /// </summary>
        public bool Default { get; init; }

        /// <summary>
        /// Creates a new <see cref="EmailAddressDto"/> instance from the specified <see cref="EmailAddress{T}"/>
        /// object.
        /// </summary>
        /// <typeparam name="T">The type of the entity associated with the email address.</typeparam>
        /// <param name="emailAddress">The <see cref="EmailAddress{T}"/> object to convert. Cannot be <see langword="null"/>.</param>
        /// <returns>A new <see cref="EmailAddressDto"/> containing the data from the specified <see cref="EmailAddress{T}"/>.</returns>
        public static EmailAddressDto CreateDto<T>(EmailAddress<T> emailAddress)
        {
            return new EmailAddressDto()
            {

                EmailAddressId = emailAddress.Id,
                EmailAddress = emailAddress.Email,
                Default = emailAddress.Default,
                ParentId = emailAddress.EntityId
            };
        }
    }
}
