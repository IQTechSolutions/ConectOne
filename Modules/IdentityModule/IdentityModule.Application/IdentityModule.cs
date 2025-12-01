using IdentityModule.Application.RestServices;
using IdentityModule.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityModule.Application
{
    /// <summary>
    /// Provides an extension method to register all services related to the Schools module.
    /// </summary>
    public static class IdentityModule
    {
        /// <summary>
        /// Registers all repository managers, services, and utilities related to the Schools module
        /// into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to which the services will be added.</param>
        /// <returns>The updated IServiceCollection instance.</returns>
        public static IServiceCollection AddIdentityModule(this IServiceCollection services, string baseAddress)
        {
            services.AddScoped<IAuditTrailsService, AuditTrialsRestService>();
            services.AddScoped<ITokenService, TokenRestService>();
            services.AddScoped<IUserService, UserRestService>();
            services.AddScoped<IRoleService, RoleRestService>();
            services.AddScoped<IPermissionService, PermissionRestService>();
            services.AddScoped<IExportService, ExportRestService>();

            return services;
        }
    }
}
