using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using FilingModule.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a teacher with associated personal and professional details,  including name, title, address, and
    /// relationships to grades, classes,  contact numbers, and email addresses.
    /// </summary>
    /// <remarks>This class models a teacher entity with various relationships, such as: - One-to-one
    /// relationship with an <see cref="Address{T}"/>. - One-to-many relationships with <see cref="SchoolGrade"/> and
    /// <see cref="SchoolClass"/>. - Many-to-one relationships with collections of <see cref="ContactNumber"/> and
    /// <see cref="EmailAddress{T}"/>.</remarks>
    public class Teacher : FileCollection<Teacher, string>
    {
        /// <summary>
        /// Gets or sets the title associated with the entity.
        /// </summary>
        public Title Title { get; set; } = Title.Me;

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the surname of the individual.
        /// </summary>
        public string Surname { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the parent receives notifications.
        /// </summary>
        public bool ReceiveNotifications { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the parent receives messages.
        /// </summary>
        public bool ReceiveMessages { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the parent receives emails.
        /// </summary>
        public bool RecieveEmails { get; set; } = true;


        #region One to One Relationships

        /// <summary>
        /// Gets or sets the address associated with the teacher.
        /// </summary>
        public Address<Teacher>? Address { get; set; } = null!;

        #endregion

        #region One to Many Relationships

        /// <summary>
        /// Gets or sets the identifier for the associated grade.
        /// </summary>
        [ForeignKey(nameof(Grade))] public string? GradeId { get; set; }

        /// <summary>
        /// Gets or sets the grade level of the school, if available.
        /// </summary>
        public SchoolGrade? Grade { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated school class.
        /// </summary>
        [ForeignKey(nameof(SchoolClass))] public string? SchoolClassId { get; set; }

        /// <summary>
        /// Gets or sets the school class associated with the current entity.
        /// </summary>
        public SchoolClass? SchoolClass { get; set; }

        #endregion

        #region Many to One Relationships

        /// <summary>
        /// Gets or sets the collection of contact numbers associated with the teacher.
        /// </summary>
        public virtual ICollection<ContactNumber<Teacher>> ContactNumbers { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of email addresses associated with the teacher.
        /// </summary>
        public virtual ICollection<EmailAddress<Teacher>> EmailAddresses { get; set; } = [];

        #endregion
    }
}
