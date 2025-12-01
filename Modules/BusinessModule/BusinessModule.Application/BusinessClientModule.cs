using BusinessModule.Application.RestServices;
using BusinessModule.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessModule.Application
{
    /// <summary>
    /// Provides extension methods for registering business-related services in an <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>This class contains methods to simplify the registration of services related to business
    /// operations, such as advertising and directory management, into the dependency injection container.</remarks>
    public static class BusinessClientModule
    {
        /// <summary>
        /// Registers the services required for the business module into the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method adds scoped implementations for various business module services,
        /// including directory query and command services,  category services, and listing tier services. These
        /// services are registered to support dependency injection in the application.</remarks>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to which the business module services will be added.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
        public static IServiceCollection AddBusinessModuleServices(this IServiceCollection serviceCollection, string baseAddress)
        {
      //      serviceCollection.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseAddress); c.EnableIntercept(sp); }).AddHttpMessageHandler<AuthenticationHeaderHandler>();


            serviceCollection.AddScoped<IBusinessDirectoryQueryService, BusinessDirectoryQueryRestService>()
                .AddScoped<IBusinessDirectoryCommandService, BusinessDirectoryCommandRestService>();

            serviceCollection.AddScoped<IBusinessDirectoryCategoryService, BusinessDirectoryCategoryRestService>();

            serviceCollection.AddScoped<IListingTierQueryService, ListingTierQueryRestService>()
                .AddScoped<IListingTierCommandService, ListingTierCommandRestService>();

            return serviceCollection;

        }
    }
}
