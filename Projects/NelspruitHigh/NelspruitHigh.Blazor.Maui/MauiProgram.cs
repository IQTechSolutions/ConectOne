using ConectOne.Blazor.StateManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NelspruitHigh.Blazor.Maui.Localization;
using SchoolsEnterprise.Blazor.Shared.Maui.Extensions;
using System.Globalization;
using System.Reflection;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.CloudMessaging;

#if ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#elif IOS
using Plugin.Firebase.Core.Platforms.iOS;
#endif


namespace NelspruitHigh.Blazor.Maui;

public static class MauiProgram
{
    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events => {
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
        var baseAddress = $"{builder.Configuration["ApiConfiguration:BaseApiAddress"]}/api/";

        builder.Services.AddVendorServices()
            .AddSchoolMauiServices(baseAddress)
            .ConfigureMauiLocalization()
            .ConfigureMauiAuthentication();

        builder.Services.AddSingleton<LocalizationService>();

        builder.UseMauiApp<App>().RegisterFirebaseServices().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        });

        var appSettingFileName = "SchoolsEnterprise.Blazor.Maui.wwwroot.appsettings.json";
#if DEBUG
        appSettingFileName = "SchoolsEnterprise.Blazor.Maui.wwwroot.appsettings.Development.json";
#endif

        using var appSettingsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(appSettingFileName);

        if (appSettingsStream != null)
        {
            var config = new ConfigurationBuilder().AddJsonStream(appSettingsStream).Build();
            builder.Configuration.AddConfiguration(config);
        }

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var host = builder.Build();

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