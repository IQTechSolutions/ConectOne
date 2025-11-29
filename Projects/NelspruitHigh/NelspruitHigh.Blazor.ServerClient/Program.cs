using AdvertisingModule.Infrastructure;
using Blazored.LocalStorage;
using BloggingModule.Infrastructure;
using BusinessModule.Infrastructure;
using CalendarModule.Infrastructure;
using ConectOne.Blazor.StateManagers;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.Mailing.Extensions;
using ConectOne.Domain.Providers;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
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
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using MudBlazor.Services;
using NelspruitHigh.Blazor.ServerClient;
using NelspruitHigh.Blazor.ServerClient.Components;
using NelspruitHigh.Blazor.ServerClient.Components.Account;
using NelspruitHigh.Blazor.ServerClient.DataSeeder;
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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination"));
});

builder.Services.AddBlazorDownloadFile();
builder.Services.AddMemoryCache();
builder.Services.AddCropper();
builder.Services.AddRadzenComponents();
builder.Services.AddMudServices(configuration =>
{
    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    configuration.SnackbarConfiguration.HideTransitionDuration = 100;
    configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
    configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
    configuration.SnackbarConfiguration.ShowCloseIcon = false;
});

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer(options =>
{
    // Listen to both push queues
    options.Queues = new[] { "pushnotifications", "webpush" };
    options.WorkerCount = Environment.ProcessorCount;
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, PermissionClaimsPrincipalFactory>();

builder.Services.AddAuthentication(options =>
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
        RoleClaimType = ClaimTypes.Role,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("139796117134-f89i9blu0qwerqweqredfasdrt1234rasdfasdfasdfasdrqwerqwe"))
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

builder.Services.AddAuthorization(options =>
{
    var perm = typeof(IdentityModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList();
    perm.AddRange(typeof(MessagingModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
    perm.AddRange(typeof(CalendarModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
    perm.AddRange(typeof(SchoolsModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
    perm.AddRange(typeof(BloggingModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
    perm.AddRange(typeof(ProductsModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
    perm.AddRange(typeof(BusinessModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
    perm.AddRange(typeof(FilingModule.Application.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddTransient<IDataSeeder, DataSeeder>();
builder.Services.AddScoped<GenericDbContextFactory>();
builder.Services.AddDbContextFactory<GenericDbContext>(options => options.UseSqlServer(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly(migrationAssembly)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserStore<ApplicationUser>>(provider =>
{
    var factory = provider.GetRequiredService<IDbContextFactory<GenericDbContext>>();
    return new UserStore<ApplicationUser, ApplicationRole, GenericDbContext, string, IdentityUserClaim<string>,
        IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, ApplicationRoleClaim>(
        factory.CreateDbContext());
});

builder.Services.AddScoped<IRoleStore<ApplicationRole>>(provider =>
{
    var factory = provider.GetRequiredService<IDbContextFactory<GenericDbContext>>();
    return new RoleStore<ApplicationRole, GenericDbContext, string, IdentityUserRole<string>, ApplicationRoleClaim>(
        factory.CreateDbContext());
});

var baseAddress = $"{builder.Configuration["ApiConfiguration:BaseApiAddress"]}/api/";

builder.Services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseAddress); });

builder.Services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>((sp, c) => { c.BaseAddress = new Uri(baseAddress); });

builder.Services.AddIdentityModule().AddSchoolsModule().AddMassagingModule().AddAdvertisingServices().AddBloggingServices()
    .AddCalanderModule().AddFilingModule().AddProductsModule().AddBusinessModuleServices().AddShoppingModule();

builder.Services.AddControllers().AddApplicationPart(typeof(IdentityModule.Infrastructure.AssemblyReference).Assembly)
    .AddApplicationPart(typeof(FilingModule.Infrastucture.AssemblyReference).Assembly)
    .AddApplicationPart(typeof(SchoolsModule.Infrastructure.AssemblyReference).Assembly)
    .AddApplicationPart(typeof(MessagingModule.Infrastructure.AssemblyReference).Assembly)
    .AddApplicationPart(typeof(BloggingModule.Infrastructure.AssemblyReference).Assembly)
    .AddApplicationPart(typeof(AdvertisingModule.Infrastructure.AssemblyReference).Assembly)
    .AddApplicationPart(typeof(BusinessModule.Infrastructure.AssemblyReference).Assembly)
    .AddApplicationPart(typeof(ProductsModule.Infrastructure.AssemblyReference).Assembly)
    .AddApplicationPart(typeof(ShoppingModule.Infrastructure.AssemblyReference).Assembly);

builder.Services.ConfigureEmailServices(builder.Configuration);
builder.Services.AddScoped<SchoolsEnterpriseEmailSender>();
builder.Services.AddHostedService<EmailBackgroundService>();
builder.Services.AddTransient<NotificationsEmailSender>();

builder.Services.AddScoped<SchoolEventStateManager>();

builder.Services.AddTransient<IGoogleCalendarService, GoogleCalendarService>();
builder.Services.AddScoped<IClientPreferenceManager, ClientPreferenceManager>();

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

app.UseCors("CorsPolicy");

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(ComponentAssemblyHelper.AdditionalAssemblies); ;

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
