using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing lodging types, including retrieving, creating, updating, and removing lodging
    /// type data.
    /// </summary>
    /// <remarks>This service provides methods to interact with lodging type data, such as retrieving all
    /// lodging types,  fetching a specific lodging type by its identifier, creating new lodging types, updating
    /// existing ones,  and removing lodging types. Each method supports asynchronous execution and accepts a
    /// cancellation token  to handle task cancellation.</remarks>
    public interface ILodgingTypeService
    {
        /// <summary>
        /// Retrieves all available lodging types.
        /// </summary>
        /// <remarks>This method fetches all lodging types and returns them as a collection. The result
        /// may be empty if no lodging types are available.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// with a collection of <see cref="LodgingTypeDto"/> objects representing the lodging types.</returns>
        Task<IBaseResult<IEnumerable<LodgingTypeDto>>> AllLodgingTypesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the lodging type details for the specified lodging type ID.
        /// </summary>
        /// <param name="lodgingTypeId">The unique identifier of the lodging type to retrieve. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the lodging type details as a <see cref="LodgingTypeDto"/>. If the lodging type is not found,
        /// the result may indicate an error or an empty value, depending on the implementation.</returns>
        Task<IBaseResult<LodgingTypeDto>> LodgingTypeAsync(string lodgingTypeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new lodging type asynchronously.
        /// </summary>
        /// <remarks>Use this method to add a new lodging type to the system. Ensure that the provided
        /// <paramref name="lodgingType"/>  contains all required fields and adheres to any validation rules.</remarks>
        /// <param name="lodgingType">The lodging type data to create. This must contain valid information for the lodging type.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with the created <see cref="LodgingTypeDto"/> if the operation is successful.</returns>
        Task<IBaseResult<LodgingTypeDto>> CreateLodgingTypeAsync(LodgingTypeDto lodgingType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the details of an existing lodging type.
        /// </summary>
        /// <remarks>Use this method to update the properties of an existing lodging type in the system.
        /// Ensure that the <paramref name="lodgingType"/> object contains valid and complete data before calling this
        /// method.</remarks>
        /// <param name="lodgingType">The lodging type data transfer object containing the updated details. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the update operation.</returns>
        Task<IBaseResult> UpdateLodgingTypeAsync(LodgingTypeDto lodgingType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a lodging type identified by the specified ID.
        /// </summary>
        /// <remarks>Use this method to remove a lodging type from the system. Ensure that the specified
        /// ID corresponds to an  existing lodging type. The operation is performed asynchronously and supports
        /// cancellation.</remarks>
        /// <param name="lodgingTypeId">The unique identifier of the lodging type to be removed. Must be a valid, existing ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the operation.</returns>
        Task<IBaseResult> RemoveLodgingTypeAsync(string lodgingTypeId, CancellationToken cancellationToken = default);
    }
}
