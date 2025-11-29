using System.Reflection;
using System.Web;
using Newtonsoft.Json;

namespace ConectOne.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="HttpClient"/> class to simplify making HTTP requests.
    /// </summary>
    /// <remarks>This static class includes methods for performing common HTTP operations such as GET, POST,
    /// PUT, and DELETE requests. It also provides utility methods for generating MIME types and query strings. These
    /// methods are designed to handle JSON serialization and deserialization, manage HTTP headers, and process
    /// pagination metadata when applicable.</remarks>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Retrieves the MIME type associated with the specified file name based on its extension.
        /// </summary>
        /// <remarks>This method uses the Windows Registry to look up the MIME type for the file
        /// extension.  If the file extension is not found in the registry or the registry does not contain a "Content
        /// Type" value,  the method defaults to <c>"application/unknown"</c>.</remarks>
        /// <param name="fileName">The name of the file, including its extension, for which to determine the MIME type.</param>
        /// <returns>A string representing the MIME type associated with the file extension.  If the MIME type cannot be
        /// determined, returns <c>"application/unknown"</c>.</returns>
        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey? regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        /// <summary>
        /// Converts the properties of the specified object into a query string representation.
        /// </summary>
        /// <remarks>This method processes both top-level and nested properties of the object. The
        /// resulting query string  is formatted as key-value pairs joined by '&', where keys represent property names
        /// and values represent  their corresponding values.</remarks>
        /// <param name="obj">The object whose properties will be converted into a query string.  Nested properties are also included in
        /// the resulting query string.</param>
        /// <returns>A query string representation of the object's properties, with key-value pairs  joined by '&'. Returns an
        /// empty string if the object has no properties.</returns>
        public static string GetQueryString(this object obj)
        {
            var propValues = GetNestedPropertyValues(obj);
            var finalQueryString = string.Join('&', propValues);
            return finalQueryString;

        }

        /// <summary>
        /// Retrieves the values of all non-indexed properties from the specified object, including nested properties.
        /// </summary>
        /// <param name="obj">The object from which to retrieve property values. Must not be <see langword="null"/>.</param>
        /// <returns>An enumerable collection of strings representing the values of all non-indexed properties  of the specified
        /// object, including values from nested properties. Returns an empty collection  if the object has no
        /// properties or all property values are <see langword="null"/>.</returns>
        private static IEnumerable<string> GetNestedPropertyValues(object obj)
        {
            return obj.GetType().GetProperties().Where(p => !p.GetIndexParameters().Any()).Where(p => p.GetValue(obj, null) != null)
                .SelectMany(nestedProperty => GetPropertyValues(nestedProperty, obj));
        }

        /// <summary>
        /// Retrieves a collection of URL-encoded property values for the specified property of an object.
        /// </summary>
        /// <remarks>This method supports nested properties and arrays. For nested properties, the values
        /// are prefixed with the parent property name. For arrays, each element is processed individually. The property
        /// name and values are URL-encoded to ensure safe inclusion in query strings.</remarks>
        /// <param name="property">The <see cref="PropertyInfo"/> representing the property to retrieve values from.</param>
        /// <param name="parentObject">The object containing the property whose values are to be retrieved. Cannot be <see langword="null"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of strings representing the URL-encoded property values.  If the property is
        /// a nested object or an array, the values are flattened and prefixed with the property name.</returns>
        private static IEnumerable<string> GetPropertyValues(PropertyInfo property, object parentObject)
        {
                try
                {
                    string propertyName = property.Name;
            object propertyValue = property.GetValue(parentObject)!;
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                if (property.PropertyType.IsArray && propertyValue != null)
                {
                    return GetArrayValues(propertyName, (Array)propertyValue);
                }
                else
                {
                    return GetNestedPropertyValues(propertyValue)
                        .Select(nestedValue =>
                            $"{HttpUtility.UrlEncode(propertyName)}.{HttpUtility.UrlEncode(nestedValue)}");
                }
            }
            else
            {
                return new[] { $"{HttpUtility.UrlEncode(propertyName)}={HttpUtility.UrlEncode(propertyValue?.ToString())}" };
            }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

        /// <summary>
        /// Generates a collection of URL-encoded key-value pairs representing the elements of an array.
        /// </summary>
        /// <remarks>Each element in the array is converted to a string using its <see
        /// cref="object.ToString"/> method.  If an element is null, its value in the key-value pair will be an empty
        /// string.</remarks>
        /// <param name="propertyName">The base name to use as the key for each array element. Each key will be suffixed with the element's index
        /// (e.g., "propertyName[0]").</param>
        /// <param name="array">The array whose elements will be converted into key-value pairs. Cannot be null.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of strings, where each string is a URL-encoded key-value pair in the format
        /// "key=value".</returns>
        private static IEnumerable<string> GetArrayValues(string propertyName, Array array)
        {
            try
            {

                return Enumerable.Range(0, array.Length)
        .Select(i =>
        {
            var arrayElementValue = HttpUtility.UrlEncode(array.GetValue(i)?.ToString()) ?? string.Empty;
            var arrayPropName = propertyName + $"[{i}]";
            return $"{HttpUtility.UrlEncode(arrayPropName)}={arrayElementValue}";
        });


            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

    /// <summary>
    /// Represents the result of an operation, including its success status and any associated error details.
    /// </summary>
    /// <remarks>This class is typically used to encapsulate the outcome of an operation, providing both a
    /// success flag and an optional error object that contains additional information about a failure, if
    /// applicable.</remarks>
    public class Erroring
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        [JsonProperty("success")] public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error details associated with the current operation.
        /// </summary>
        [JsonProperty("error")] public Error Error { get; set; }
    }

    /// <summary>
    /// Represents an error with a specific code and message.
    /// </summary>
    /// <remarks>This class is typically used to encapsulate error details, including a numeric code and a
    /// descriptive message.</remarks>
    public class Error
    {
        /// <summary>
        /// Gets or sets the code associated with the object.
        /// </summary>
        [JsonProperty("code")] public int Code { get; set; }

        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        [JsonProperty("message")] public string Message { get; set; }
    }

    /// <summary>
    /// Represents an HTTP error with a status code and a descriptive message.
    /// </summary>
    /// <remarks>This class is typically used to encapsulate error information in HTTP responses,  providing
    /// both the HTTP status code and a human-readable error message.</remarks>
    public class HttpError
    {
        /// <summary>
        /// Gets or sets the HTTP status code associated with the response.
        /// </summary>
        [JsonProperty("statuscode")] public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the operation or context.
        /// </summary>
        [JsonProperty("message")] public string Message { get; set; }
    }

}