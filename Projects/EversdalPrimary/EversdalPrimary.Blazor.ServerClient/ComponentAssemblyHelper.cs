using System.Reflection;

namespace EversdalPrimary.Blazor.ServerClient;

/// <summary>
/// Provides helper functionality for accessing additional assemblies required by application components.
/// </summary>
/// <remarks>This class is intended for internal use to aggregate assemblies from various modules that may be
/// needed for component discovery or registration. It is not intended to be used directly by application
/// code.</remarks>
internal static class ComponentAssemblyHelper
{
    /// <summary>
    /// Provides a collection of additional assemblies to be included for application configuration or discovery
    /// purposes.
    /// </summary>
    /// <remarks>This array can be used to register or scan for types, resources, or services defined in the
    /// specified assemblies. The set of assemblies may be used by frameworks or libraries that require explicit
    /// assembly references for reflection, dependency injection, or module loading.</remarks>
    public static readonly Assembly[] AdditionalAssemblies = new[]
    {
        typeof(AdvertisingModule.Blazor.AssemblyReference).Assembly,
        typeof(BloggingModule.Blazor.AssemblyReference).Assembly,
        typeof(BusinessModule.Blazor.AssemblyReference).Assembly,
        typeof(CalendarModule.Blazor.AssemblyReference).Assembly,
        typeof(FilingModule.Blazor.AssemblyReference).Assembly,
        typeof(IdentityModule.Blazor.AssemblyReference).Assembly,
        typeof(MessagingModule.Blazor.AssemblyReference).Assembly,
        typeof(ProductsModule.Blazor.AssemblyReference).Assembly,
        typeof(SchoolsModule.Blazor.AssemblyReference).Assembly,
        typeof(ShoppingModule.Blazor.AssemblyReference).Assembly,
        typeof(SchoolsEnterprise.Blazor.Shared.AssemblyReference).Assembly,
    };
}
