using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices
{
    /// <summary>
    /// Provides methods for managing meal addition templates through RESTful API calls.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform CRUD operations on meal addition templates.
    /// It uses an <see cref="IBaseHttpProvider"/> to send HTTP requests and handle responses.</remarks>
    /// <param name="provider"></param>
    public class MealAdditionTemplateRestService(IBaseHttpProvider provider) : IMealAdditionTemplateService
    {
        /// <summary>
        /// Retrieves all meal addition templates asynchronously.
        /// </summary>
        /// <remarks>This method fetches all available meal addition templates from the underlying data
        /// source. The operation can be canceled by passing a cancellation token.</remarks>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing a list of <see cref="MealAdditionTemplateDto"/> objects.</returns>
        public async Task<IBaseResult<List<MealAdditionTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<MealAdditionTemplateDto>>("meal-addition-templates/all");
            return result;
        }

        /// <summary>
        /// Retrieves a meal addition template by its unique identifier.
        /// </summary>
        /// <remarks>If the specified identifier does not exist, the returned result may indicate a
        /// failure or contain no data, depending on the implementation of the underlying provider.</remarks>
        /// <param name="id">The unique identifier of the meal addition template to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="MealAdditionTemplateDto"/> corresponding to the specified identifier.</returns>
        public async Task<IBaseResult<MealAdditionTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<MealAdditionTemplateDto>($"meal-addition-templates/{id}");
            return result;
        }

        /// <summary>
        /// Adds a new meal addition template asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided meal addition template to the underlying data provider
        /// for storage.  Ensure that the <paramref name="dto"/> contains valid data before calling this
        /// method.</remarks>
        /// <param name="dto">The data transfer object representing the meal addition template to be added.  This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing a cancellation token allows the operation to be
        /// canceled.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object that includes the added <see cref="MealAdditionTemplateDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<MealAdditionTemplateDto>> AddAsync(MealAdditionTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<MealAdditionTemplateDto, MealAdditionTemplateDto>("meal-addition-templates", dto);
            return result;
        }

        /// <summary>
        /// Updates an existing meal addition template asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated meal addition template to the underlying provider for
        /// processing. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
        /// <param name="dto">The data transfer object containing the updated details of the meal addition template.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the updated <see cref="MealAdditionTemplateDto"/> if the operation is successful.</returns>
        public async Task<IBaseResult<MealAdditionTemplateDto>> EditAsync(MealAdditionTemplateDto dto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync<MealAdditionTemplateDto, MealAdditionTemplateDto>("meal-addition-templates", dto);
            return result;
        }

        /// <summary>
        /// Deletes the specified resource asynchronously.
        /// </summary>
        /// <remarks>This method performs an asynchronous delete operation for the specified resource.
        /// Ensure that the  <paramref name="id"/> corresponds to a valid resource. The operation may fail if the
        /// resource does not exist.</remarks>
        /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("meal-addition-templates", id);
            return result;
        }
    }
}
