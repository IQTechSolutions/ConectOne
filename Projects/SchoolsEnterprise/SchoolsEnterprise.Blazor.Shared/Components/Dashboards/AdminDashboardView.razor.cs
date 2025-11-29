using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Components.Dashboards
{
    /// <summary>
    /// Represents the administrative dashboard view, providing role-based user statistics and overall user count.
    /// </summary>
    /// <remarks>This component initializes by fetching role data and user statistics from the server. It
    /// retrieves the list of roles, the number of users associated with each role, and the total user count. The data
    /// is fetched asynchronously using the injected HTTP provider.</remarks>
    public partial class AdminDashboardView
    {
        private List<RoleDto> _useRoles = new List<RoleDto>();
        private Dictionary<string, int> _roleUserCountCollection = new Dictionary<string, int>();
        private int _userCount;
        private readonly List<string> _monthLabels = new() { "Jan", "Feb", "Mar", "Apr" };
        private readonly double[] _incomeChart = { 32000, 34000, 36500, 38200 };
        private readonly double[] _clothingChart = { 45, 30, 15, 10 };
        private readonly double[] _supportersChart = { 50, 28, 12, 10 };
        private readonly string[] _pieLabels = { "Uniforms", "Sports", "Accessories", "Other" };

        /// <summary>
        /// Gets or sets the service used to manage and query user roles within the application.
        /// </summary>
        [Inject] private IRoleService RoleService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations within the component.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be set manually
        /// in most scenarios.</remarks>
        [Inject] private IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Represents a sales record for a specific month, including revenue from clothing shop, supporters gear, and
        /// advertising.
        /// </summary>
        /// <param name="Month">The name of the month to which the sales record applies.</param>
        /// <param name="ClothingShop">The total revenue, in the local currency, generated from the clothing shop for the specified month.</param>
        /// <param name="SupportersGear">The total revenue, in the local currency, generated from supporters gear sales for the specified month.</param>
        /// <param name="Advertising">The total revenue, in the local currency, generated from advertising for the specified month.</param>
        private record SalesRecord(string Month, decimal ClothingShop, decimal SupportersGear, decimal Advertising);

        /// <summary>
        /// Represents a learner with identifying and academic information.
        /// </summary>
        /// <param name="Id">The unique identifier for the learner.</param>
        /// <param name="FirstName">The first name of the learner. Cannot be null.</param>
        /// <param name="LastName">The last name of the learner. Cannot be null.</param>
        /// <param name="Gender">The gender of the learner. Cannot be null.</param>
        /// <param name="Grade">The grade or class designation of the learner. Cannot be null.</param>
        /// <param name="AcademicAverage">The learner's academic average as a decimal value.</param>
        private record LearnerItem(int Id, string FirstName, string LastName, string Gender, string Grade, decimal AcademicAverage);

        /// <summary>
        /// Represents a statistical data item with a label, count, color, and percentage value.
        /// </summary>
        /// <param name="Label">The display label or name associated with the statistical item.</param>
        /// <param name="Count">The numeric count or frequency represented by the item.</param>
        /// <param name="Color">The color used to visually represent the item, such as in charts or graphs.</param>
        /// <param name="Percentage">The percentage value that this item represents relative to the total, expressed as a value between 0.0 and
        /// 100.0.</param>
        private record StatItem(string Label, int Count, Color Color, double Percentage);

        /// <summary>
        /// Represents statistical information for a specific grade, including the count and percentage of occurrences.
        /// </summary>
        /// <param name="Grade">The grade value for which statistics are recorded. Cannot be null.</param>
        /// <param name="Count">The number of times the specified grade occurs. Must be zero or greater.</param>
        /// <param name="Percentage">The percentage that the specified grade represents out of the total, expressed as a value between 0.0 and
        /// 100.0.</param>
        private record GradeStat(string Grade, int Count, double Percentage);

        /// <summary>
        /// Contains the collection of sales records for each month.
        /// </summary>
        private readonly List<SalesRecord> _salesRecords = new()
        {
            new("January", 32000, 25800, 14000),
            new("February", 34000, 27100, 16000),
            new("March", 36500, 28000, 17500),
            new("April", 38200, 29000, 18200)
        };

        /// <summary>
        /// Contains the collection of gender-based statistics used for reporting or display purposes.
        /// </summary>
        private readonly List<StatItem> _genderStats = new()
        {
            new("Boys", 525, Color.Info, 68),
            new("Girls", 430, Color.Secondary, 32)
        };

        /// <summary>
        /// Contains the default statistical data for each grade level.
        /// </summary>
        /// <remarks>This list is initialized with predefined grade statistics and is intended for
        /// internal use within the class. The data includes grade names and associated statistical values for each
        /// grade level.</remarks>
        private readonly List<GradeStat> _gradeStats = new()
        {
            new("Grade 8", 160, 20),
            new("Grade 9", 175, 22),
            new("Grade 10", 190, 24),
            new("Grade 11", 215, 27),
            new("Grade 12", 215, 27)
        };

        /// <summary>
        /// Contains the predefined statistics for each class group, including group name, count, color, and associated
        /// value.
        /// </summary>
        /// <remarks>This list is initialized with a fixed set of class group statistics and is intended
        /// for internal use. The contents should not be modified at runtime.</remarks>
        private readonly List<StatItem> _classGroupStats = new()
        {
            new("8A", 32, Color.Primary, 80),
            new("10A", 28, Color.Primary, 70),
            new("10B", 26, Color.Primary, 65),
            new("11A", 22, Color.Primary, 55),
            new("11B", 20, Color.Primary, 50),
            new("11C", 18, Color.Primary, 45),
            new("12C", 16, Color.Primary, 40)
        };

        /// <summary>
        /// Represents the summary statistics for different learner groups.
        /// </summary>
        private readonly List<StatItem> _learnerSummary = new()
        {
            new("Total Learners", 7000, Color.Primary, 100),
            new("Parents", 2700, Color.Secondary, 0),
            new("Staff", 4300, Color.Info, 0),
            new("Club Members", 1200, Color.Success, 0)
        };

        /// <summary>
        /// Contains a predefined list of learners with their respective details.
        /// </summary>
        private readonly List<LearnerItem> _learners = new()
        {
            new(1, "Nicole", "Adams", "Female", "Grade 12", 0.82m),
            new(2, "Jessica", "Meyer", "Female", "Grade 11", 0.75m),
            new(3, "Tom", "Mokone", "Male", "Grade 10", 0.78m),
            new(4, "Sarah", "Impey", "Female", "Grade 9", 0.81m),
            new(5, "Phillip", "Dube", "Male", "Grade 8", 0.70m),
            new(6, "Ashley", "Naidoo", "Female", "Grade 10", 0.76m),
            new(7, "Marius", "Van der Merwe", "Male", "Grade 11", 0.83m),
            new(8, "Daniel", "De vos", "Male", "Grade 12", 0.88m)
        };

        /// <summary>
        /// Asynchronously initializes the component by retrieving role and user data from the server.
        /// </summary>
        /// <remarks>This method fetches a list of roles and their associated user counts, as well as the
        /// total user count, from the server using the provided data provider. The retrieved data is stored in local
        /// collections for use within the component. This method also invokes the base class's initialization
        /// logic.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var roleResponse = await RoleService.AllRoles();
            if (roleResponse.Succeeded)
            {
                _useRoles = roleResponse.Data.ToList();

                foreach (var role in _useRoles)
                {
                    var roleUserCountResponse = await RoleService.RoleUserCount(role.Name);
                    if (roleUserCountResponse.Succeeded)
                    {
                        _roleUserCountCollection[role.Name] = roleUserCountResponse.Data;
                    }
                }
            }

            var userCountResponse = await UserService.UserCount();
            if (userCountResponse.Succeeded)
            {
                _userCount = userCountResponse.Data;
            }


            await base.OnInitializedAsync();
        }
    }
}
