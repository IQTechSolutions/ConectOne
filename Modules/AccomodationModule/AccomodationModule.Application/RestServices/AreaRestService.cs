using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing areas, including retrieving, creating, updating, and deleting areas.
    /// </summary>
    /// <remarks>This service acts as a client for interacting with an external API that manages area data. 
    /// It uses an <see cref="IBaseHttpProvider"/> to perform HTTP operations and returns results wrapped in <see
    /// cref="IBaseResult"/>.</remarks>
    /// <param name="provider"></param>
    public class AreaRestService(IBaseHttpProvider provider) : IAreaService
    {
        /// <summary>
        /// Retrieves all available areas asynchronously.
        /// </summary>
        /// <remarks>This method fetches the list of areas from the underlying data provider. If no areas
        /// are available, the result may contain an empty collection.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an <see cref="IEnumerable{T}"/> of <see cref="AreaDto"/> objects representing the available
        /// areas.</returns>
        public async Task<IBaseResult<IEnumerable<AreaDto>>> AllAreasAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AreaDto>>("areas");
            return result;
        }

        /// <summary>
        /// Retrieves the details of an area based on the specified area identifier.
        /// </summary>
        /// <remarks>This method uses the underlying provider to fetch area details. Ensure that the
        /// <paramref name="areaId"/> is valid and corresponds to an existing area in the system. The operation may be
        /// canceled by passing a cancellation token.</remarks>
        /// <param name="areaId">The unique identifier of the area to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the details of the area as an <see cref="AreaDto"/>. If the area is not found, the result may
        /// indicate an error or an empty response, depending on the implementation of the provider.</returns>
        public async Task<IBaseResult<AreaDto>> AreaAsync(string areaId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<AreaDto>($"areas/{areaId}");
            return result;
        }

        /// <summary>
        /// Creates a new area asynchronously.
        /// </summary>
        /// <remarks>This method sends a request to create a new area using the provided <see
        /// cref="AreaDto"/> model. Ensure that the <paramref name="model"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="model">The <see cref="AreaDto"/> object containing the details of the area to be created.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// where <c>T</c> is <see cref="AreaDto"/>. The result includes the created area details if the operation
        /// succeeds.</returns>
        public async Task<IBaseResult<AreaDto>> CreateAreaAsync(AreaDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<AreaDto, AreaDto>("areas", model);
            return result;
        }

        /// <summary>
        /// Updates an existing area with the specified details.
        /// </summary>
        /// <remarks>This method sends the updated area details to the underlying provider for processing.
        /// Ensure that the <paramref name="areaDto"/> contains valid data before calling this method.</remarks>
        /// <param name="areaDto">The data transfer object containing the updated details of the area.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the updated <see cref="AreaDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<AreaDto>> UpdateAreaAsync(AreaDto areaDto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<AreaDto, AreaDto>("areas", areaDto);
            return result;
        }

        /// <summary>
        /// Removes an area with the specified identifier asynchronously.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// area. Ensure that the <paramref name="areaId"/> corresponds to an existing area.</remarks>
        /// <param name="areaId">The unique identifier of the area to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAreaAsync(string areaId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("areas", areaId);
            return result;
        }
    }
}
