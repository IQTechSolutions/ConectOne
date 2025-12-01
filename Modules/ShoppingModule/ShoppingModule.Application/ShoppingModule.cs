using Microsoft.Extensions.DependencyInjection;
using ShoppingModule.Application.RestServices;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Application
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
        /// implementations for  <see cref="ICouponService"/> and <see cref="services"/>. These services are
        /// registered with a scoped lifetime.</remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the shopping module services will be added.</param>
        /// <returns>The same <see cref="IShoppingCartService"/> instance, allowing for method chaining.</returns>
        public static IServiceCollection AddShoppingModule(this IServiceCollection services, string baseAddress)
        {
         //   services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseAddress); c.EnableIntercept(sp); }).AddHttpMessageHandler<AuthenticationHeaderHandler>();
            
            services.AddScoped<IShoppingCartService, ShoppingCartRestService>();
            services.AddScoped<ICouponService, CouponRestService>();
            services.AddScoped<ISalesOrderService, SalesOrderRestService>();
            services.AddScoped<ISalesOrderDetailService, SalesOrderDetailRestService>();
            services.AddScoped<IDonationService, DonationRestService>();

            return services;
        }
    }
}
