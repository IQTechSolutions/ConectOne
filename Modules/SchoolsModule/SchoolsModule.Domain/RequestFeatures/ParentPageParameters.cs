using ConectOne.Domain.RequestFeatures;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Provides filtering and paging parameters specific to parent records,
    /// such as linking to a particular learner or searching by parent ID.
    /// Inherits common paging/sorting functionality from <see cref="RequestParameters"/>.
    /// </summary>
    public class ParentPageParameters : RequestParameters
    {
        /// <summary>
        /// An optional learner ID to further filter or link parent data.
        /// </summary>
        public string? LearnerId { get; set; }

        /// <summary>
        /// Optional parent ID for direct parent lookups.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Indicates whether the system should link parents to learners 
        /// or perform some additional relational logic.
        /// </summary>
        public bool LinkParents { get; set; }
    }
}
