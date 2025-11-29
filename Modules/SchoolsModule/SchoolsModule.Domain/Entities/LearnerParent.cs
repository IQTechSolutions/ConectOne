using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents the relationship between a learner and a parent, including optional metadata such as consent
    /// requirements.
    /// </summary>
    /// <remarks>This class associates a learner with a parent entity, providing navigation properties to
    /// access the related entities. It also includes a flag to indicate whether parental consent is required for
    /// certain actions.</remarks>
    public class LearnerParent : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerParent"/> class.
        /// </summary>
        public LearnerParent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerParent"/> class with the specified learner and parent
        /// identifiers.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner. This value can be <see langword="null"/> if not specified.</param>
        /// <param name="parentId">The unique identifier of the parent. This value can be <see langword="null"/> if not specified.</param>
        public LearnerParent(string? learnerId, string? parentId)
        {
            LearnerId=learnerId;
            ParentId=parentId;
        }

        #endregion

        /// <summary>
        /// Gets or sets the identifier of the learner associated with this entity.
        /// </summary>
        [ForeignKey(nameof(Learner))] public string? LearnerId { get; set; }

        /// <summary>
        /// Gets or sets the learner associated with the current context.
        /// </summary>
        public Learner? Learner { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        [ForeignKey(nameof(Parent))] public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent object associated with the current instance.
        /// </summary>
        public Parent? Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether parental consent is required.
        /// </summary>
        public bool ParentConsentRequired { get; set; }
    }
}
