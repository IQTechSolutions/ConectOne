using IdentityModule.Domain.Interfaces;
using IdentityModule.Infrastructure.Implimentation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityModule.Infrastructure
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
        public static IServiceCollection AddIdentityModule(this IServiceCollection services)
        {
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddScoped<IAuditTrailsService, AuditTrialsService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }

}
