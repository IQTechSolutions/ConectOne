using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace SchoolsModule.Domain.Entities
{
    /// <summary>
    /// Represents a team member participating in an activity group.
    /// </summary>
    /// <remarks>This class associates a team member with a specific participating activity group. It includes
    /// references to both the activity group and the team member.</remarks>
    public class ParticipatingActitivityGroupTeamMember : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the identifier of the participating activity group.
        /// </summary>
        [ForeignKey(nameof(ParticipatingActivityGroup))] public string ParticipatingActitivityGroupId { get; set; }

        /// <summary>
        /// Gets or sets the activity group that is participating in the current operation.
        /// </summary>
        public ParticipatingActivityGroup ParticipatingActivityGroup { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated team member.
        /// </summary>
        [ForeignKey(nameof(TeamMember))] public string  TeamMemberId { get; set; }

        /// <summary>
        /// Gets or sets the team member associated with the current context.
        /// </summary>
        public Learner TeamMember { get; set; }
    }
}
