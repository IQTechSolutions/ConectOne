using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Interfaces;

namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents an email address entity.
    /// </summary>
    public class EmailAddress : EntityBase<string>, IDefaultEntity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress"/> class.
        /// </summary>
        public EmailAddress() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress"/> class with the specified email and default status.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="isDefault">A value indicating whether this email address is the default.</param>
        public EmailAddress(string email, bool isDefault)
        {
            Email = email;
            Default = isDefault;
        }

        #endregion

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether this email address is the default.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Email;
        }
    }

    /// <summary>
    /// Represents an email address entity associated with another entity.
    /// </summary>
    /// <typeparam name="T">The type of the associated entity.</typeparam>
    public class EmailAddress<T> : EmailAddress
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress{T}"/> class.
        /// </summary>
        public EmailAddress() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress{T}"/> class with the specified email and default status.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="isDefault">A value indicating whether this email address is the default.</param>
        public EmailAddress(string email, bool isDefault) : base(email, isDefault) { }

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
