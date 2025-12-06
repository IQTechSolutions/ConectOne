using System.Net;
using AccomodationModule.Infrastructure;
using BytexDigital.Blazor.Components.CookieConsent;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.Mailing.Extensions;
using ConectOne.EntityFrameworkCore.Sql;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Cropper.Blazor.Extensions;
using FilingModule.Domain.Implementation;
using FilingModule.Domain.Interfaces;
using FilingModule.Infrastucture.Implementation;
using GroupingModule.Domain.Interfaces;
using GroupingModule.Infrastructure.Implementation;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Infrastructure.Implimentation;
using KiburiOnline.Blazor.ServerClient;
using KiburiOnline.Blazor.ServerClient.Components;
using KiburiOnline.Blazor.ServerClient.Components.Account;
using KiburiOnline.Blazor.ServerClient.DataSeeder;
using KiburiOnline.Blazor.Shared.Mails;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using Radzen;
using Recapcha.Services;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using ConectOne.Domain.ResultWrappers;
using LocationModule.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ProductsModule.Infrastructure;
using AssemblyReference = AccomodationModule.Infrastructure.AssemblyReference;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();

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

builder.Services.AddAccommodationModuleServices().AddProductsModule().AddLocationModuleServices();

builder.Services.AddCookieConsent(o =>
{
    o.Revision = 1;
    o.PolicyUrl = "/cookie-policy";

    // Call optional
    o.UseDefaultConsentPrompt(prompt =>
    {
        prompt.Position = ConsentModalPosition.BottomRight;
        prompt.Layout = ConsentModalLayout.Bar;
        prompt.SecondaryActionOpensSettings = false;
        prompt.AcceptAllButtonDisplaysFirst = false;
    });

    o.Categories.Add(new CookieCategory
    {
        TitleText = new()
        {
            ["en"] = "Google Services",
            ["de"] = "Google Dienste"
        },
        DescriptionText = new()
        {
            ["en"] = "Allows the integration and usage of Google services.",
            ["de"] = "Erlaubt die Verwendung von Google Diensten."
        },
        Identifier = "google",
        IsPreselected = true,

        Services = new()
        {
            new CookieCategoryService
            {
                Identifier = "google-maps",
                PolicyUrl = "https://policies.google.com/privacy",
                TitleText = new()
                {
                    ["en"] = "Google Maps",
                    ["de"] = "Google Maps"
                },
                ShowPolicyText = new()
                {
                    ["en"] = "Display policies",
                    ["de"] = "Richtlinien anzeigen"
                }
            },
            new CookieCategoryService
            {
                Identifier = "google-analytics",
                PolicyUrl = "https://policies.google.com/privacy",
                TitleText = new()
                {
                    ["en"] = "Google Analytics",
                    ["de"] = "Google Analytics"
                },
                ShowPolicyText = new()
                {
                    ["en"] = "Display policies",
                    ["de"] = "Richtlinien anzeigen"
                }
            }
        }
    });
});

builder.Services.AddMudServices(configuration =>
{
    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    configuration.SnackbarConfiguration.HideTransitionDuration = 100;
    configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
    configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
    configuration.SnackbarConfiguration.ShowCloseIcon = false;
});
builder.Services.AddScoped<IAuditTrailsService, AuditTrialsService>();

builder.Services.AddScoped<IImageProcessingService, ImageProcessingService>();
builder.Services.AddScoped<IVideoProcessingService, VideoProcessingService>();
builder.Services.AddCropper();

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
                    ValidIssuer = builder.Configuration["JWTSettings:validIssuer"],
                    ValidAudience = builder.Configuration["JWTSettings:validAudience"],
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:securityKey"]))
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


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

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

builder.Services.AddAuthorization(options =>
{
    var perm = typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList();
    perm.AddRange(typeof(AccomodationModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
    perm.AddRange(typeof(FilingModule.Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList());
    foreach (var prop in perm)
    {
        var propertyValue = prop.GetValue(null);
        if (propertyValue is not null)
        {
            options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
        }
    }
});

builder.Services.AddControllers().AddApplicationPart(typeof(FilingModule.Infrastucture.AssemblyReference).Assembly)
    .AddApplicationPart(typeof(MessagingModule.Infrastructure.MessagingModule).Assembly)
    .AddApplicationPart(typeof(AssemblyReference).Assembly);

builder.Services.AddScoped<IRoleStore<ApplicationRole>>(provider =>
{
    var factory = provider.GetRequiredService<IDbContextFactory<GenericDbContext>>();
    return new RoleStore<ApplicationRole, GenericDbContext, string, IdentityUserRole<string>, ApplicationRoleClaim>(
        factory.CreateDbContext());
});

builder.Services.AddScoped<GooglereCAPTCHAv3Service>();
builder.Services.AddScoped<GenericDbContextFactory>();

builder.Services.AddScoped<IDataSeeder, DataSeeder>();
builder.Services.AddScoped<ISitemapGenerator, SitemapGenerator>();

builder.Services.ConfigureEmailServices(builder.Configuration);

builder.Services.AddScoped<ProGolfEmailSender>();
builder.Services.AddScoped<ProGolfEmailTemplateSender>();

builder.Services.AddScoped(typeof(ICategoryService<>), typeof(CategoryService<>));

builder.Services.AddScoped<IAuditTrailsService, AuditTrialsService>();
builder.Services.AddScoped<IExcelService, ExcelService>();

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
app.MapRazorPages();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(KiburiOnline.Blazor.Shared.AssemblyReference).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
