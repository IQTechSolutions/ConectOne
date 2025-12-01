using Microsoft.Extensions.DependencyInjection;
using ProductsModule.Application.RestServices;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Application
{
    /// <summary>
    /// Provides an extension method to register all services related to the Schools module.
    /// </summary>
    public static class ProductsModule
    {
        /// <summary>
        /// Registers all repository managers, services, and utilities related to the Schools module
        /// into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to which the services will be added.</param>
        /// <returns>The updated IServiceCollection instance.</returns>
        public static IServiceCollection AddProductsModule(this IServiceCollection services, string baseAddress)
        {
            // Domain-specific services
            services.AddScoped<IProductService, ProductRestService>();
            services.AddScoped<IProductCategoryService, ProductCategoryRestService>();
            services.AddScoped<IServiceService, ServiceRestService>();
            services.AddScoped<IServiceTierService, ServiceTierRestService>();

            return services;
        }
    }

}
