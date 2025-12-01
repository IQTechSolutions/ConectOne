using FilingModule.Application.RestServices;
using FilingModule.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FilingModule.Application
{
    /// <summary>
    /// Provides extension methods for registering services related to the filing module in an application's dependency
    /// injection container.
    /// </summary>
    /// <remarks>This class contains methods to simplify the registration of services required for the filing
    /// module,  including services for image processing, document handling, and video processing.  It is intended to be
    /// used during application startup to configure the dependency injection container.</remarks>
    public static class FilingModule
    {
        /// <summary>
        /// Registers services related to the filing module into the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method adds scoped service registrations for various filing-related services,
        /// including image processing,  document handling, and media services. These services are registered with their
        /// respective interfaces, enabling  dependency injection throughout the application.</remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the filing module services will be added.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
        public static IServiceCollection AddFilingModule(this IServiceCollection services)
        {
            services.AddScoped<IVideoProcessingService, VideoProcessingRestService>();
            services.AddScoped<IImageProcessingService, ImageProcessingRestService>();

            return services;
        }
    }
}
