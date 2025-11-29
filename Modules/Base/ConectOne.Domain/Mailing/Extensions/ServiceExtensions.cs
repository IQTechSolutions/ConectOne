using ConectOne.Domain.Mailing.Interfaces;
using ConectOne.Domain.Mailing.Services;
using ConectOne.Domain.Mailing.TemplateSender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConectOne.Domain.Mailing.Extensions
{
    /// <summary>
    /// Provides extension methods for registering email-related services with an application's dependency injection
    /// container.
    /// </summary>
    /// <remarks>This class contains static methods that extend IServiceCollection to simplify the
    /// configuration and registration of email services. These methods are intended to be used during application
    /// startup to ensure all required email components are available for dependency injection.</remarks>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers email-related services and configuration with the dependency injection container.
        /// </summary>
        /// <remarks>This method configures the required services for email sending, queuing, and
        /// templating. It expects the configuration to contain a section matching the name of the EmailConfiguration
        /// type. Call this method during application startup to enable email functionality.</remarks>
        /// <param name="services">The service collection to which the email services will be added.</param>
        /// <param name="configuration">The application configuration containing the email settings section.</param>
        /// <returns>The same service collection instance, for chaining additional service registrations.</returns>
        public static IServiceCollection ConfigureEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection(typeof(EmailConfiguration).Name).Get<EmailConfiguration>();
            services.AddSingleton(config);

            services.AddSingleton<EmailQueue>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddTransient<IEmailTemplateSender, EmailTemplateSender>();
            services.AddTransient<DefaultEmailSender>();

            return services;
        }
    }
}
