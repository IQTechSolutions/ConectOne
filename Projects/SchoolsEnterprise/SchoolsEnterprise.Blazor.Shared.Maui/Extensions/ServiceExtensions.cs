using AdvertisingModule.Application;
using Blazored.LocalStorage;
using BloggingModule.Application;
using BusinessModule.Application;
using CalendarModule.Application;
using CalendarModule.Application.Extensions;
using ConectOne.Blazor.Services;
using ConectOne.Blazor.StateManagers;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.Providers;
using Cropper.Blazor.Extensions;
using FilingModule.Application;
using IdentityModule.Application;
using IdentityModule.Blazor.Implimentation;
using IdentityModule.Blazor.StateManagers;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using MessagingModule.Application;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using NeuralTech.Services;
using ProductsModule.Application;
using Radzen;
using SchoolsEnterprise.Blazor.Shared.Maui.Handler;
using SchoolsEnterprise.Blazor.Shared.Maui.Services;
using SchoolsModule.Application;
using SchoolsModule.Application.Extensions;
using ShoppingModule.Application;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Extensions
{
    /// <summary>
    /// Provides extension methods for registering application-specific and third-party services with an <see
    /// cref="IServiceCollection"/> in a .NET application.
    /// </summary>
    /// <remarks>These extension methods are intended to be used during application startup to configure
    /// dependency injection for vendor components, localization, authentication, and domain-specific services. Each
    /// method encapsulates a set of related service registrations to simplify and standardize service configuration
    /// across the application.</remarks>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds and configures vendor-provided services required for the application, including memory caching, UI
        /// components, and local storage support.
        /// </summary>
        /// <remarks>This method registers services for Radzen components, Blazored local storage,
        /// MudBlazor UI components with custom snackbar configuration, and in-memory caching. Call this method during
        /// application startup to ensure all required vendor services are available for dependency injection.</remarks>
        /// <param name="services">The service collection to which the vendor services will be added. Cannot be null.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
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
        /// Adds school-related services to the specified <see cref="IServiceCollection"/> for dependency injection.
        /// </summary>
        /// <remarks>This method registers implementations for <see cref="IDeviceInfoService"/> and <see
        /// cref="IClientPreferenceManager"/> with scoped lifetimes. Call this method during application startup to make
        /// these services available for dependency injection.</remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the school services will be added. Cannot be null.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
        public static IServiceCollection AddSchoolServices(this IServiceCollection services, string baseApiAddress)
        {
            services.AddScoped<IDeviceInfoService, DeviceInfoService>();
            services.AddScoped<IClientPreferenceManager, ClientPreferenceManager>();
            services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseApiAddress); c.EnableIntercept(sp); }).AddHttpMessageHandler<AuthenticationHeaderHandler>();
            services.AddHttpClient<IAccountsProvider, AccountsClient>((sp, c) => { c.BaseAddress = new Uri(baseApiAddress); c.EnableIntercept(sp); }).AddHttpMessageHandler<AuthenticationHeaderHandler>();
            
            services.AddFilingModule().AddBloggingServices(baseApiAddress).AddMessagingModule(baseApiAddress).AddSchoolsModule(baseApiAddress).AddAdvertisingServices(baseApiAddress)
                .AddShoppingModule(baseApiAddress).AddBusinessModuleServices(baseApiAddress).AddCalanderModule(baseApiAddress).AddProductsModule(baseApiAddress).AddIdentityModule(baseApiAddress);
            
            return services;
        }
        
        /// <summary>
        /// Configures localization services and sets the resources path for localization to "Resources".
        /// </summary>
        /// <param name="services">The service collection to which localization services will be added. Cannot be null.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddScoped<CultureService>();

            return services;
        }

        /// <summary>
        /// Configures authentication services and adds support for cascading authentication state to the specified
        /// service collection.
        /// </summary>
        /// <param name="services">The service collection to which authentication services will be added. Cannot be null.</param>
        /// <returns>The same instance of <see cref="IServiceCollection"/> with authentication services configured.</returns>
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddCascadingAuthenticationState();
            services.AddAuthorizationCore(options =>
            {
                options.RegisterIdentityPermissionClaims().RegisterSchoolModulePermissionClaims().RegisterCalendarModulePermissionClaims();
            });
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            services.AddTransient<AuthenticationHeaderHandler>();

            services.AddHttpClientInterceptor();
            services.AddScoped<HttpInterceptorService>();
            
            return services;
        }

        public static IServiceCollection AddSchoolMauiServices(this IServiceCollection services, string baseApiAddress)
        {
            services.AddScoped<IDeviceInfoService, DeviceInfoService>();
            services.AddScoped<IClientPreferenceManager, ClientPreferenceManager>();

            #if DEBUG
                var handler = new HttpsClientHandlerService();
                services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseApiAddress); c.EnableIntercept(sp); }).ConfigurePrimaryHttpMessageHandler(() => handler.GetPlatformMessageHandler()).AddHttpMessageHandler<MauiAuthenticationHeaderHandler>();
                services.AddHttpClient<IAccountsProvider, AccountsClient>((sp, c) => { c.BaseAddress = new Uri(baseApiAddress); c.EnableIntercept(sp); }).ConfigurePrimaryHttpMessageHandler(() => handler.GetPlatformMessageHandler()).AddHttpMessageHandler<MauiAuthenticationHeaderHandler>();
#else
                services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseApiAddress); c.EnableIntercept(sp); }).AddHttpMessageHandler<MauiAuthenticationHeaderHandler>();
            services.AddHttpClient<IAccountsProvider, AccountsClient>((sp, c) => { c.BaseAddress = new Uri(baseApiAddress); c.EnableIntercept(sp); }).AddHttpMessageHandler<MauiAuthenticationHeaderHandler>();
#endif

            services.AddFilingModule().AddBloggingServices(baseApiAddress).AddMessagingModule(baseApiAddress).AddSchoolsModule(baseApiAddress).AddAdvertisingServices(baseApiAddress)
                .AddShoppingModule(baseApiAddress).AddBusinessModuleServices(baseApiAddress).AddCalanderModule(baseApiAddress).AddProductsModule(baseApiAddress).AddIdentityModule(baseApiAddress);

            return services;
        }

        public static IServiceCollection ConfigureMauiAuthentication(this IServiceCollection services)
        {
            services.AddScoped<MauiAuthenticationHeaderHandler>();
            services.AddScoped<MauiAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<MauiAuthenticationStateProvider>());

            services.AddAuthorizationCore(options =>
            {
                options.RegisterIdentityPermissionClaims().RegisterSchoolModulePermissionClaims().RegisterCalendarModulePermissionClaims();
            });

            services.AddHttpClientInterceptor();
            services.AddScoped<HttpInterceptorService>();

            return services;
        }

        public static IServiceCollection ConfigureMauiLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources/Localization");
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            
            return services;
        }
    }
}
