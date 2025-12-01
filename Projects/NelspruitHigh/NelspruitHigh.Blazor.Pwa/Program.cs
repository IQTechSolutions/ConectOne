using ConectOne.Blazor.StateManagers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NelspruitHigh.Blazor.Pwa;
using SchoolsEnterprise.Blazor.Shared.Maui.Extensions;
using System.Globalization;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var baseAddress = $"{builder.Configuration["ApiConfiguration:BaseApiAddress"]}/api/";

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddVendorServices()
    .AddSchoolServices(baseAddress)
    .ConfigureLocalization()
    .ConfigureAuthentication();

var appSettingFileName = "SchoolsEnterprise.Blazor.Maui.wwwroot.appsettings.json";
#if DEBUG
appSettingFileName = "SchoolsEnterprise.Blazor.Maui.wwwroot.appsettings.Development.json";
#endif
using var appSettingsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(appSettingFileName);

if (appSettingsStream != null)
{
    var confidg = new ConfigurationBuilder().AddJsonStream(appSettingsStream).Build();
    builder.Configuration.AddConfiguration(confidg);
}


var host = builder.Build();

var storageService = host.Services.GetRequiredService<IClientPreferenceManager>();

if (storageService != null)
{
    var preference = await storageService.GetPreference();
    var cultureInfo = new CultureInfo(preference.LanguageCode);
    cultureInfo.NumberFormat.CurrencySymbol = "R";

    CultureInfo.CurrentCulture = cultureInfo;
    CultureInfo.CurrentUICulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
}

await host.RunAsync();
