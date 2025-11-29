using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace BusinessModule.Domain.Entities
{
    /// <summary>
    /// Represents a company entity, including its details, relationships, and associated collections.
    /// </summary>
    /// <remarks>The <see cref="Company"/> class provides properties to store information about a company,
    /// such as its name,  registration details, and employee/license counts. It also defines relationships to other
    /// entities, such as  the company's address, owner, teams, industries, and associated documents.</remarks>
    public class Company : FileCollection<Company, string>
    {
        #region Public Members

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        [Required, MaxLength(255, ErrorMessage = "Company name can only have a maximum ammount or 255 characters")] public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name of the entity.
        /// </summary>
        [Required, MaxLength(255, ErrorMessage = "Display name can only have a maximum ammount or 255 characters")] public string DisplayName { get; set; }
        public string? RegistrationNr { get; set; }

        /// <summary>
        /// Gets or sets the VAT (Value-Added Tax) number associated with the entity.
        /// </summary>
        public string? VatNr { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the unique URL associated with the entity.
        /// </summary>
        public string? WebsiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the number of employees in the organization.
        /// </summary>
        public int NumberOfEmployees { get; set; }

        /// <summary>
        /// Gets or sets the number of licenses associated with the current instance.
        /// </summary>
        public int NumberOfLicenses { get; set; }

        /// <summary>
        /// Gets or sets the number of licenses currently in use.
        /// </summary>
        public int UsedLicenses { get; set; }

        /// <summary>
        /// Gets or sets the number of times the associated content has been viewed.
        /// </summary>
        public int ViewCount { get; set; }

        #endregion
        
        #region One-One Relationships

        /// <summary>
        /// Gets or sets the address associated with this entity.
        /// </summary>
        public Address Address { get; set; }

        #endregion
        
        #region One-Many Relationships

        /// <summary>
        /// Gets or sets the unique identifier of the owner associated with this entity.
        /// </summary>
        /// <remarks>The <see cref="OwnerId"/> property is required to associate this entity with an
        /// owner. Ensure that the value corresponds to a valid owner in the related data store.</remarks>
        [ForeignKey(nameof(Owner))] public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the owner of the application resource.
        /// </summary>
        public ApplicationUser Owner { get; set; }

        #endregion


        #region Collections

        /// <summary>
        /// Gets or sets the collection of industries associated with the company.
        /// </summary>
        public virtual ICollection<EntityCategory<Company>> Industries { get; set; } = new List<EntityCategory<Company>>();
        
        #endregion

    }
}