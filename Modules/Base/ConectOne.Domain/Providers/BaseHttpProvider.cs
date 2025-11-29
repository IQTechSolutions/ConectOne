using System.Net.Http.Headers;
using System.Net.Http.Json;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace ConectOne.Domain.Providers
{
    /// <summary>
    /// Provides a base implementation for making HTTP requests, including support for common operations such as GET,
    /// POST, PUT, and DELETE, with optional query parameters and authentication.
    /// </summary>
    /// <remarks>This class is designed to simplify HTTP communication by wrapping common request patterns and
    /// providing methods for handling paginated results, single entities, and parameterized requests. It uses an <see
    /// cref="HttpClient"/> instance for all HTTP operations, which is configured with a default timeout of 30 seconds
    /// and cleared default headers upon initialization.</remarks>
    public class BaseHttpProvider : IBaseHttpProvider
    {
        /// <summary>
        /// Represents the <see cref="HttpClient"/> instance used to send HTTP requests.
        /// </summary>
        /// <remarks>This field is intended for use by derived classes to perform HTTP operations.  It is
        /// initialized and managed by the containing class and should not be modified directly.</remarks>
        protected readonly HttpClient Client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHttpProvider"/> class.
        /// </summary>
        /// <param name="client">The HTTP client to use for making requests.</param>
        public BaseHttpProvider(HttpClient client)
        {
            Client = client;
            Client.Timeout = new TimeSpan(0, 0, 30);
            Client.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// Adds a security token to the application for authentication. 
        /// </summary>
        /// <param name="accessToken">The access token to add to the application</param>
        public Task AddSecurityToken(string accessToken)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// Sends a GET request to retrieve a paginated list of entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TParameter">The type of the query parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <returns>A paginated result containing the entities.</returns>
        public async Task<PaginatedResult<TEntity>> GetPagedAsync<TEntity, TParameter>(string url, TParameter parameters)
        {
            var result = await Client.GetAsync($"{url}?{parameters.GetQueryString()}");
            return await result.ToPaginatedResultAsync<TEntity>();
        }

        /// <summary>
        /// Sends a GET request to retrieve a single entity with query parameters.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TParameter">The type of the query parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <returns>A result containing the entity.</returns>
        public async Task<IBaseResult<TEntity>> GetAsync<TEntity, TParameter>(string url, TParameter parameters)
        {
            var result = await Client.GetAsync($"{url}?{parameters.GetQueryString()}");
            return await result.ToResultAsync<TEntity>();
        }

        /// <summary>
        /// Sends a GET request to retrieve a single entity without query parameters.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>A result containing the entity.</returns>
        public async Task<IBaseResult<TEntity>> GetAsync<TEntity>(string url)
        {
            var result = await Client.GetAsync(url);
            return await result.ToResultAsync<TEntity>();
        }

        /// <summary>
        /// Sends a POST request with parameters to create a new entity and retrieve the created entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TParameter">The type of the parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The parameters to include in the request body.</param>
        /// <returns>A result containing the created entity.</returns>
        public async Task<IBaseResult<TEntity>> PostAsync<TEntity, TParameter>(string url, TParameter parameters)
        {
            var result = await Client.PostAsJsonAsync(url, parameters);
            return await result.ToResultAsync<TEntity>();
        }

        /// <summary>
        /// Sends a POST request with parameters to create a new entity.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The parameters to include in the request body.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> PostAsync<TParameter>(string url, TParameter parameters)
        {
            var result = await Client.PostAsJsonAsync(url, parameters);
            return await result.ToResultAsync();
        }

        /// <summary>
        /// Sends an HTTP POST request to the specified URL with the provided content and processes the response.
        /// </summary>
        /// <remarks>This method sends an HTTP POST request using the <see cref="HttpClient"/> instance
        /// and processes the  response into a result of type <typeparamref name="TParameter"/>. Ensure that the URL is
        /// valid and  the content is properly formatted for the server.</remarks>
        /// <typeparam name="TParameter">The type of the result object to be deserialized from the response.</typeparam>
        /// <param name="url">The URL to which the POST request is sent. This cannot be null or empty.</param>
        /// <param name="parameters">The HTTP content to include in the POST request body. This cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object implementing  <see
        /// cref="IBaseResult{TParameter}"/> that encapsulates the deserialized response.</returns>
        public async Task<IBaseResult<TParameter>> PostingAsync<TParameter>(string url, HttpContent parameters)
        {
            var result = await Client.PostAsync(url,parameters);
            return await result.ToResultAsync<TParameter>();
        }

        /// <summary>
        /// Sends a POST request without parameters to create a new entity and retrieve the created entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>A result containing the created entity.</returns>
        public async Task<IBaseResult<TEntity>> PostAsync<TEntity>(string url)
        {
            var result = await Client.PostAsync(url, null);
            return await result.ToResultAsync<TEntity>();
        }

        /// <summary>
        /// Sends a POST request without parameters to create a new entity.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> PostAsync(string url)
        {
            var result = await Client.PostAsync(url, null);
            return await result.ToResultAsync();
        }

        /// <summary>
        /// Sends a PUT request with parameters to update an existing entity and retrieve the updated entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TParameter">The type of the parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The parameters to include in the request body.</param>
        /// <returns>A result containing the updated entity.</returns>
        public async Task<IBaseResult<TEntity>> PutAsync<TEntity, TParameter>(string url, TParameter parameters)
        {
            var result = await Client.PutAsJsonAsync(url, parameters);
            return await result.ToResultAsync<TEntity>();
        }

        /// <summary>
        /// Sends a PUT request with parameters to update an existing entity.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameters.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="parameters">The parameters to include in the request body.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> PutAsync<TParameter>(string url, TParameter parameters)
        {
            var result = await Client.PutAsJsonAsync(url, parameters);
            return await result.ToResultAsync();
        }

        /// <summary>
        /// Sends a DELETE request to delete an entity by its ID and retrieve the deleted entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A result containing the deleted entity.</returns>
        public async Task<IBaseResult<TEntity>> DeleteAsync<TEntity>(string url, string id)
        {
            var result = await Client.DeleteAsync($"{url}/{id}");
            return await result.ToResultAsync<TEntity>();
        }

        /// <summary>
        /// Sends a DELETE request to delete an entity by its ID.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> DeleteAsync(string url, string id)
        {
            var result = await Client.DeleteAsync($"{url}/{id}");
            return await result.ToResultAsync();
        }
    }
}
