using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.RequestFeatures;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IVacationHostService"/> interface for managing vacation
    /// hosts.
    /// </summary>
    /// <remarks>This service interacts with a RESTful API to perform operations related to vacation hosts,
    /// such as retrieving, creating, updating, and deleting vacation host data. It uses an <see
    /// cref="IBaseHttpProvider"/> to handle HTTP requests and responses.</remarks>
    /// <param name="provider"></param>
    public class VacationHostRestService(IBaseHttpProvider provider) : IVacationHostService
    {
        /// <summary>
        /// Retrieves a paginated list of vacation hosts based on the specified request parameters.
        /// </summary>
        /// <remarks>The method fetches data from the "vacationHosts" endpoint and applies the specified
        /// pagination and filtering options.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of vacation hosts.</returns>
        public async Task<PaginatedResult<VacationHostDto>> PagedVacationHostsAsync(RequestParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<VacationHostDto, RequestParameters>("vacationHosts", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of all vacation hosts.
        /// </summary>
        /// <remarks>This method fetches all vacation hosts from the underlying data provider. The result
        /// includes metadata and the collection of vacation hosts. If no vacation hosts are available, the collection
        /// will be empty.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with an <see cref="IEnumerable{T}"/> of <see cref="VacationHostDto"/> representing the vacation hosts.</returns>
        public async Task<IBaseResult<IEnumerable<VacationHostDto>>> AllVacationHostsAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<VacationHostDto>>("vacationHosts/all");
            return result;
        }

        /// <summary>
        /// Retrieves vacation host details asynchronously based on the specified vacation host ID.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch vacation host details. Ensure that
        /// the provided <paramref name="vacationHostId"/> corresponds to a valid vacation host.</remarks>
        /// <param name="vacationHostId">The unique identifier of the vacation host to retrieve. This value cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that wraps the <see cref="VacationHostDto"/> containing the vacation host details.</returns>
        public async Task<IBaseResult<VacationHostDto>> VacationHostAsync(string vacationHostId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationHostDto>($"vacationHosts/{vacationHostId}");
            return result;
        }

        /// <summary>
        /// Retrieves a vacation host by its name.
        /// </summary>
        /// <param name="vacationHostName">The name of the vacation host to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the <see cref="VacationHostDto"/> for the specified vacation host, or an empty result if the
        /// host is not found.</returns>
        public async Task<IBaseResult<VacationHostDto>> VacationHostFromNameAsync(string vacationHostName, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<VacationHostDto>($"vacationHosts/fromName/{vacationHostName}");
            return result;
        }

        /// <summary>
        /// Creates or updates a vacation host asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided vacation host data to the underlying provider for
        /// creation or update. Ensure that the <paramref name="dto"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the vacation host details to be created or updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// representing the outcome of the operation.</returns>
        public async Task<IBaseResult> CreateAsync(VacationHostDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync($"vacationHosts", dto);
            return result;
        }

        /// <summary>
        /// Updates the vacation host information asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated vacation host data to the underlying provider. Ensure
        /// that the <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated vacation host information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(VacationHostDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"vacationHosts", dto);
            return result;
        }

        /// <summary>
        /// Removes the specified resource asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the resource
        /// identified by the specified <paramref name="id"/>.</remarks>
        /// <param name="id">The unique identifier of the resource to remove. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"vacationHosts", id);
            return result;
        }

        /// <summary>
        /// Adds an image to the entity as specified in the request.
        /// </summary>
        /// <param name="request">The request containing the details of the image to add. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the add image operation.</returns>
        /// <exception cref="NotImplementedException">The method is not implemented.</exception>
        public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("vacationHosts", request);
            return result;
        }

        /// <summary>
        /// Removes the image with the specified identifier from the data store asynchronously.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous remove operation. The task result contains an object indicating the
        /// outcome of the operation.</returns>
        /// <exception cref="NotImplementedException">The method is not implemented.</exception>
        public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("vacationHosts", imageId);
            return result;
        }
    }
}
