using CalendarModule.Application.RestServices;
using CalendarModule.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CalendarModule.Application
{
    /// <summary>
    /// Adds the services required for the business module to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>This method registers the following services: <list type="bullet"> <item> <description>An
    /// HTTP client for <see cref="IBaseHttpProvider"/> with the specified <paramref name="baseAddress"/> and an
    /// authentication message handler.</description> </item> <item> <description>A scoped implementation of <see
    /// cref="IAppointmentService"/> using <see cref="AppointmentRestService"/>.</description> </item> </list></remarks>
    public static class CalanderModule
    {
        /// <summary>
        /// Adds the Calendar module services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method registers the HTTP client and related services required for the Calendar
        /// module.  It configures the HTTP client with the specified base address and enables request interception.
        /// Additionally, it registers the <see cref="IAppointmentService"/> implementation for dependency
        /// injection.</remarks>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <param name="baseAddress">The base address for the HTTP client used by the Calendar module.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddCalanderModule(this IServiceCollection serviceCollection, string baseAddress)
        {
            serviceCollection.AddScoped<IAppointmentService, AppointmentRestService>();

            return serviceCollection;

        }
    }
}
