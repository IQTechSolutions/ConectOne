using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages
{
    /// <summary>
    /// Represents the Privacy Policy page component.
    /// This component displays the privacy policy information of the application.
    /// </summary>
    public partial class PrivacyPolicy
    {
        /// <summary>
        /// Gets the application name from the configuration settings.
        /// </summary>
        private string ApplicationName => Configuration.GetValue<string>("CompanyConfiguration:SchoolName");

        /// <summary>
        /// Gets the application name from the configuration settings.
        /// </summary>
        private string SchoolType => Configuration.GetValue<string>("CompanyConfiguration:SchoolTypeNameRight");

        /// <summary>
        /// Gets the information email address from the configuration settings.
        /// </summary>
        private string InfoEmailAddress => Configuration.GetValue<string>("EmailConfiguration:InfoEmailAddress");

        /// <summary>
        /// A list of breadcrumb items to display the navigation path.
        /// </summary>
        private readonly List<BreadcrumbItem> _items = new()
        {
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Privacy Policy", href: null, disabled: true, icon: Icons.Material.Filled.Policy)
        };
    }
}