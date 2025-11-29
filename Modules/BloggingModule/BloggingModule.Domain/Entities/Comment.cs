using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace BloggingModule.Domain.Entities
{
    /// <summary>
    /// Represents a comment entity that can be associated with a specific entity, user, or parent comment.
    /// </summary>
    /// <remarks>This class supports hierarchical relationships, allowing comments to have parent comments and
    /// child comments. It also includes user information and metadata such as approval status and content
    /// validation.</remarks>
    /// <typeparam name="T">The type of the associated entity that the comment is related to.</typeparam>
    public class Comment<T> : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the individual.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the individual.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the entity.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        public string? PhoneNr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is approved.
        /// </summary>
        public bool Approved { get; set; } = false;

        /// <summary>
        /// Gets or sets the subject associated with the entity.
        /// </summary>
        [Required] public string Subject { get; set; } = null!;

        /// <summary>
        /// Gets or sets the content of the entity.
        /// </summary>
        [Required(ErrorMessage = "Content is required"), DataType(DataType.MultilineText)] public string Content { get; set; } = null!;

        #region Relationships

        /// <summary>
        /// Gets or sets the identifier of the related entity.
        /// </summary>
        [ForeignKey(nameof(Entity))] public string? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity associated with the current operation.
        /// </summary>
        public T? Entity { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent comment.
        /// </summary>
        [ForeignKey(nameof(ParentComment))] public string? ParentCommentId { get; set; }

        /// <summary>
        /// Gets or sets the parent comment associated with this comment.
        /// </summary>
        public Comment<T>? ParentComment { get; set; }

        #endregion

        #region Collections   

        /// <summary>
        /// Gets or sets the collection of comments associated with the entity.
        /// </summary>
        public virtual ICollection<Comment<T>> Comments { get; set; } = [];

        #endregion
    }
}
