using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a team member in an Activity Group.
    /// </summary>
    public class ActivityGroupTeamMember : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ActivityGroupTeamMember() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityGroupTeamMember"/> class with the specified Activity Group ID and Learner ID.
        /// </summary>
        /// <param name="activityGroupId">The ID of the Activity Group.</param>
        /// <param name="learnerId">The ID of the Learner.</param>
        public ActivityGroupTeamMember(string activityGroupId, string learnerId)
        {
            this.ActivityGroupId = activityGroupId;
            this.LearnerId = learnerId;
        }

        #endregion

        /// <summary>
        /// Gets or sets the ID of the Activity Group.
        /// </summary>
        [ForeignKey(nameof(ActivityGroup))] public string ActivityGroupId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Activity Group associated with this team member.
        /// </summary>
        public ActivityGroup ActivityGroup { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the Learner.
        /// </summary>
        [ForeignKey(nameof(Learner))] public string LearnerId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Learner associated with this team member.
        /// </summary>
        public Learner Learner { get; set; } = null!;
    }
}