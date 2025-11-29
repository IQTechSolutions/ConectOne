using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Domain.Interfaces.SchoolEvents
{
    /// <summary>
    /// Interface defining command operations for managing <see cref="SchoolEventDto"/> entities,
    /// including creation, updating, and deletion.
    /// </summary>
    public interface ISchoolEventCommandService
    {
        /// <summary>
        /// Creates a new school event based on the provided event data.
        /// </summary>
        /// <param name="schoolEvent">The event data to be created.</param>
        /// <returns>
        /// A result containing the created <see cref="SchoolEventDto"/>, or an error message if creation fails.
        /// </returns>
        Task<IBaseResult<SchoolEventDto>> Create(SchoolEventDto schoolEvent, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing school event.
        /// </summary>
        /// <param name="schoolEvent">The updated event data.</param>
        /// <returns>
        /// A result containing update metadata or error messages if the update fails.
        /// </returns>
        Task<IBaseResult<EventUpdateResponse>> UpdateAsync(SchoolEventDto schoolEvent, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a school event by its unique identifier.
        /// </summary>
        /// <param name="schoolEventId">The ID of the event to delete.</param>
        /// <returns>A result indicating success or failure of the deletion operation.</returns>
        Task<IBaseResult> DeleteAsync(string schoolEventId, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new ticket type for a school event asynchronously.
        /// </summary>
        /// <remarks>The <paramref name="dto"/> parameter must include all required fields for creating a
        /// ticket type.  Ensure that the provided data adheres to any validation rules defined for the ticket
        /// type.</remarks>
        /// <param name="dto">The data transfer object containing the details of the ticket type to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        Task<IBaseResult> CreateTicketTypeAsync(SchoolEventTicketTypeDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the ticket type for a school event based on the provided data transfer object (DTO).
        /// </summary>
        /// <remarks>Ensure that the provided <paramref name="dto"/> contains valid and complete data  for
        /// the ticket type update. The operation may fail if the data is invalid or if  the ticket type does not
        /// exist.</remarks>
        /// <param name="dto">The data transfer object containing the updated ticket type information.  This must include all required
        /// fields for the update operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation, including  success or failure details.</returns>
        Task<IBaseResult> UpdateTicketTypeAsync(SchoolEventTicketTypeDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a ticket type identified by the specified ID.
        /// </summary>
        /// <remarks>Use this method to remove a ticket type from the system. Ensure that the specified
        /// ticket type ID exists  before calling this method to avoid unnecessary operations.</remarks>
        /// <param name="ticketTypeId">The unique identifier of the ticket type to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> DeleteTicketTypeAsync(string ticketTypeId, CancellationToken cancellationToken = default);
    }

}
