using System.Text.Json.Serialization;
using ConectOne.Domain.ResultWrappers;
using Newtonsoft.Json;

namespace ConectOne.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for converting HTTP responses to result objects.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Converts an HTTP response to a result object asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the data payload.</typeparam>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>A task representing the asynchronous operation, with a result object as the outcome.</returns>
        public static async Task<IBaseResult<T>> ToResultAsync<T>(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonConvert.DeserializeObject<Result<T>>(responseAsString);
                return responseObject!;
            }
            else
            {
                try
                {
                    var errorObject = JsonConvert.DeserializeObject<ResponseError>(responseAsString);
                    if (errorObject is not null)
                    {
                        return await Result<T>.FailAsync($"The following error occurred on the server: StatusCode - {errorObject.Status}, Message - {string.Join("->", errorObject.Errors.Select(c => $"{c.Key} : {string.Join(",", c.Value)}"))}");
                    }
                    else
                    {
                        return await Result<T>.FailAsync(new List<string> { $"The following error occurred on the server: StatusCode - {response.StatusCode}, Message - {response.ReasonPhrase}" });
                    }
                }
                catch (Exception e)
                {
                    return await Result<T>.FailAsync(new List<string> { $"The following error occurred on the server: StatusCode - {response.StatusCode}, Message - {response.ReasonPhrase}" });
                }
            }
        }

        /// <summary>
        /// Converts an HTTP response to a result object asynchronously.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>A task representing the asynchronous operation, with a result object as the outcome.</returns>
        public static async Task<IBaseResult> ToResultAsync(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseObject = JsonConvert.DeserializeObject<Result>(responseAsString);
                    return responseObject!;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                try
                {
                    var errorObject = JsonConvert.DeserializeObject<ResponseError>(responseAsString);
                    if (errorObject is not null)
                    {
                        return await Result.FailAsync($"The following error occurred on the server: StatusCode - {errorObject.Status}, Message - {string.Join("->", errorObject.Errors.Select(c => $"{c.Key} : {string.Join(",", c.Value)}"))}");
                    }
                    else
                    {
                        return await Result.FailAsync(new List<string> { $"The following error occurred on the server: StatusCode - {response.StatusCode}, Message - {response.ReasonPhrase}" });
                    }
                }
                catch (Exception e)
                {
                    return await Result.FailAsync(new List<string> { $"The following error occurred on the server: StatusCode - {response.StatusCode}, Message - {response.ReasonPhrase}" });
                }
            }
        }

        /// <summary>
        /// Converts an HTTP response to a paginated result object asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the data payload.</typeparam>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>A task representing the asynchronous operation, with a paginated result object as the outcome.</returns>
        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonConvert.DeserializeObject<PaginatedResult<T>>(responseAsString);
                return responseObject!;
            }
            else
            {
                try
                {
                    var errorObject = JsonConvert.DeserializeObject<ResponseError>(responseAsString);
                    return PaginatedResult<T>.Failure(errorObject is not null ? new List<string> { $"The following error occurred on the server: StatusCode - {errorObject.Status}, Message - {string.Join("->", errorObject.Errors.Select(c => $"{c.Key} : {string.Join(",", c.Value)}"))}" } : new List<string> { $"The following error occurred on the server: StatusCode - {response.StatusCode}, Message - {response.ReasonPhrase}" });
                }
                catch (Exception e)
                {
                    return PaginatedResult<T>.Failure(new List<string> { $"The following error occurred on the server: StatusCode - {response.StatusCode}, Message - {response.ReasonPhrase}" });
                }
            }
        }
    }

    /// <summary>
    /// Represents an error response from the server.
    /// </summary>
    public class ResponseError
    {
        /// <summary>
        /// Gets or sets the type of the object represented as a string.
        /// </summary>
        [JsonPropertyName("type")] public string Type { get; set; } = null!;

        /// <summary>
        /// Gets or sets the title associated with the object.
        /// </summary>
        [JsonPropertyName("title")] public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the status code representing the current state of the operation.
        /// </summary>
        [JsonPropertyName("status")] public int Status { get; set; }

        /// <summary>
        /// Key = field/property name, Value = list of validation messages.
        /// </summary>
        [JsonPropertyName("errors")] public Dictionary<string, string[]> Errors { get; set; } = new();

        /// <summary>
        /// Gets or sets the unique identifier for tracing a request through a distributed system.
        /// </summary>
        [JsonPropertyName("traceId")] public string TraceId { get; set; } = null!;
    }

}
