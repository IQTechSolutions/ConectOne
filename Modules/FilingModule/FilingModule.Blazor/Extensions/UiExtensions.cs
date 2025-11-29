using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace FilingModule.Blazor.Extensions
{
    /// <summary>
    /// Converts a collection of images to a URL based on the specified upload type.
    /// </summary>
    /// <remarks>This method constructs the URL using the base API address specified in the application's
    /// configuration  under the key "ApiConfiguration:BaseApiAddress". If multiple images match the specified upload
    /// type,  only the first match is used.</remarks>
    public static class UiExtensions
    {
        /// <summary>
        /// Converts a collection of images to a URL based on the specified upload type.
        /// </summary>
        /// <param name="images">The collection of <see cref="ImageDto"/> objects to search for the specified upload type. Can be null.</param>
        /// <param name="uploadType">The type of image to locate within the collection.</param>
        /// <param name="configuration">The application configuration containing the base API address.</param>
        /// <param name="placeholder">An optional placeholder value to return if no matching image is found. Defaults to an empty string if not
        /// provided.</param>
        /// <returns>A URL string constructed from the base API address and the relative path of the first image matching the
        /// specified upload type. Returns the <paramref name="placeholder"/> value or an empty string if no matching
        /// image is found.</returns>
        public static string ToUrl(this ICollection<ImageDto>? images, UploadType uploadType, IConfiguration configuration, string? placeholder = null)
        {
            if (images == null || images?.FirstOrDefault(c => c.ImageType == uploadType) == null)
                return placeholder ?? string.Empty;
            return $"{configuration["ApiConfiguration:BaseApiAddress"]}/{images?.FirstOrDefault(c => c.ImageType == uploadType)?.RelativePath}";
        }
    }
}