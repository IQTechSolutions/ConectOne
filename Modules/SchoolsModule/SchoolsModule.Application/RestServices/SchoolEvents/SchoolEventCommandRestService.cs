using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;

namespace SchoolsModule.Application.RestServices.SchoolEvents
{
    /// <summary>
    /// Provides REST-based operations for managing school events, including creating, updating, and deleting events.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform operations on school events. It uses an
    /// <see cref="IBaseHttpProvider"> to send HTTP requests and handle responses. The service methods return results
    /// wrapped in <see cref="IBaseResult"> to encapsulate the operation's outcome.</remarks>
    /// <param name="provider"></param>
    public class SchoolEventCommandRestService(IBaseHttpProvider provider) : ISchoolEventCommandService
    {
        /// <summary>
        /// Creates a new school event by sending the specified event data to the underlying provider.
        /// </summary>
        /// <remarks>The method sends the provided SchoolEventDto to the underlying provider  for
        /// creation. Ensure that the schoolEvent contains valid data  before calling this method.</remarks>
        /// <param name="schoolEvent">The data for the school event to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} containing
        /// the created SchoolEventDto object.</returns>
        public async Task<IBaseResult<SchoolEventDto>> Create(SchoolEventDto schoolEvent, CancellationToken cancellationToken)
        {
            var result = await provider.PutAsync<SchoolEventDto, SchoolEventDto>("schoolevents", schoolEvent);
            return result;
        }

        /// <summary>
        /// Updates the specified school event asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided school event data to the underlying provider for
        /// updating.  Ensure that the schoolEvent contains valid data before calling this method.</remarks>
        /// <param name="schoolEvent">The school event data to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with
        /// the response of the update operation.</returns>
        public async Task<IBaseResult<EventUpdateResponse>> UpdateAsync(SchoolEventDto schoolEvent, CancellationToken cancellationToken)
        {
            var result = await provider.PostAsync<EventUpdateResponse, SchoolEventDto>("schoolevents", schoolEvent);
            return result;
        }

        /// <summary>
        /// Deletes a school event asynchronously.
        /// </summary>
        /// <remarks>The operation is performed asynchronously and may be canceled by passing a 
        /// CancellationToken. Ensure the schoolEventId is valid  and corresponds to an existing school event.</remarks>
        /// <param name="schoolEventId">The unique identifier of the school event to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult indicating the
        /// outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string schoolEventId, CancellationToken cancellationToken)
        {
            var result = await provider.DeleteAsync("schoolevents", schoolEventId);
            return result;
        }

        /// <summary>
        /// Creates a new ticket type for a school event.
        /// </summary>
        /// <param name="dto">The data transfer object containing the details of the ticket type to create.  This must include all
        /// required fields for the ticket type.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. If the operation is canceled, the task will be terminated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation, including success or failure details.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult> CreateTicketTypeAsync(SchoolEventTicketTypeDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync("schoolevents/ticketTypes", dto);
            return result;
        }

        /// <summary>
        /// Updates the ticket type for a school event asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided ticket type details to the underlying provider for
        /// updating. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the details of the ticket type to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. This allows the operation to be canceled if needed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateTicketTypeAsync(SchoolEventTicketTypeDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("schoolevents/ticketTypes", dto);
            return result;
        }

        /// <summary>
        /// Deletes a ticket type with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the "schoolevents/ticketTypes" endpoint to
        /// remove the specified ticket type. Ensure the provided <paramref name="ticketTypeId"/> corresponds to an
        /// existing ticket type.</remarks>
        /// <param name="ticketTypeId">The unique identifier of the ticket type to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing a canceled token will immediately terminate the
        /// operation.</param>
        /// <returns>An <see cref="IBaseResult"/> representing the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteTicketTypeAsync(string ticketTypeId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("schoolevents/ticketTypes", ticketTypeId);
            return result;
        }
    }
}
