using Microsoft.Extensions.DependencyInjection;
using ProductsModule.Domain.Interfaces;
using ProductsModule.Infrastructure.Implementations;

namespace ProductsModule.Infrastructure
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
        public static IServiceCollection AddProductsModule(this IServiceCollection services)
        {
            // Domain-specific services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceTierService, ServiceTierService>();

            return services;
        }
    }

}
