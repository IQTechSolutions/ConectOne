using ConectOne.Domain.ResultWrappers;

namespace ConectOne.Domain.Interfaces
{
    /// <summary>
    /// Interface for HTTP provider to handle HTTP requests and responses.
    /// </summary>
    public interface IBaseHttpProvider
    {
        /// <summary>
        /// Adds a security token to the current context for authentication or authorization purposes.
        /// </summary>
        /// <param name="accessToken">The security token to be added. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddSecurityToken(string accessToken);

        /// <summary>
        /// Sends a GET request to retrieve a paginated list of entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TParameter">The type of the query parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <returns>A paginated result containing the entities.</returns>
        Task<PaginatedResult<TEntity>> GetPagedAsync<TEntity, TParameter>(string url, TParameter parameters);
        
        /// <summary>
        /// Sends a GET request to retrieve a single entity with query parameters.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TParameter">The type of the query parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <returns>A result containing the entity.</returns>
        Task<IBaseResult<TEntity>> GetAsync<TEntity, TParameter>(string url, TParameter parameters);

        /// <summary>
        /// Sends a GET request to retrieve a single entity without query parameters.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>A result containing the entity.</returns>
        Task<IBaseResult<TEntity>> GetAsync<TEntity>(string url);

        /// <summary>
        /// Sends a POST request with parameters to create a new entity and retrieve the created entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TParameter">The type of the parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The parameters to include in the request body.</param>
        /// <returns>A result containing the created entity.</returns>
        Task<IBaseResult<TEntity>> PostAsync<TEntity, TParameter>(string url, TParameter parameters);

        /// <summary>
        /// Sends a POST request with parameters to create a new entity.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The parameters to include in the request body.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> PostAsync<TParameter>(string url, TParameter parameters);

        /// <summary>
        /// Sends an asynchronous HTTP POST request to the specified URL with the provided content and returns the
        /// result.
        /// </summary>
        /// <typeparam name="TParameter">The type of the result object expected from the response.</typeparam>
        /// <param name="url">The URL to which the POST request is sent. Cannot be null or empty.</param>
        /// <param name="parameters">The HTTP content to include in the POST request body. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{TParameter}"/> representing the response from the server.</returns>
        Task<IBaseResult<TParameter>> PostingAsync<TParameter>(string url, HttpContent parameters);

        /// <summary>
        /// Sends a POST request without parameters to create a new entity and retrieve the created entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>A result containing the created entity.</returns>
        Task<IBaseResult<TEntity>> PostAsync<TEntity>(string url);

        /// <summary>
        /// Sends a POST request without parameters to create a new entity.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> PostAsync(string url);

        /// <summary>
        /// Sends a PUT request with parameters to update an existing entity and retrieve the updated entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TParameter">The type of the parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The parameters to include in the request body.</param>
        /// <returns>A result containing the updated entity.</returns>
        Task<IBaseResult<TEntity>> PutAsync<TEntity, TParameter>(string url, TParameter parameters);

        /// <summary>
        /// Sends a PUT request with parameters to update an existing entity.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The parameters to include in the request body.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> PutAsync<TParameter>(string url, TParameter parameters);

        /// <summary>
        /// Sends a DELETE request to delete an entity by its ID and retrieve the deleted entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A result containing the deleted entity.</returns>
        Task<IBaseResult<TEntity>> DeleteAsync<TEntity>(string url, string id);

        /// <summary>
        /// Sends a DELETE request to delete an entity by its ID.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        Task<IBaseResult> DeleteAsync(string url, string id);
    }
}
