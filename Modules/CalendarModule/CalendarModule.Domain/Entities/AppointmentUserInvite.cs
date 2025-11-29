using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace CalendarModule.Domain.Entities
{
    /// <summary>
    /// Represents a link between an appointment and a user that has been invited to it.
    /// </summary>
    public class AppointmentUserInvite : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the identifier of the appointment.
        /// </summary>
        [Required, ForeignKey(nameof(Appointment))]
        public string AppointmentId { get; set; } = null!;

        /// <summary>
        /// Navigation property to the related appointment instance.
        /// </summary>
        public Appointment Appointment { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the invited user.
        /// </summary>
        [Required, ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the application user associated with the current context.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
    }
}
