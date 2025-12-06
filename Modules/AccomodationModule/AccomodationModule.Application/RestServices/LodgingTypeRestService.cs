using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing lodging types through a RESTful API.
    /// </summary>
    /// <remarks>This service allows clients to perform CRUD operations on lodging types, including retrieving
    /// all lodging types, fetching a specific lodging type by ID, creating new lodging types, updating existing ones,
    /// and removing lodging types. The service communicates with the underlying REST API using the provided <see
    /// cref="IBaseHttpProvider"/>.</remarks>
    /// <param name="provider"></param>
    public class LodgingTypeRestService(IBaseHttpProvider provider) : ILodgingTypeService
    {
        /// <summary>
        /// Retrieves all lodging types asynchronously.
        /// </summary>
        /// <remarks>This method fetches lodging type data from the underlying provider. The returned
        /// result may include an empty collection if no lodging types are available.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="LodgingTypeDto"/> objects.</returns>
        public async Task<IBaseResult<IEnumerable<LodgingTypeDto>>> AllLodgingTypesAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<LodgingTypeDto>>("lodgingTypes");
            return result;
        }

        /// <summary>
        /// Retrieves lodging type details based on the specified lodging type identifier.
        /// </summary>
        /// <remarks>This method fetches lodging type information from the underlying data provider.
        /// Ensure that the <paramref name="lodgingTypeId"/> corresponds to a valid lodging type.</remarks>
        /// <param name="lodgingTypeId">The unique identifier of the lodging type to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the lodging type details as a <see cref="LodgingTypeDto"/>.</returns>
        public async Task<IBaseResult<LodgingTypeDto>> LodgingTypeAsync(string lodgingTypeId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<LodgingTypeDto>($"lodgingTypes/{lodgingTypeId}");
            return result;
        }

        /// <summary>
        /// Creates a new lodging type asynchronously.
        /// </summary>
        /// <param name="lodgingType">The lodging type data to be created. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the created lodging type data.</returns>
        public async Task<IBaseResult<LodgingTypeDto>> CreateLodgingTypeAsync(LodgingTypeDto lodgingType, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<LodgingTypeDto, LodgingTypeDto>($"lodgingTypes", lodgingType);
            return result;
        }

        /// <summary>
        /// Updates the lodging type information asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated lodging type data to the underlying provider. Ensure
        /// that the  <paramref name="lodgingType"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="lodgingType">The lodging type data to be updated. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateLodgingTypeAsync(LodgingTypeDto lodgingType, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"lodgingTypes", lodgingType);
            return result;
        }

        /// <summary>
        /// Removes a lodging type identified by the specified ID.
        /// </summary>
        /// <remarks>This method sends a request to delete the specified lodging type. Ensure the provided
        /// ID corresponds to an  existing lodging type. The operation respects the provided <paramref
        /// name="cancellationToken"/>.</remarks>
        /// <param name="lodgingTypeId">The unique identifier of the lodging type to be removed. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> RemoveLodgingTypeAsync(string lodgingTypeId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"lodgingTypes", lodgingTypeId);
            return result;
        }
    }
}
