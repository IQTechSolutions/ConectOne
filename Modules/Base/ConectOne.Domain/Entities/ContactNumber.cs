using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Interfaces;

namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents a contact number entity.
    /// </summary>
    public class ContactNumber : EntityBase<string>, IDefaultEntity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumber"/> class.
        /// </summary>
        public ContactNumber() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumber"/> class with the specified number and default status.
        /// </summary>
        /// <param name="number">The contact number.</param>
        /// <param name="isDefault">A value indicating whether this contact number is the default.</param>
        public ContactNumber(string number, bool isDefault)
        {
            Number = number;
            Default = isDefault;
        }

        #endregion

        /// <summary>
        /// Gets or sets the name associated with the contact number.
        /// </summary>
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the international code for the contact number.
        /// </summary>
        [MaxLength(5, ErrorMessage = "Maximum length for the InternationalCode is 5 characters.")]
        public string? InternationalCode { get; set; } = "+27";

        /// <summary>
        /// Gets or sets the area code for the contact number.
        /// </summary>
        [MaxLength(5, ErrorMessage = "Maximum length for the Area Code is 5 characters.")]
        public string? AreaCode { get; set; }

        /// <summary>
        /// Gets or sets the contact number.
        /// </summary>
        [Required, MaxLength(20, ErrorMessage = "Maximum length for the Number is 20 characters.")]
        public string Number { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether this contact number is the default.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "Contact Number";
        }
    }

    /// <summary>
    /// Represents a contact number entity associated with another entity.
    /// </summary>
    /// <typeparam name="T">The type of the associated entity.</typeparam>
    public class ContactNumber<T> : ContactNumber
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumber{T}"/> class.
        /// </summary>
        public ContactNumber() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumber{T}"/> class with the specified number and default status.
        /// </summary>
        /// <param name="number">The contact number.</param>
        /// <param name="isDefault">A value indicating whether this contact number is the default.</param>
        public ContactNumber(string number, bool isDefault) : base(number, isDefault) { }

        #endregion

        /// <summary>
        /// Gets or sets the ID of the associated entity.
        /// </summary>
        [ForeignKey(nameof(Entity))]
        public string EntityId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the associated entity.
        /// </summary>
        public T Entity { get; set; } = default!;
    }
}
