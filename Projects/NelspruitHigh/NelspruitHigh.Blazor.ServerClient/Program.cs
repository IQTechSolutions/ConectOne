using ConectOne.Domain.Interfaces;
using ConectOne.Domain.Providers;
using ConectOne.EntityFrameworkCore.Sql;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using NelspruitHigh.Blazor.ServerClient;
using NelspruitHigh.Blazor.ServerClient.Components;
using NelspruitHigh.Blazor.ServerClient.Components.Account;
using NelspruitHigh.Blazor.ServerClient.DataSeeder;
using System.Reflection;
using SchoolsEnterprise.Blazor.Shared.Extensions;

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

var baseAddress = $"{builder.Configuration["ApiConfiguration:BaseApiAddress"]}/api/";

builder.Services.AddHttpClient<IBaseHttpProvider, BaseHttpProvider>(( c) => { c.BaseAddress = new Uri(baseAddress); });

builder.Services.AddVendorServices().AddSchoolServices().ConfigureHangfire(builder.Configuration).ConfigureLocalization().ConfigureAuthentication(builder.Configuration).ConfigureAppEmailServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder2 =>
        builder2.AllowAnyOrigin()
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

app.UseCors("CorsPolicy");

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(ComponentAssemblyHelper.AdditionalAssemblies); 

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
