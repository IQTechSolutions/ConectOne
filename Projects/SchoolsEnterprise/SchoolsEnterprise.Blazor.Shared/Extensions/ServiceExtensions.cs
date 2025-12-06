

using AdvertisingModule.Infrastructure;
using Blazored.LocalStorage;
using BloggingModule.Infrastructure;
using BusinessModule.Infrastructure;
using CalendarModule.Infrastructure;
using ConectOne.Blazor.StateManagers;
using ConectOne.Domain.Mailing.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql;
using Cropper.Blazor.Extensions;
using FilingModule.Blazor.Extensions;
using FilingModule.Infrastucture;
using GoogleCalendar.Contracts;
using GoogleCalendar.Service;
using Hangfire;
using Hangfire.SqlServer;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Security;
using IdentityModule.Infrastructure;
using MessagingModule.Infrastructure;
using MessagingModule.Infrastructure.Mails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using MudBlazor.Services;
using Newtonsoft.Json;
using ProductsModule.Infrastructure;
using Radzen;
using SchoolsEnterprise.Base;
using SchoolsEnterprise.Base.Mails;
using SchoolsModule.Blazor.StateManagers;
using SchoolsModule.Infrastructure;
using ShoppingModule.Infrastructure;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Permissions = FilingModule.Domain.Constants.Permissions;

namespace SchoolsEnterprise.Blazor.Shared.Extensions
{
    /// <summary>
    /// Provides extension methods for registering and configuring application services, authentication, localization,
    /// background processing, and vendor integrations within an ASP.NET Core application's dependency injection
    /// container.
    /// </summary>
    /// <remarks>These extension methods are intended to be called during application startup to add and
    /// configure required services for the application's operation. They group related service registrations for
    /// modularity and maintainability, and should be invoked on the IServiceCollection instance in the application's
    /// Startup or Program class.</remarks>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds and configures third-party vendor services required for the application to the specified service
        /// collection.
        /// </summary>
        /// <remarks>This method registers services such as memory caching, image cropping, UI components,
        /// and local storage support. It also configures MudBlazor's snackbar notifications with predefined settings.
        /// Call this method during application startup to ensure all required vendor services are available for
        /// dependency injection.</remarks>
        /// <param name="services">The service collection to which the vendor services will be added. Cannot be null.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance that was provided, with vendor services registered.</returns>
        public static IServiceCollection AddVendorServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddCropper();
            services.AddRadzenComponents();
            services.AddBlazoredLocalStorage();
            services.AddMudServices(configuration =>
            {
                configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                configuration.SnackbarConfiguration.HideTransitionDuration = 100;
                configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
                configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
                configuration.SnackbarConfiguration.ShowCloseIcon = false;
            });
            return services;
        }

        /// <summary>
        /// Adds and configures all core school-related services and modules to the specified service collection.
        /// </summary>
        /// <remarks>This method registers services for identity, schools, messaging, advertising,
        /// blogging, calendar, filing, products, business, and shopping modules, as well as related controllers and
        /// application parts. It should be called during application startup to ensure all required school-related
        /// features are available.</remarks>
        /// <param name="services">The service collection to which the school services and related modules will be added. Must not be null.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance that was provided, with school services and modules
        /// registered.</returns>
        public static IServiceCollection AddSchoolServices(this IServiceCollection services)
        {
            services.AddBlazorDownloadFile();
            services.AddIdentityModule().AddSchoolsModule().AddMassagingModule().AddAdvertisingServices().AddBloggingServices()
                .AddCalanderModule().AddFilingModule().AddProductsModule().AddBusinessModuleServices().AddShoppingModule();

            services.AddControllers().AddApplicationPart(typeof(IdentityModule.Infrastructure.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(FilingModule.Infrastucture.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(SchoolsModule.Infrastructure.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(MessagingModule.Infrastructure.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(BloggingModule.Infrastructure.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(AdvertisingModule.Infrastructure.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(BusinessModule.Infrastructure.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(ProductsModule.Infrastructure.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(ShoppingModule.Infrastructure.AssemblyReference).Assembly);

            services.AddScoped<SchoolEventStateManager>();
            services.AddTransient<IGoogleCalendarService, GoogleCalendarService>();
            services.AddScoped<IClientPreferenceManager, ClientPreferenceManager>();

            return services;
        }

        /// <summary>
        /// Configures Hangfire services and server for background job processing using SQL Server storage.
        /// </summary>
        /// <remarks>This method sets up Hangfire with recommended serializer settings and SQL Server
        /// storage, and configures the Hangfire server to process jobs from the 'pushnotifications' and 'webpush'
        /// queues. The worker count is set to match the number of processor cores.</remarks>
        /// <param name="services">The service collection to which Hangfire services will be added. Must not be null.</param>
        /// <param name="configuration">The application configuration used to retrieve the SQL Server connection string. Must not be null.</param>
        /// <returns>The same service collection instance with Hangfire services configured. This enables further chaining of
        /// service configuration calls.</returns>
        public static IServiceCollection ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer(options =>
            {
                // Listen to both push queues
                options.Queues = new[] { "pushnotifications", "webpush" };
                options.WorkerCount = Environment.ProcessorCount;
            });

            return services;
        }

        /// <summary>
        /// Configures localization services for the application and sets the resources path to "Resources".
        /// </summary>
        /// <remarks>This method enables localization support by registering the necessary services and
        /// specifying the directory where resource files are located. Call this method during application startup to
        /// prepare the application for localization.</remarks>
        /// <param name="services">The service collection to which localization services will be added. Cannot be null.</param>
        /// <returns>The same instance of <see cref="IServiceCollection"/> with localization services configured.</returns>
        public static IServiceCollection ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            return services;
        }

        /// <summary>
        /// Configures authentication, authorization, and identity services for the application using JWT bearer tokens
        /// and custom permission policies.
        /// </summary>
        /// <remarks>This method sets up JWT bearer authentication with custom token validation parameters
        /// and event handlers for handling authentication failures and SignalR connections. It also registers
        /// permission-based authorization policies by scanning permission constants from multiple modules, and
        /// configures ASP.NET Core Identity with custom user and role stores. Call this method during application
        /// startup to enable authentication and authorization features.</remarks>
        /// <param name="services">The service collection to which authentication, authorization, and identity services will be added. Cannot
        /// be null.</param>
        /// <returns>The same service collection instance with authentication, authorization, and identity services configured.</returns>
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCascadingAuthenticationState();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, PermissionClaimsPrincipalFactory>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidIssuer = configuration["JWTSettings:validIssuer"],
                    ValidAudience = configuration["JWTSettings:validAudience"],
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:securityKey"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        var isSignalRRequest = path.StartsWithSegments("/signalRHub")
                            || path.StartsWithSegments("/notificationsHub");

                        if (!string.IsNullOrEmpty(accessToken) && isSignalRRequest)
                        {
                            // Allows using JWT tokens passed as a query string for SignalR connections
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = c =>
                    {
                        // Custom logic for handling token expiration and other auth failures
                        if (c.Exception is SecurityTokenExpiredException)
                        {
                            c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            c.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("The Token is expired."));
                            return c.Response.WriteAsync(result);
                        }
                        else
                        {
#if DEBUG
                            c.NoResult();
                            c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
#else
                                c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                c.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject("An unhandled error has occurred.");
                                return c.Response.WriteAsync(result);
#endif
                        }
                    },
                    OnChallenge = context =>
                    {
                        // If not authorized, return a clear JSON response
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("You are not Authorized."));
                            return context.Response.WriteAsync(result);
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(Result.Fail("You are not authorized to access this resource."));
                        return context.Response.WriteAsync(result);
                    },
                };
            });

            services.AddAuthorization(options =>
            {
                var perm = typeof(IdentityModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList();
                perm.AddRange(typeof(MessagingModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
                perm.AddRange(typeof(CalendarModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
                perm.AddRange(typeof(SchoolsModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
                perm.AddRange(typeof(BloggingModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
                perm.AddRange(typeof(ProductsModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
                perm.AddRange(typeof(BusinessModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
                perm.AddRange(typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
                perm.AddRange(typeof(AdvertisingModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());

                foreach (var prop in perm)
                {
                    var propertyValue = prop.GetValue(null);
                    if (propertyValue is not null)
                    {
                        options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
                    }
                }
            });

           services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
                })
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserStore<ApplicationUser>>(provider =>
            {
                var factory = provider.GetRequiredService<IDbContextFactory<GenericDbContext>>();
                return new UserStore<ApplicationUser, ApplicationRole, GenericDbContext, string, IdentityUserClaim<string>,
                    IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, ApplicationRoleClaim>(
                    factory.CreateDbContext());
            });

            services.AddScoped<IRoleStore<ApplicationRole>>(provider =>
            {
                var factory = provider.GetRequiredService<IDbContextFactory<GenericDbContext>>();
                return new RoleStore<ApplicationRole, GenericDbContext, string, IdentityUserRole<string>, ApplicationRoleClaim>(
                    factory.CreateDbContext());
            });

            return services;
        }

        /// <summary>
        /// Configures application email-related services and dependencies for use with dependency injection.
        /// </summary>
        /// <remarks>This method registers core email sending services, background processing, and related
        /// dependencies required for email functionality within the application. Call this method during application
        /// startup to ensure email features are available throughout the app.</remarks>
        /// <param name="services">The service collection to which the email services will be added. Cannot be null.</param>
        /// <param name="configuration">The application configuration containing settings required for email services. Cannot be null.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance that was provided, with email services registered.</returns>
        public static IServiceCollection ConfigureAppEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureEmailServices(configuration);
            services.AddScoped<SchoolsEnterpriseEmailSender>();
            services.AddHostedService<EmailBackgroundService>();
            services.AddTransient<NotificationsEmailSender>();

            return services;
        }

    }
}
