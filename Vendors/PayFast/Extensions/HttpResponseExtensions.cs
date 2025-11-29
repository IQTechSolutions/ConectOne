using Newtonsoft.Json;

namespace PayFast.Extensions
{
    /// <summary>
    /// Provides extension methods for deserializing HTTP response content into objects.
    /// </summary>
    /// <remarks>These extension methods are intended to simplify the process of converting HTTP response
    /// content to strongly typed objects when working with HTTP APIs. The methods assume that the response content is
    /// formatted as JSON and use default JSON deserialization settings unless otherwise specified.</remarks>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Deserializes the HTTP response content to an object of the specified type.
        /// </summary>
        /// <remarks>This method expects the response content to be in JSON format. The deserialization
        /// process allows non-public default constructors on the target type. The method blocks while reading the
        /// response content; consider using an asynchronous alternative in performance-sensitive or UI
        /// applications.</remarks>
        /// <typeparam name="T">The type to which the response content is deserialized. Must be a reference type.</typeparam>
        /// <param name="response">The HTTP response message containing the content to deserialize. Cannot be null.</param>
        /// <returns>An instance of type T deserialized from the response content, or null if the content is empty or cannot be
        /// deserialized to the specified type.</returns>
        public static T Deserialize<T>(this HttpResponseMessage response) where T : class
        {
            var settings = new JsonSerializerSettings();
            settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;

            var stream = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<T>(stream, settings);
        }
    }
}
