using CalendarModule.Domain.Interfaces;
using CalendarModule.Infrastructure.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace CalendarModule.Infrastructure
{
    /// <summary>
    /// Provides extension methods for configuring the Calendar module in an application's dependency injection
    /// container.
    /// </summary>
    /// <remarks>This class contains methods to register services related to the Calendar module, such as
    /// appointment management.</remarks>
    public static class CalanderModule
    {
        /// <summary>
        /// Adds the Calendar module services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method registers the <see cref="IAppointmentService"/> implementation with a
        /// scoped lifetime.</remarks>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to which the Calendar module services will be added.</param>
        /// <param name="baseAddress">The base address used for configuring the Calendar module. This parameter is currently unused.</param>
        /// <returns>The <see cref="IServiceCollection"/> instance with the Calendar module services added.</returns>
        public static IServiceCollection AddCalanderModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAppointmentService, AppointmentService>();

            return serviceCollection;

        }
    }
}
