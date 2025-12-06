using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines methods for managing average temperature data, including retrieval, creation, updating, and deletion.
    /// </summary>
    /// <remarks>This service provides functionality to interact with average temperature records, allowing
    /// clients to perform CRUD operations and retrieve data based on specific identifiers or areas. All methods are
    /// asynchronous and support cancellation via <see cref="CancellationToken"/>.</remarks>
    public interface IAverageTemperatureService
    {
        /// <summary>
        /// Retrieves the average temperatures for all available locations within the specified area.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch average temperature data. The
        /// caller can use the <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="areaId">The identifier of the area for which average temperature data is requested. Must not be <see
        /// langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of  <see cref="AverageTemperatureDto"/> objects
        /// representing the average temperatures.</returns>
        Task<IBaseResult<IEnumerable<AverageTemperatureDto>>> AllAverageTempraturesAsync(string areaId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves the average temperature data for the specified identifier.
        /// </summary>
        /// <param name="averageTempId">The unique identifier for the average temperature record to retrieve.  This parameter cannot be null or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// object wrapping an <see cref="AverageTemperatureDto"/>  with the average temperature data.</returns>
        Task<IBaseResult<AverageTemperatureDto>> AverageTempratureAsync(string averageTempId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new average temperature record asynchronously.
        /// </summary>
        /// <param name="model">The data transfer object containing the average temperature details to be created. Must not be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// object wrapping the created <see cref="AverageTemperatureDto"/>.</returns>
        Task<IBaseResult<AverageTemperatureDto>> CreateAverageTempratureAsync(AverageTemperatureDto model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the average temperature data asynchronously.
        /// </summary>
        /// <remarks>Use this method to update the average temperature data in the system. The operation
        /// is performed asynchronously  and supports cancellation via the <paramref name="cancellationToken"/>
        /// parameter.</remarks>
        /// <param name="package">The <see cref="AverageTemperatureDto"/> containing the updated temperature data. This parameter cannot be
        /// null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object wrapping the updated <see cref="AverageTemperatureDto"/>.</returns>
        Task<IBaseResult<AverageTemperatureDto>> UpdateAverageTempratureAsync(AverageTemperatureDto package, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the average temperature record associated with the specified identifier.
        /// </summary>
        /// <remarks>Use this method to remove an average temperature record from the system. Ensure that
        /// the <paramref name="id"/> corresponds to a valid record before calling this method.</remarks>
        /// <param name="id">The unique identifier of the average temperature record to remove. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveAverageTempratureAsync(string id, CancellationToken cancellationToken = default);
    }
}
