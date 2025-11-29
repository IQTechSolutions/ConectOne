namespace GoogleCalendar.Base;

/// <summary>
/// Provides a base class for services that perform HTTP operations using an injected HttpClient instance.
/// </summary>
/// <remarks>BaseService is intended to be inherited by service classes that require HTTP communication. It
/// centralizes common HTTP functionality and error handling to promote code reuse and consistency across derived
/// services. This class is not intended to be used directly.</remarks>
public abstract class BaseService
{

    protected readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the BaseService class using the specified HttpClient.
    /// </summary>
    /// <param name="httpClient">The HttpClient instance to be used for sending HTTP requests. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if httpClient is null.</exception>
    public BaseService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Writes a log entry with the specified message and a UTC timestamp to the console output.
    /// </summary>
    /// <param name="message">The message to include in the log entry. Cannot be null.</param>
    protected void Log(string message)
    {
        Console.WriteLine($"[{DateTime.UtcNow}] - {message}");
    }

    /// <summary>
    /// Executes the specified asynchronous operation and logs any exceptions that occur.
    /// </summary>
    /// <remarks>If the operation throws an exception, the exception is logged and then rethrown to the
    /// caller. The caller is responsible for handling any exceptions that are not handled within the
    /// operation.</remarks>
    /// <param name="operation">A delegate representing the asynchronous operation to execute. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task HandleErrorAsync(Func<Task> operation)
    {
        try
        {
            await operation();
        }
        catch (Exception ex)
        {
            Log($"Error occurred: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Executes the specified asynchronous operation and logs any exceptions that occur before rethrowing them.
    /// </summary>
    /// <remarks>This method logs any exception thrown by the operation before rethrowing it to the caller.
    /// The original exception is not altered. Ensure that the provided operation does not return null if a non-null
    /// result is required by the caller.</remarks>
    /// <typeparam name="T">The type of the result returned by the asynchronous operation.</typeparam>
    /// <param name="operation">A function that represents the asynchronous operation to execute. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value returned by the operation.</returns>
    protected async Task<T> HandleErrorAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            Log($"Error occurred: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Sends an asynchronous HTTP GET request to the specified URI and returns the response body as a string.
    /// </summary>
    /// <remarks>The request is considered successful only if the HTTP response status code indicates success;
    /// otherwise, an exception is thrown. The caller is responsible for handling any exceptions that may occur due to
    /// network errors or unsuccessful status codes.</remarks>
    /// <param name="uri">The URI of the resource to retrieve. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response body as a string.</returns>
    protected async Task<string> GetAsync(string uri)
    {
        Log($"GET request to {uri}");
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Sends a POST request to the specified URI with the provided HTTP content and returns the response body as a
    /// string.
    /// </summary>
    /// <param name="uri">The URI to which the POST request is sent. Must be a valid absolute or relative URI.</param>
    /// <param name="content">The HTTP content to send in the request body. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response body as a string.</returns>
    protected async Task<string> PostAsync(string uri, HttpContent content)
    {
        Log($"POST request to {uri}");
        HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}