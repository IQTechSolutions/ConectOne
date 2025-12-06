using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing itinerary item templates.
    /// </summary>
    /// <remarks>This service allows clients to perform CRUD operations on itinerary item templates, including
    /// retrieving all templates, fetching a specific template by ID, adding new templates, editing existing templates,
    /// and deleting templates.</remarks>
    /// <param name="provider"></param>
    public class ItineraryItemTemplateRestService(IBaseHttpProvider provider) : IItineraryItemTemplateService
    {
        /// <summary>
        /// Retrieves all itinerary entry item templates asynchronously.
        /// </summary>
        /// <remarks>This method fetches all available itinerary entry item templates from the underlying
        /// data source. The operation is performed asynchronously and supports cancellation through the provided
        /// <paramref name="cancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing a list of <see cref="ItineraryEntryItemTemplateDto"/> objects.</returns>
        public async Task<IBaseResult<List<ItineraryEntryItemTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<ItineraryEntryItemTemplateDto>>("itinerary-item-templates/all");
            return result;
        }

        /// <summary>
        /// Retrieves an itinerary entry item template by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the itinerary entry item template associated
        /// with the specified <paramref name="id"/>. Ensure that the provided <paramref name="id"/> corresponds to an
        /// existing template in the system.</remarks>
        /// <param name="id">The unique identifier of the itinerary entry item template to retrieve. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="ItineraryEntryItemTemplateDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<ItineraryEntryItemTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<ItineraryEntryItemTemplateDto>($"itinerary-item-templates/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new itinerary entry item template asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided itinerary entry item template to the underlying data
        /// provider for creation. Ensure that the <paramref name="dto"/> contains all required fields before calling
        /// this method.</remarks>
        /// <param name="dto">The data transfer object representing the itinerary entry item template to be added.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object that includes the added <see cref="ItineraryEntryItemTemplateDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<ItineraryEntryItemTemplateDto>> AddAsync(ItineraryEntryItemTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<ItineraryEntryItemTemplateDto, ItineraryEntryItemTemplateDto>($"itinerary-item-templates", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing itinerary entry item template asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated itinerary entry item template to the underlying
        /// provider for processing. Ensure that the <paramref name="dto"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the itinerary entry item template.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the updated <see cref="ItineraryEntryItemTemplateDto"/>.</returns>
        public async Task<IBaseResult<ItineraryEntryItemTemplateDto>> EditAsync(ItineraryEntryItemTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<ItineraryEntryItemTemplateDto, ItineraryEntryItemTemplateDto>($"itinerary-item-templates", dto);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync($"itinerary-item-templates", id);
            return result;
        }
    }
}
