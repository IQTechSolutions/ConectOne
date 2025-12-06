using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="IDayTourActivityTemplateService"/> interface for managing
    /// day tour activity templates.
    /// </summary>
    /// <remarks>This service facilitates CRUD operations for day tour activity templates by interacting with
    /// an underlying HTTP provider.  It supports asynchronous operations for retrieving, adding, updating, and deleting
    /// templates.  Use this service to manage day tour activity templates in a RESTful environment.</remarks>
    /// <param name="provider"></param>
    public class DayTourActivityTemplateRestService(IBaseHttpProvider provider) : IDayTourActivityTemplateService 
    {
        /// <summary>
        /// Retrieves all day tour activity templates asynchronously.
        /// </summary>
        /// <remarks>This method fetches all available day tour activity templates from the underlying
        /// data source. The operation is performed asynchronously and can be canceled using the provided <paramref
        /// name="cancellationToken"/>.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing a list of <see cref="DayTourActivityTemplateDto"/> objects.</returns>
        public async Task<IBaseResult<List<DayTourActivityTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<DayTourActivityTemplateDto>>("day-tour-activity-templates/all");
            return result;
        }

        /// <summary>
        /// Retrieves a day tour activity template by its unique identifier.
        /// </summary>
        /// <remarks>This method sends a request to retrieve the day tour activity template associated
        /// with the specified identifier. Ensure the <paramref name="id"/> corresponds to a valid template.</remarks>
        /// <param name="id">The unique identifier of the day tour activity template to retrieve. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="DayTourActivityTemplateDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<DayTourActivityTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<DayTourActivityTemplateDto>($"day-tour-activity-templates/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new day tour activity template asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided day tour activity template to the underlying data
        /// provider for creation.</remarks>
        /// <param name="dto">The data transfer object representing the day tour activity template to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the operation,
        /// including the added day tour activity template.</returns>
        public async Task<IBaseResult<DayTourActivityTemplateDto>> AddAsync(DayTourActivityTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<DayTourActivityTemplateDto, DayTourActivityTemplateDto>($"day-tour-activity-templates", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing day tour activity template with the specified data.
        /// </summary>
        /// <remarks>This method sends the updated day tour activity template data to the underlying
        /// provider for processing. Ensure that the <paramref name="dto"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the day tour activity template.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the updated <see cref="DayTourActivityTemplateDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<DayTourActivityTemplateDto>> EditAsync(DayTourActivityTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<DayTourActivityTemplateDto, DayTourActivityTemplateDto>($"day-tour-activity-templates", dto);
            return result;
        }

        /// <summary>
        /// Deletes the specified resource asynchronously.
        /// </summary>
        /// <remarks>This method performs an asynchronous delete operation for the specified resource.
        /// Ensure that the provided  <paramref name="id"/> corresponds to an existing resource. The operation may fail
        /// if the resource does not exist  or if there are connectivity issues.</remarks>
        /// <param name="id">The unique identifier of the resource to delete. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see
        /// cref="IBaseResult"/>  indicating the outcome of the operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("day-tour-activity-templates", id);
            return result;
        }
    }
}
