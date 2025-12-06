using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing average temperature data, including retrieval, creation, update, and deletion.
    /// </summary>
    /// <remarks>This service acts as a wrapper around an HTTP provider to interact with an external data
    /// source for average temperature records. It supports asynchronous operations and cancellation tokens for all
    /// methods. Ensure that the identifiers and data models passed to the methods are valid and correspond to the
    /// expected format of the underlying data provider.</remarks>
    public class AverageTemperatureRestService(IBaseHttpProvider provider) : IAverageTemperatureService
    {
        /// <summary>
        /// Retrieves the average temperatures for a specified area.
        /// </summary>
        /// <remarks>This method fetches data from an external provider. Ensure that the <paramref
        /// name="areaId"/> corresponds to a valid area recognized by the provider. The operation may be canceled by
        /// passing a cancellation token.</remarks>
        /// <param name="areaId">The identifier of the area for which to retrieve average temperature data. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing a collection of <see cref="AverageTemperatureDto"/> objects representing the average temperatures
        /// for the specified area.</returns>
        public async Task<IBaseResult<IEnumerable<AverageTemperatureDto>>> AllAverageTempraturesAsync(string areaId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<AverageTemperatureDto>>($"averagetemperatures/areas/{areaId}");
            return result;
        }

        /// <summary>
        /// Retrieves the average temperature data for the specified identifier.
        /// </summary>
        /// <remarks>This method fetches the average temperature data from the underlying provider using
        /// the specified identifier. Ensure that the identifier corresponds to a valid record in the data
        /// source.</remarks>
        /// <param name="averageTempId">The unique identifier of the average temperature record to retrieve. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="AverageTemperatureDto"/> for the specified identifier.</returns>
        public async Task<IBaseResult<AverageTemperatureDto>> AverageTempratureAsync(string averageTempId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<AverageTemperatureDto>($"averagetemperatures/{averageTempId}");
            return result;
        }

        /// <summary>
        /// Updates or creates an average temperature record asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided average temperature data to the underlying data
        /// provider for creation or update. The operation is performed asynchronously and supports cancellation through
        /// the <paramref name="cancellationToken"/>.</remarks>
        /// <param name="model">The <see cref="AverageTemperatureDto"/> object containing the data for the average temperature to be created
        /// or updated.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the updated or created <see cref="AverageTemperatureDto"/>.</returns>
        public async Task<IBaseResult<AverageTemperatureDto>> CreateAverageTempratureAsync(AverageTemperatureDto model, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<AverageTemperatureDto, AverageTemperatureDto>($"averagetemperatures", model);
            return result;
        }

        /// <summary>
        /// Updates the average temperature data by sending the specified package to the server.
        /// </summary>
        /// <remarks>This method sends the provided average temperature data to the server for updating.
        /// Ensure that the <paramref name="package"/> parameter contains valid data before calling this method. The
        /// operation may be canceled by passing a cancellation token.</remarks>
        /// <param name="package">The <see cref="AverageTemperatureDto"/> containing the average temperature data to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the updated <see cref="AverageTemperatureDto"/>.</returns>
        public async Task<IBaseResult<AverageTemperatureDto>> UpdateAverageTempratureAsync(AverageTemperatureDto package, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<AverageTemperatureDto, AverageTemperatureDto>($"averagetemperatures", package);
            return result;
        }

        /// <summary>
        /// Deletes the average temperature record with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying data provider to remove the
        /// specified average temperature record. Ensure that the <paramref name="id"/> corresponds to an existing
        /// record.</remarks>
        /// <param name="id">The unique identifier of the average temperature record to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> RemoveAverageTempratureAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"averagetemperatures", id);
            return result;
        }
    }
}
