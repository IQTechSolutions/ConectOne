using Microsoft.AspNetCore.Components;

namespace SchoolsModule.Blazor.Pages.Events
{
    /// <summary>
    /// Represents a group of participants associated with an activity and their transport type.
    /// </summary>
    /// <remarks>This class is used to manage attendance-related data for a specific activity group, 
    /// including the group's identifier and the type of transport associated with it.</remarks>
    public partial class AttendanceGroups
    {
        /// <summary>
        /// Gets or sets the identifier of the participating activity group.
        /// </summary>
        [Parameter] public string ParticipatingActivityGroupId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the transport type identifier.
        /// </summary>
        [Parameter] public int TransportType { get; set; }
    }
}
