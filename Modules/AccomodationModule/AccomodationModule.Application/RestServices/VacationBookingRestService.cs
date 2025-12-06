using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Domain.RequestFeatures;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IVacationBookingService"/> interface for managing
    /// vacation bookings.
    /// </summary>
    /// <remarks>This service interacts with a RESTful API to perform operations such as retrieving, adding,
    /// updating, and deleting vacation bookings. It supports asynchronous operations and provides methods for paginated
    /// queries, retrieving all bookings, and managing individual bookings by their unique identifiers. Use this service
    /// to integrate vacation booking functionality into your application.</remarks>
    /// <param name="provider"></param>
    public class VacationBookingRestService(IBaseHttpProvider provider) : IVacationBookingService
    {
        /// <summary>
        /// Retrieves a paginated list of vacation bookings based on the specified parameters.
        /// </summary>
        /// <remarks>This method queries the data source for vacation bookings and returns the results in
        /// a paginated format. Use the <paramref name="parameters"/> argument to specify the page size, page number,
        /// and any filtering criteria.</remarks>
        /// <param name="parameters">The pagination and filtering parameters used to retrieve the vacation bookings.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of <see cref="VacationBookingDto"/>
        /// objects.</returns>
        public async Task<PaginatedResult<VacationBookingDto>> PagedAsync(VacationBookingPageParams parameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<VacationBookingDto, VacationBookingPageParams>("vacationBookings", parameters);
            return result;
        }

        /// <summary>
        /// Marks the specified vacation booking as active.
        /// </summary>
        /// <remarks>This method sends a request to mark the specified vacation booking as active. Ensure
        /// that the  <paramref name="vacationBookingId"/> corresponds to a valid booking. The operation is asynchronous
        /// and can be canceled using the provided <paramref name="cancellationToken"/>.</remarks>
        /// <param name="vacationBookingId">The unique identifier of the vacation booking to mark as active. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> MarkAsActiveAsync(string vacationBookingId, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacationBookings/markAsActive/{vacationBookingId}");
            return result;
        }

        /// <summary>
        /// Retrieves all vacation bookings asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to retrieve all vacation bookings and returns the result
        /// as a strongly-typed list. If no vacation bookings are available, the result may contain an empty
        /// list.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing a list of <see cref="VacationBookingDto"/> objects representing the vacation bookings.</returns>
        public async Task<IBaseResult<List<VacationBookingDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<VacationBookingDto>>("vacationBookings/all");
            return result;
        }

        /// <summary>
        /// Retrieves a vacation booking by its unique identifier.
        /// </summary>
        /// <remarks>This method retrieves the vacation booking details from the underlying data provider.
        /// If the specified identifier does not exist, the result may indicate a failure or an empty response,
        /// depending on the implementation of the data provider.</remarks>
        /// <param name="id">The unique identifier of the vacation booking to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="VacationBookingDto"/> corresponding to the specified identifier.</returns>
        public async Task<IBaseResult<VacationBookingDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationBookingDto>($"vacationBookings/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new vacation booking asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided vacation booking data to the underlying provider for
        /// storage. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object representing the vacation booking to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the added <see cref="VacationBookingDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<VacationBookingDto>> AddAsync(VacationBookingDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<VacationBookingDto, VacationBookingDto>($"vacationBookings", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing vacation booking with the specified details.
        /// </summary>
        /// <remarks>This method sends the updated vacation booking details to the underlying provider for
        /// processing. Ensure that the <paramref name="dto"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the updated vacation booking details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// object with the updated <see cref="VacationBookingDto"/> details.</returns>
        public async Task<IBaseResult<VacationBookingDto>> EditAsync(VacationBookingDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<VacationBookingDto, VacationBookingDto>($"vacationBookings", dto);
            return result; 
        }

        /// <summary>
        /// Deletes a vacation booking with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// vacation booking. Ensure the <paramref name="id"/> corresponds to an existing booking to avoid unexpected
        /// results.</remarks>
        /// <param name="id">The unique identifier of the vacation booking to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacationBookings", id);
            return result; 
        }
    }
}
