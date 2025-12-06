using ConectOne.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using SchoolsEnterprise.Base.Extensions;

namespace GeckoAfrica.Blazor.ServerClient.DataSeeder
{
    /// <summary>
    /// Provides functionality to seed initial data into the application's database, including roles and users.
    /// </summary>
    /// <remarks>This class is responsible for initializing essential data required for the application to
    /// function correctly. It creates predefined roles and a super user account, assigns permissions to roles, and
    /// ensures the database is populated with necessary entities. This is typically used during application startup or
    /// deployment.</remarks>
    public class DataSeeder : IDataSeeder
    {
        private readonly ILogger<DataSeeder> _logger;
        private readonly GenericDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeeder"/> class, which is responsible for seeding  initial
        /// data into the application's database.
        /// </summary>
        /// <param name="logger">The logger instance used to log information, warnings, and errors during the data seeding process.</param>
        /// <param name="context">The database context used to interact with the application's database.</param>
        /// <param name="userManager">The user manager used to manage application users during the seeding process.</param>
        /// <param name="roleManager">The role manager used to manage application roles during the seeding process.</param>
        public DataSeeder(ILogger<DataSeeder> logger, GenericDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _logger=logger;
            _context=context;
            _userManager=userManager;
            _roleManager=roleManager;
        }

        /// <summary>
        /// Initializes the system by setting up default roles and a superuser account.
        /// </summary>
        /// <remarks>This method adds predefined roles and a superuser to the system, ensuring that  the
        /// application has the necessary initial configuration for user management.  Changes are persisted to the
        /// database upon completion.</remarks>
        public void Initialize()
        {
            AddSuperUser();
            AddRoles();
            _context.SaveChanges();
        }

        /// <summary>
        /// Ensures the existence of a "Super User" role and user in the system, with full administrative access and
        /// permissions.
        /// </summary>
        /// <remarks>This method creates the "Super User" role if it does not already exist, assigns all
        /// registered permissions to the role,  and creates a default "Super User" account if one is not found. The
        /// "Super User" account is assigned to the "Super User" role.</remarks>
        private void AddSuperUser()
        {
            Task.Run(async () =>
            {

                var superUserRole = await _roleManager.FindByNameAsync(RoleConstants.SuperUser);

                if (superUserRole == null)
                {
                    superUserRole = new ApplicationRole(RoleConstants.SuperUser, "Administrative role with full access and all permissions", true);
                    var roleResult = await _roleManager.CreateAsync(superUserRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded Super User Role.");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                var superUser = await _userManager.FindByEmailAsync("ivanrossouw2@gmail.com");

                if (superUser == null)
                {
                    superUser = new ApplicationUser(Guid.NewGuid().ToString(), "ivanrossouw2@gmail.com", "Ivan", "Rossouw", "0764348180", "ivanrossouw2@gmail.com", "Private");
                    superUser.EmailConfirmed = true;

                    var registrationResult = await _userManager.CreateAsync(superUser, "Is20170804!!");
                    if (registrationResult.Succeeded)
                    {
                        var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.SuperUser);
                        if (registrationResult.Succeeded)
                        {
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                _logger.LogError(error.Description);
                            }
                        }
                    }
                    else
                    {
                        foreach (var error in registrationResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                
                var superUser2 = await _userManager.FindByEmailAsync("franco@personablaze.com");

                if (superUser2 == null)
                {
                    superUser2 = new ApplicationUser(Guid.NewGuid().ToString(), "franco@personablaze.com", "Franco", "Blaze", "0820879305", "franco@personablaze.com", "ProGolf Safaris");
                    superUser2.EmailConfirmed = true;

                    var registrationResult = await _userManager.CreateAsync(superUser2, "1Franco!!");
                    if (registrationResult.Succeeded)
                    {
                        var result = await _userManager.AddToRoleAsync(superUser2, RoleConstants.SuperUser);
                        if (registrationResult.Succeeded)
                        {
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                _logger.LogError(error.Description);
                            }
                        }
                    }
                    else
                    {
                        foreach (var error in registrationResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                var registeredPermisions = ClaimsPrincipalExtensions.GetRegisteredPermissions();
                foreach (var permission in registeredPermisions)
                {
                    await ClaimsExtensions.AddPermissionClaim(_roleManager, superUserRole, permission);
                }
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Seeds predefined roles and their associated permissions into the system.
        /// </summary>
        /// <remarks>This method ensures that the roles required for the application, such as
        /// Administrator, Guest, and Basic,  are created if they do not already exist. It also assigns the appropriate
        /// permissions to these roles. The roles and permissions are defined based on application-specific constants
        /// and logic.</remarks>
        private void AddRoles()
        {
            Task.Run(async () =>
            {

                var administratorRole = await _roleManager.FindByNameAsync(RoleConstants.Administrator);

                if (administratorRole == null)
                {
                    administratorRole = new ApplicationRole(RoleConstants.Administrator, "Administrative role with access and permissions granted by the super user", true);
                    var roleResult = await _roleManager.CreateAsync(administratorRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded Administrator Role.");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                var adminPermisions = ClaimsPrincipalExtensions.GetRegisteredPermissions(false);
                foreach (var permission in adminPermisions)
                {
                    await ClaimsExtensions.AddPermissionClaim(_roleManager, administratorRole, permission);
                }

                var companyAdminRole = await _roleManager.FindByNameAsync(RoleConstants.CompanyAdmin);

                if (companyAdminRole == null)
                {
                    companyAdminRole = new ApplicationRole(RoleConstants.Guest, "Administrative role for company with access and permissions granted by the company owner", true);
                    var roleResult = await _roleManager.CreateAsync(companyAdminRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded CompanyAdmin Role.");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                var guestRole = await _roleManager.FindByNameAsync(RoleConstants.Guest);

                if (guestRole == null)
                {
                    guestRole = new ApplicationRole(RoleConstants.Guest, "Administrative role for company with access and permissions granted by the company owner", true);
                    var roleResult = await _roleManager.CreateAsync(guestRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded CompanyAdmin Role.");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                foreach (var permission in adminPermisions)
                {
                    await ClaimsExtensions.AddPermissionClaim(_roleManager, guestRole, permission);
                }

                var basicRole = await _roleManager.FindByNameAsync(RoleConstants.Basic);

                if (basicRole == null)
                {
                    basicRole = new ApplicationRole(RoleConstants.Basic, "Basic role with basic access and basic permissions", true);
                    var roleResult = await _roleManager.CreateAsync(basicRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded Basic Role.");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }
            }).GetAwaiter().GetResult();
        }
    }
}
