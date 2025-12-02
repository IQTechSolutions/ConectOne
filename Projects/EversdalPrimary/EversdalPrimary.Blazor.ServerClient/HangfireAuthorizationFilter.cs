using Hangfire.Dashboard;
using IdentityModule.Domain.Constants;

namespace EversdalPrimary.Blazor.ServerClient
{
    /// <summary>
    /// Custom authorization filter for the Hangfire dashboard.
    /// Only authenticated users with the SuperUser role are allowed access.
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// Determines whether the current user is authorized to access the Hangfire dashboard.
        /// </summary>
        /// <param name="context">The Hangfire dashboard context.</param>
        /// <returns>
        /// <c>true</c> if the user is authenticated and is in the SuperUser role; otherwise, <c>false</c>.
        /// </returns>
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.IsInRole(RoleConstants.SuperUser);
        }
    }

}
