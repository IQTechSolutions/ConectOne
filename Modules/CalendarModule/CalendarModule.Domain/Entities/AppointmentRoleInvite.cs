using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace CalendarModule.Domain.Entities
{
    /// <summary>
    /// Represents the association between an appointment and a role that is invited to it.
    /// </summary>
    public class AppointmentRoleInvite : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the identifier of the appointment.
        /// </summary>
        [Required, ForeignKey(nameof(Appointment))] public string AppointmentId { get; set; } = null!;

        /// <summary>
        /// Navigation property to the related appointment.
        /// </summary>
        public Appointment Appointment { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the invited role.
        /// </summary>
        [Required, ForeignKey(nameof(Role))] public string RoleId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the role associated with the application.
        /// </summary>
        public ApplicationRole Role { get; set; }
    }
}
