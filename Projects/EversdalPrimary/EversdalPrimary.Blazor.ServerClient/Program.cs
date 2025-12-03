using ConectOne.Domain.Interfaces;
using ConectOne.Domain.Providers;
using ConectOne.EntityFrameworkCore.Sql;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using EversdalPrimary.Blazor.ServerClient;
using EversdalPrimary.Blazor.ServerClient.Components;
using EversdalPrimary.Blazor.ServerClient.Components.Account;
using EversdalPrimary.Blazor.ServerClient.DataSeeder;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using IdentityModule.Domain.Entities;
using MessagingModule.Infrastructure.Hubs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolsEnterprise.Blazor.Shared.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });

builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddTransient<IDataSeeder, DataSeeder>();
builder.Services.AddScoped<GenericDbContextFactory>();
builder.Services.AddDbContextFactory<GenericDbContext>(options => options.UseSqlServer(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly(migrationAssembly)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
});
builder.Services.AddSignalRCore();

var baseAddress = $"{builder.Configuration["ApiConfiguration:BaseApiAddress"]}/api/";

builder.Services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseAddress); });

builder.Services.AddVendorServices().AddSchoolServices().ConfigureHangfire(builder.Configuration).ConfigureLocalization().ConfigureAuthentication().ConfigureAppEmailServices(builder.Configuration);

var json = @"
{
  ""type"": ""service_account"",
  ""project_id"": ""eversdal-primary"",
  ""private_key_id"": ""0ce1aba5a0dfd5c3f41509de17d240d879b944c6"",
  ""private_key"": ""-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDFnWcXahwo6Omd
9V9SnOLZnbTxWKvnEg4c6Y1lx4PccryRC5YjgwBMHk1hOZsXkMoFws95lELTGWDM
C1KgLW9i4wno3P80BBFDdppVlFxiYLis2aHCPQu4oVDxaGA1NWuAa7/nYs8A6KPw
0LwYKfqYr7pte2oYeOFsShCQ99NWwg7+e2YkWI3XJkoErDTQtB1+ovhsH4QJFwww
Y9uypUGrBgnild2+j3/L91Zcp7sqoTnxgffArwP5S2J8BHEqybjKgJbL16MkmKdL
08zZ8B9joS/YllX05WMZrgMgF8SdGB2Y7UZJbfh4iKeb9b9Vw9HpnjxZ5vmSRhzi
X5bfvIZTAgMBAAECggEACobn3X2dEhQqPGxm+gx6dyjpYSvzPa7JYAKUXODOe5lZ
xd5DTrv6nD4cVXq58ieX43ZXvWcc/N2EDwqRXxAHOCeBDnhrWIwuQT+Zrse6+NCk
iyytax6mYlz8O11EjBZpliCR9f5eVEg6/NKZxS3DTYvwQfHKRT3MlbYHq46ldvXC
f/CEKomQZfZOv4YaedMdl6T6u6mdS4faHaL28F9tiMBQbBhtTlINw4BfqlV7ejyx
7WUNuR54LPvaLi9MfcmrVLcc8joi4aKMoIOR8xp1RXASmPN/Wtu7Q8nx8sBXaYeU
I8m3r+QfuTHMiqAaV9kjOKXsvi53mI6HcMo9xxa7XQKBgQDreoH//NQIsH7o7c7I
lEMCCsv6seq5UG/2mugjmznE/Y7vRkW14XIkn4W9v5Z+n5aib3CLW+c0FIIHUBFg
BanIIPWadPTvAiq2LGDxXSLH+3lOS0x9MW5UUtGO95lJiwcW0sa4UnkO2ALaGwb/
qCwbnT+wkHb2zBKW9QHK3Ny+JwKBgQDW1ilSxHWNwLzDbBU2kLm+n+3XfRk8ZLSz
SeaWycA8igjXut2ApUxHo8+/ysbGCpUoRndQ7pLyCyKPSvsewNMoNFUT7RE8ByTy
/PIVgGoHKDaLR6T4jK7KRlnNe0v1Qa5y94kC5wvRtgSxDtYD3Us52iNdvcmVwh9V
bx55KcL99QKBgQDBrFrrvnhuRSu8TIs8saSDM3odUOPrUtsjirjPQEY9XQaIidWg
CDKnKIUJUWBn+L4eD2YTWJ1KWmuvtAz2WvIPZfxa80LYqYGTn+CD48RNkulsmcXp
WhSSSiONYNu0fyJvhSBoTXy/1q5R0NTqxNiyRZ0yjT8rMSFBA3Zb8VelFQKBgD4C
KoM9OUDk9JbjR2wscbBXsOqVrQcFejKVjfzP1rCiltLNmMlc7Jtw6LHO8XiP7WVm
kWHGupsqI5uAZOMHfOsJz9SkhGjzFvMblzQlkHToYnXpA/DJilnP3HO2tsHfZ16/
m8g+r+ajxs7kHVmYMfJMyBByixCyqyCZpg4Kw8FRAoGAO+vCD7kbMzvxRg0oyulp
Uf60MZ8ZAPKC2xhnU2N27OWLpnhApMGRx1rV/SkFs1bKHWghkcvxg7IXUn9W2SPh
f8irBvs0dVO1/VjteZqi4moQPxCp/xZtryhDMVfGDMo2rh6/P8vs4Fzuh5vo1TNY
+P6qF7LlKWPMtOCEOCYkjYA=
-----END PRIVATE KEY-----
"",
  ""client_email"": ""firebase-adminsdk-fbsvc@eversdal-primary.iam.gserviceaccount.com"",
  ""client_id"": ""101690637598940830677"",
  ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
  ""token_uri"": ""https://oauth2.googleapis.com/token"",
  ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
  ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-fbsvc%40eversdal-primary.iam.gserviceaccount.com"",
  ""universe_domain"": ""googleapis.com""
}
";


FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson(json)
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination"));
});

var app = builder.Build();

using var serviceScope = app.Services.CreateScope();

var initializers = serviceScope.ServiceProvider.GetServices<IDataSeeder>();

foreach (var initializer in initializers)
{
    initializer.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() } // optional auth
});

app.UseCors("CorsPolicy");

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapHub<NotificationsHub>("/notificationsHub");
app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(ComponentAssemblyHelper.AdditionalAssemblies); ;

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
