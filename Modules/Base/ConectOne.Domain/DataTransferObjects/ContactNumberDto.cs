// Ignore Spelling: Dto

using ConectOne.Domain.Entities;
using ConectOne.Domain.Interfaces;

namespace ConectOne.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a contact number, encapsulating details such as the  international
    /// code, area code, and the phone number itself.
    /// </summary>
    /// <remarks>This DTO is designed to facilitate the transfer of contact number information between
    /// different  layers of the application. It includes properties for identifying the contact number, its 
    /// hierarchical relationship (via <see cref="ParentId"/>), and whether it is the default contact  number.</remarks>
    public record ContactNumberDto : IDefaultEntity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumberDto"/> class.
        /// </summary>
        public ContactNumberDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumberDto"/> class using the specified contact number.
        /// </summary>
        /// <param name="contactNumber">The contact number to initialize the DTO with. If <see langword="null"/>, the DTO will remain uninitialized.</param>
        public ContactNumberDto(ContactNumber? contactNumber)
        {
            if (contactNumber is null) return;


            ContactNumberId = contactNumber.Id;

            Name = contactNumber.Name;
            InternationalCode = contactNumber.InternationalCode;
            AreaCode = contactNumber.AreaCode;
            Number = contactNumber.Number;
            Default = contactNumber.Default;

        }

        #endregion

        /// <summary>
        /// Gets the unique identifier for the contact number.
        /// </summary>
        public string? ContactNumberId { get; init; } 

        /// <summary>
        /// Gets the identifier of the parent entity, if one exists.
        /// </summary>
        public string? ParentId { get; init; }

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string? Name { get; init; }

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
        /// Gets a value indicating whether the default configuration is enabled.
        /// </summary>
        public bool Default { get; init; }

        /// <summary>
        /// Creates a new ContactNumber<TEntity> instance using the current number and specified default status.
        /// </summary>
        /// <typeparam name="TEntity">The type of the associated entity, which must implement IAuditableEntity<string>.</typeparam>
        /// <param name="defaultEntry">true to mark the contact number as the default entry; otherwise, false.</param>
        /// <returns>A new ContactNumber<TEntity> initialized with the current number and the specified default status.</returns>
        public ContactNumber<TEntity> ToContactNumber<TEntity>(bool defaultEntry) where TEntity : IAuditableEntity<string>
        {
            return new ContactNumber<TEntity>()
            {
                Name = "",
                InternationalCode = "",
                AreaCode = "",
                Number = this.Number,
                Default = defaultEntry,
                EntityId = ParentId
            };
        }
    }
}
