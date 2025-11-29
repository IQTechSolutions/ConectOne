using BusinessModule.Domain.Interfaces;
using BusinessModule.Infrastructure.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessModule.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering business-related services in an <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>This class contains methods to simplify the registration of services related to business
    /// operations, such as advertising and directory management, into the dependency injection container.</remarks>
    public static class BusinessModule
    {
        /// <summary>
        /// Adds advertising-related services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method registers the following services with scoped lifetimes: <list
        /// type="bullet"> <item><description><see cref="IBusinessDirectoryQueryService"/> as <see
        /// cref="BusinessDirectoryQueryService"/>.</description></item> <item><description><see
        /// cref="IBusinessDirectoryCommandService"/> as <see
        /// cref="BusinessDirectoryCommandService"/>.</description></item> </list></remarks>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with the advertising services registered.</returns>
        public static IServiceCollection AddBusinessModuleServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBusinessDirectoryQueryService, BusinessDirectoryQueryService>()
                .AddScoped<IBusinessDirectoryCommandService, BusinessDirectoryCommandService>();

            serviceCollection.AddScoped<IBusinessDirectoryCategoryService, BusinessDirectoryCategoryService>();

            serviceCollection.AddScoped<IListingTierQueryService, ListingTierQueryService>()
                .AddScoped<IListingTierCommandService, ListingTierCommandService>();

            return serviceCollection;

        }
    }
}
