using System.ComponentModel;

namespace NeuralTech.Base.Enums
{
    /// <summary>
    /// Represents the different types of messages in the system.
    /// </summary>
    public enum MessageType
    {
        #region Global Messages

        /// <summary>
        /// A global message visible to all users.
        /// </summary>
        [Description("Global")]
        Global = 1,

        #endregion

        #region User-Specific Messages

        /// <summary>
        /// A message intended for learners.
        /// </summary>
        [Description("Learner")]
        Learner = 2,

        /// <summary>
        /// A message intended for parents.
        /// </summary>
        [Description("Parent")]
        Parent = 3,

        /// <summary>
        /// A message intended for teachers.
        /// </summary>
        [Description("Teacher")]
        Teacher = 4,

        #endregion

        #region Event and Activity Messages

        /// <summary>
        /// A message about an event.
        /// </summary>
        [Description("Event")]
        Event = 5,

        /// <summary>
        /// A message related to an activity category.
        /// </summary>
        [Description("Activity Category")]
        ActivityCategory = 6,

        /// <summary>
        /// A message related to an activity group.
        /// </summary>
        [Description("Activity Group")]
        ActivityGroup = 7,

        #endregion

        #region Educational Messages

        /// <summary>
        /// A message about grades.
        /// </summary>
        [Description("Grade")]
        Grade = 8,

        /// <summary>
        /// A message about age groups.
        /// </summary>
        [Description("Age Group")]
        AgeGroup = 9,

        /// <summary>
        /// A message about school classes.
        /// </summary>
        [Description("School Class")]
        SchoolClass = 10,

        #endregion

        #region Miscellaneous Messages

        /// <summary>
        /// A message with no specific type.
        /// </summary>
        [Description("Notification")]
        None = 11,

        /// <summary>
        /// A message related to blog posts.
        /// </summary>
        [Description("Blog Post")]
        BlogPost = 12,

        /// <summary>
        /// A message about participating activity group categories.
        /// </summary>
        [Description("Participating Activity Group Category")]
        ParticipatingActivityGroupCategory = 13,

        /// <summary>
        /// A message about participating activity groups.
        /// </summary>
        [Description("Participating Activity Group")]
        ParticipatingActivityGroup = 14,

        /// <summary>
        /// A message about participating team members.
        /// </summary>
        [Description("Participating Team Member")]
        ParticipatingTeamMember = 15,

        #endregion

        /// <summary>
        /// Represents a team member role within the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify users who are designated as team
        /// members.</remarks>
        [Description("Team Member")]
        TeamMember = 16,

        /// <summary>
        /// Represents a chat message in the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify a chat message type.  It can be utilized
        /// in scenarios where message categorization or filtering is required.</remarks>
        [Description("Chat Message")]
        ChatMessage = 17,

        /// <summary>
        /// Represents a user role message in the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify a chat message type.  It can be utilized
        /// in scenarios where message categorization or filtering is required.</remarks>
        [Description("User Role Message")]
        RoleMessage = 18
    }
}
