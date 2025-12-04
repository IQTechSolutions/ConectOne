using FilingModule.Application;
using MessagingModule.Application.HubServices;
using MessagingModule.Application.RestServices;
using MessagingModule.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingModule.Application
{
    /// <summary>
    /// Configures and registers messaging-related services in the application's dependency injection container.
    /// </summary>
    /// <remarks>This method registers various services required for messaging functionality, including image
    /// processing, push notifications, chat services, and SignalR integration. It is intended to be used during
    /// application startup to configure the dependency injection container.</remarks>
    public static class MessagingModule
    {
        /// <summary>
        /// Adds the messaging module services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method registers various services related to messaging functionality, including
        /// image processing,  push notifications, chat groups, chat services, message handling, and notifications.
        /// These services are  registered with a scoped lifetime, ensuring a new instance is created for each request
        /// within the scope.</remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the messaging module services will be added.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddMessagingModule(this IServiceCollection services, string baseAddress)
        {
            services.AddFilingModule();

            services.AddScoped<NotificationHubService>();

            services.AddScoped<IPushNotificationService, PushNotificationRestService>();
            services.AddScoped<IChatGroupService, ChatGroupRestService>();
            services.AddScoped<IChatService, ChatRestService>();
            services.AddScoped<IMessageService, MessageRestService>();
            services.AddScoped<INotificationService, NotificationRestService>();

            return services;
        }
    }
}
