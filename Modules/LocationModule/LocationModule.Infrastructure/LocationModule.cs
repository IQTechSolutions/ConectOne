
using LocationModule.Domain.Interfaces;
using LocationModule.Infrastructure.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace LocationModule.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering location-related services in the dependency injection container.
    /// </summary>
    public static class LocationModule
    {
        /// <summary>
        /// Registers all location services with scoped lifetimes.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddLocationModuleServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAddressService<>), typeof(AddressService<>));
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ILocationService, LocationService>();

            return services;

        }
    }
}
