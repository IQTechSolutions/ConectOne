using MessagingModule.Domain.Interfaces;
using MessagingModule.Infrastructure.Implementation;
using MessagingModule.Infrastructure.Mails;
using MessagingModule.Infrastructure.Queues;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingModule.Infrastructure
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
        public static IServiceCollection AddMassagingModule(this IServiceCollection services)
        {
            services.AddSingleton<NotificationQueue>();
            services.AddHostedService<NotificationBackgroundService>();

            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<IChatGroupService, ChatGroupService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<NotificationsEmailSender>();

            return services;
        }
    }
}
