using BloggingModule.Application.RestServices;
using BloggingModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BloggingModule.Application
{
    /// <summary>
    /// Provides extension methods for registering blogging-related services in an application.
    /// </summary>
    /// <remarks>This static class contains methods to simplify the registration of services related to
    /// blogging functionality. It is designed to be used in the application's dependency injection setup, typically in
    /// the `Startup` class or during service configuration.</remarks>
    public static class BloggingModule
    {
        /// <summary>
        /// Adds the blogging-related services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method registers the following services: <list type="bullet"> <item>
        /// <description>An HTTP client for <see cref="IBaseHttpProvider"/> with the specified <paramref
        /// name="baseAddress"/> and an authentication message handler.</description> </item> <item> <description>A
        /// scoped implementation of <see cref="IBlogPostService"/> using <see
        /// cref="BlogPostRestService"/>.</description> </item> </list></remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <param name="baseAddress">The base address used to configure the HTTP client for the blogging services.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddBloggingServices(this IServiceCollection services, string baseAddress)
        {
            services.AddScoped<IBlogPostService, BlogPostRestService>();
            services.AddScoped<IBlogPostCategoryService, BlogPostCategoryRestService>();


            return services;
        }
    }
}
