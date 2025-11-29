using Microsoft.Extensions.DependencyInjection;
using ShoppingModule.Domain.Interfaces;
using ShoppingModule.Infrastructure.Implementation;

namespace ShoppingModule.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering shopping module services in an application.
    /// </summary>
    /// <remarks>The <see cref="ShoppingModule"/> class contains methods to configure and register the
    /// services required for the shopping module, such as shopping cart and coupon management services. These services
    /// are typically registered with a scoped lifetime to ensure proper dependency injection behavior.</remarks>
    public static class ShoppingModule
    {
        /// <summary>
        /// Adds the shopping module services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method registers the required services for the shopping module, including
        /// implementations for  <see cref="IShoppingCartService"/> and <see cref="ICouponService"/>. These services are
        /// registered with a scoped lifetime.</remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the shopping module services will be added.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
        public static IServiceCollection AddShoppingModule(this IServiceCollection services)
        {
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<ISalesOrderService, SalesOrderService>();
            services.AddScoped<ISalesOrderDetailService, SalesOrderDetailService>();
            services.AddScoped<IDonationService, DonationService>();

            return services;
        }
    }
}
