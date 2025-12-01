using CalendarModule.Domain.DataTransferObjects;
using CalendarModule.Domain.Interfaces;
using CalendarModule.Domain.RequestFeatures;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace CalendarModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing appointments, including retrieving, adding, editing, and deleting
    /// appointment data.
    /// </summary>
    /// <remarks>This service implements the <see cref="IAppointmentService"/> interface and provides methods
    /// for interacting with appointment data. The methods support asynchronous operations and allow for flexible
    /// mapping between domain entities and DTOs.</remarks>
    public class AppointmentRestService(IBaseHttpProvider provider) : IAppointmentService
    {
        /// <summary>
        /// Retrieves all calendar entries asynchronously.
        /// </summary>
        /// <remarks>This method fetches calendar entries from the underlying data provider. The returned
        /// result encapsulates the data and any associated metadata or status information.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="CalendarEntryDto"/>
        /// containing a list of <see cref="IBaseResult"/> objects representing the calendar entries.</returns>
        public async Task<IBaseResult<List<CalendarEntryDto>>> GetAllAsync(CalendarPageParameters requestParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<CalendarEntryDto>>($"calendar?{requestParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves a calendar entry by its unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch a calendar entry from the
        /// underlying data provider. Ensure that the <paramref name="id"/> parameter is valid and corresponds to an
        /// existing calendar entry.</remarks>
        /// <param name="id">The unique identifier of the calendar entry to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="CalendarEntryDto"/> for the specified identifier, or an empty result if the
        /// entry is not found.</returns>
        public async Task<IBaseResult<CalendarEntryDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<CalendarEntryDto>($"calendar/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new calendar entry asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided calendar entry to the underlying data provider for
        /// addition. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object representing the calendar entry to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the added <see cref="CalendarEntryDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<CalendarEntryDto>> AddAsync(CalendarEntryDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<CalendarEntryDto, CalendarEntryDto>($"calendar", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing calendar entry asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated calendar entry details to the underlying provider for
        /// processing. Ensure that the <paramref name="dto"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the calendar entry.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the updated <see cref="CalendarEntryDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<CalendarEntryDto>> EditAsync(CalendarEntryDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<CalendarEntryDto, CalendarEntryDto>($"calendar", dto);
            return result;
        }

        /// <summary>
        /// Deletes a calendar entry asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// calendar entry.  Ensure the <paramref name="id"/> corresponds to an existing entry; otherwise, the operation
        /// may fail.</remarks>
        /// <param name="id">The unique identifier of the calendar entry to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"calendar", id);
            return result;
        }
    }
}
