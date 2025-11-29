using BloggingModule.Domain.Entities;
using BloggingModule.Domain.Interfaces;
using BloggingModule.Infrastructure.Implementation;
using GroupingModule.Domain.Interfaces;
using GroupingModule.Infrastructure.Implementation;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Infrastructure.Implementation;
using MessagingModule.Infrastructure.Mails;
using Microsoft.Extensions.DependencyInjection;

namespace BloggingModule.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering blogging-related services in an application's dependency injection
    /// container.
    /// </summary>
    public static class BloggingModule
    {
        /// <summary>
        /// Adds the blogging-related services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method registers the following services with scoped lifetimes: <list
        /// type="bullet"> <item><description><see cref="IBlogPostService"/> with its implementation <see
        /// cref="BlogPostService"/>.</description></item> <item><description><see cref="ICategoryService{TEntity}"/> for <see
        /// cref="BlogPost"/> with its implementation <see cref="CategoryService{T}"/>.</description></item>
        /// <item><description><see cref="IPushNotificationService"/> with its implementation <see
        /// cref="PushNotificationService"/>.</description></item> <item><description><see
        /// cref="NotificationsEmailSender"/>.</description></item> </list></remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
        public static IServiceCollection AddBloggingServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ICategoryService<>), typeof(CategoryService<>));

            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<IBlogPostCategoryService, BlogPostCategoryService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<NotificationsEmailSender>();

            return services;
        }
    }
}
