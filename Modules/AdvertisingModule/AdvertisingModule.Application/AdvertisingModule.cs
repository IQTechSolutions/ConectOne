using AdvertisingModule.Application.RestServices;
using AdvertisingModule.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AdvertisingModule.Application
{
    /// <summary>
    /// Provides methods for registering advertising-related services in an application's dependency injection
    /// container.
    /// </summary>
    /// <remarks>This static class is designed to simplify the registration of services related to
    /// advertisements and affiliates. It includes a method to add the necessary services to an <see
    /// cref="IServiceCollection"/> with scoped lifetimes.</remarks>
    public static class AdvertisingModule
    {
        /// <summary>
        /// Adds the advertising-related services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method registers the following services with scoped lifetimes: <list
        /// type="bullet"> <item><description><see cref="IAdvertisementCommandService"/> mapped to </remarks>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
        public static IServiceCollection AddAdvertisingServices(this IServiceCollection serviceCollection, string baseAddress)
        {
            serviceCollection.AddScoped<IAdvertisementCommandService, AdvertisementCommandRestService>()
                .AddScoped<IAdvertisementQueryService, AdvertisementQueryRestService>();

            serviceCollection.AddScoped<IAffiliateCommandService, AffiliateCommandRestService>()
                .AddScoped<IAffiliateQueryService, AffiliateQueryRestService>();

            serviceCollection.AddScoped<IAdvertisementTierQueryService, AdvertisementTierQueryRestService>()
                .AddScoped<IAdvertisementTierCommandService, AdvertisementTierCommandRestService>();

            return serviceCollection;

        }
    }
}
