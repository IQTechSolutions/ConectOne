using ConectOne.Blazor.StateManagers;
using EversdalPrimary.Blazor.Maui.Localization;
using EversdalPrimary.Blazor.Maui.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolsEnterprise.Blazor.Shared.Maui.Extensions;
using System.Globalization;
using System.Reflection;
using CommunityToolkit.Maui;
using Plugin.Firebase.CloudMessaging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.AspNetCore.SignalR;

using static ProductsModule.Domain.Constants.Permissions;
using IdentityModule.Domain.Interfaces;
using EversdalPrimary.Blazor.Maui.Handler;
using ConectOne.Domain.Interfaces;
using IdentityModule.Blazor.StateManagers;
using IdentityModule.Blazor.Implimentation;

using ConectOne.Domain.Providers;
using Toolbelt.Blazor.Extensions.DependencyInjection;


#if ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#elif IOS
using Plugin.Firebase.Core.Platforms.iOS;
#endif

namespace EversdalPrimary.Blazor.Maui
{
    public static class MauiProgram
    {
        public static IServiceProvider? Services { get; private set; }

        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
#if IOS
         events.AddiOS(iOS => iOS.WillFinishLaunching((_, __) => {
            CrossFirebase.Initialize();
            FirebaseCloudMessagingImplementation.Initialize();
            return false;
        }));
#elif ANDROID
                events.AddAndroid(android => android.OnCreate((activity, _) =>
                    CrossFirebase.Initialize(activity)));
#endif
            });

            return builder;
        }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            var appSettingFileName = "EversdalPrimary.Blazor.Maui.wwwroot.appsettings.json";
#if DEBUG
            appSettingFileName = "EversdalPrimary.Blazor.Maui.wwwroot.appsettings.Development.json";
#endif

            using var appSettingsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(appSettingFileName);

            if (appSettingsStream != null)
            {
                var config = new ConfigurationBuilder().AddJsonStream(appSettingsStream).Build();
                builder.Configuration.AddConfiguration(config);
            }

            var baseAddress = $"{builder.Configuration["ApiConfiguration:BaseApiAddress"]}/api/";

            builder.Services.AddVendorServices()
                .AddSchoolMauiServices(baseAddress)
                .ConfigureMauiLocalization()
                .ConfigureMauiAuthentication();

#if DEBUG
            var handler = new HttpsClientHandlerService();
            builder.Services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseAddress); c.EnableIntercept(sp); }).ConfigurePrimaryHttpMessageHandler(() => handler.GetPlatformMessageHandler()).AddHttpMessageHandler<MauiAuthenticationHeaderHandler>();
            builder.Services.AddHttpClient<IAccountsProvider, MauiAccountsClient>((sp, c) => { c.BaseAddress = new Uri(baseAddress); c.EnableIntercept(sp); }).ConfigurePrimaryHttpMessageHandler(() => handler.GetPlatformMessageHandler()).AddHttpMessageHandler<MauiAuthenticationHeaderHandler>();
#else
                services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseApiAddress); c.EnableIntercept(sp); }).AddHttpMessageHandler<MauiAuthenticationHeaderHandler>();
                services.AddHttpClient<IAccountsProvider, AccountsClient>((sp, c) => { c.BaseAddress = new Uri(baseApiAddress); c.EnableIntercept(sp); }).AddHttpMessageHandler<MauiAuthenticationHeaderHandler>();
#endif

            builder.Services.AddSingleton<LocalizationService>();
            builder.Services.AddSingleton<NotificationNavigationService>();

            builder.UseMauiApp<App>().RegisterFirebaseServices().UseMauiCommunityToolkit().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var host = builder.Build();
            Services = host.Services;

            var storageService = host.Services.GetService<ClientPreferenceManager>();

            if (storageService != null)
            {
                Task.Run(async () =>
                {
                    var preference = await storageService.GetPreference();
                    var cultureInfo = new CultureInfo(preference.LanguageCode);
                    cultureInfo.NumberFormat.CurrencySymbol = "R";

                    CultureInfo.CurrentCulture = cultureInfo;
                    CultureInfo.CurrentUICulture = cultureInfo;
                    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

                }).Wait();

            }

            return host;
        }
    }
}
