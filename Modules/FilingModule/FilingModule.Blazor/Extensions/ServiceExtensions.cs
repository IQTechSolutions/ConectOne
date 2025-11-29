using FilingModule.Blazor.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.JSInterop;

namespace FilingModule.Blazor.Extensions
{
    /// <summary>
    /// Provides extension methods for registering Blazor file download services with an <see
    /// cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds the Blazor download file manager service to the specified service collection.
        /// </summary>
        /// <remarks>This method registers an implementation of IBlazorDownloadFileManager for use with
        /// dependency injection. Call this method during application startup to enable file download functionality in
        /// Blazor applications.</remarks>
        /// <param name="services">The service collection to which the Blazor download file manager will be added. Cannot be null.</param>
        /// <param name="lifetime">The lifetime with which to register the Blazor download file manager service. The default is Scoped.</param>
        /// <returns>The original service collection with the Blazor download file manager service registered.</returns>
        public static IServiceCollection AddBlazorDownloadFile(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            return ServiceCollectionDescriptorExtensions.Add(services, new ServiceDescriptor(typeof(IBlazorDownloadFileManager), sp => new BlazorDownloadFileManager(sp.GetRequiredService<IJSRuntime>()), lifetime));
        }
    }
}
