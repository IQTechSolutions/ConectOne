using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using ConectOne.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql;
using GroupingModule.Domain.Entities;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using SchoolsEnterprise.Base.Extensions;
using SchoolsModule.Domain.Entities;

namespace NelspruitHigh.Blazor.ServerClient.DataSeeder
{
    /// <summary>
    /// DataSeeder is responsible for creating initial data in the database
    /// (like users, roles, and default super-user) if they do not exist.
    /// Implements <see cref="IDataSeeder"/> to standardize the "Initialize" method.
    /// </summary>
    public class DataSeeder : IDataSeeder
    {
        private readonly ILogger<DataSeeder> _logger;
        private readonly GenericDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        /// <summary>
        /// The constructor that uses Dependency Injection to receive the required services:
        /// - ILogger for logging
        /// - DbContext (SchoolsEnterprise2023DbContext) typed as AuditableContext
        /// - UserManager & RoleManager from ASP.NET Core Identity
        /// </summary>
        public DataSeeder(ILogger<DataSeeder> logger, GenericDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Entry point to run seeding logic. Called once on startup or migration phase.
        /// </summary>
        public void Initialize()
        {
            AddSuperUser();
            AddRoles();
            SeedAgeGroups();
            SeedCategoriesForActivityGroups();
            SeedActivityGroups();
            SeedSchoolGrades();
            SeedSeverityScales();

            _context.SaveChanges();
        }

        /// <summary>
        /// Creates a super user role (if not already present) with all permissions,
        /// then checks if a user with a known email (ivanrossouw2@gmail.com) exists.
        /// If not, seeds that super user with a default password and attaches them to the role.
        /// Also adds all known permission claims to that super user role.
        /// </summary>
        private void AddSuperUser()
        {
            // Because we’re using async methods in seeding, we run them in a Task and block with GetAwaiter()
            Task.Run(async () =>
            {
                // 1. Ensure the SuperUser role is present or create it
                var superUserRole = await _roleManager.FindByNameAsync(RoleConstants.SuperUser);
                if (superUserRole == null)
                {
                    superUserRole = new ApplicationRole(RoleConstants.SuperUser, "Administrative role with full access and all permissions") { NotAvailableForRegistrationSelection = true, CannotDelete = true};

                    var roleResult = await _roleManager.CreateAsync(superUserRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded Super User Role.");
                    }
                    else
                    {
                        // Log any errors from the creation process
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                // 2. Check or create the SuperUser account by email
                var superUser = await _userManager.FindByEmailAsync("ivanrossouw2@gmail.com");
                if (superUser == null)
                {
                    // Creating a new user with specific details
                    superUser = new ApplicationUser(
                        Guid.NewGuid().ToString(),
                        "ivanrossouw2@gmail.com",
                        "Ivan",
                        "Rossouw",
                        "0764348180",
                        "ivanrossouw2@gmail.com",
                        "Private")
                    {
                        EmailConfirmed = true,
                        UserInfo = new UserInfo()
                        {
                            Title = Title.Me,
                            FirstName = "Ivan",
                            OtherNames = "",
                            LastName = "Rossouw",
                            IdentityNr = "",
                            Gender = Gender.Unknown,
                            MaritalStatus = MaritalStatus.Unknown,
                            MoodStatus = "",
                            Bio = "",
                            CompanyName = "NeuralTech Solutions",
                            VatNr = "",
                            ContactNumbers = [new ContactNumber<UserInfo>() { Number = "0764348180", Default = true }],
                            EmailAddresses = [new EmailAddress<UserInfo>() { Email = "ivanrossouw2@gmail.com", Default = true }]
                        }
                    };

                    // 2a. Attempt to create the user with a default password
                    var registrationResult = await _userManager.CreateAsync(superUser, "Is20170804!!");
                    if (registrationResult.Succeeded)
                    {
                        // 2b. Add user to the SuperUser role
                        var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.SuperUser);
                        if (!result.Succeeded)
                        {
                            // If adding role failed, log errors
                            foreach (var error in result.Errors)
                            {
                                _logger.LogError(error.Description);
                            }
                        }
                    }
                    else
                    {
                        // If user creation failed, log errors
                        foreach (var error in registrationResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                // 3. Ensure the associated UserInfo record is created if it doesn’t exist
                //var userInfoResult = _context.Set<UserInfo>().FirstOrDefault(x => x.Id == superUser.Id);
                //if (userInfoResult == null)
                //{
                //    var userInfo = 

                //    _context.Set<UserInfo>().Add(userInfo);
                //    await _context.SaveChangesAsync();
                //}

                // 4. Grab all registered permissions from some extension method (e.g. reflection-based claim list)
                var registeredPermisions = ClaimsPrincipalExtensions.GetRegisteredPermissions();

                // 5. Add all permissions as claims to the SuperUser role
                foreach (var permission in registeredPermisions)
                {
                    await _roleManager.AddPermissionClaim(superUserRole, permission);
                }

            }).GetAwaiter().GetResult();  // Block the thread to ensure seeding completes
        }

        /// <summary>
        /// Adds various roles like Administrator, CompanyAdmin (Guest?), Basic, Parent, Learner, Teacher, etc.
        /// Each role is created if it doesn’t already exist, and default permissions are assigned.
        /// </summary>
        private void AddRoles()
        {
            Task.Run(async () =>
            {
                // 1. Administrator role
                var administratorRole = await _roleManager.FindByNameAsync(RoleConstants.Administrator);
                if (administratorRole == null)
                {
                    administratorRole = new ApplicationRole(
                        RoleConstants.Administrator,
                        "Administrative role with access and permissions granted by the super user")
                        { NotAvailableForRegistrationSelection = true, CannotDelete = true};

                    var roleResult = await _roleManager.CreateAsync(administratorRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded Administrator Role.");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                // 1a. Add default permissions to Administrator from the same extension method
                var adminPermisions = ClaimsPrincipalExtensions.GetRegisteredPermissions(false);
                foreach (var permission in adminPermisions)
                {
                    await _roleManager.AddPermissionClaim(administratorRole, permission);
                }

                // 4. Parent role
                var parentRole = await _roleManager.FindByNameAsync(RoleConstants.Parent);
                if (parentRole == null)
                {
                    parentRole = new ApplicationRole(
                        RoleConstants.Parent,
                        "Parent role with parent access and parent permissions") { CannotDelete = true };

                    var roleResult = await _roleManager.CreateAsync(parentRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded Parent Role.");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                // 5. Learner role
                var learnerRole = await _roleManager.FindByNameAsync(RoleConstants.Learner);
                if (learnerRole == null)
                {
                    learnerRole = new ApplicationRole(
                        RoleConstants.Learner,
                        "Learner role with learner access and learner permissions") { CannotDelete = true };

                    var roleResult = await _roleManager.CreateAsync(learnerRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded Learner Role.");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                // 6. Teacher role
                var teacherRole = await _roleManager.FindByNameAsync(RoleConstants.Teacher);
                if (teacherRole == null)
                {
                    teacherRole = new ApplicationRole(
                        RoleConstants.Teacher,
                        "Teacher role with teacher access and teacher permissions") { CannotDelete = true };

                    var roleResult = await _roleManager.CreateAsync(teacherRole);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Seeded Teacher Role.");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

            }).GetAwaiter().GetResult();  // Block the thread to ensure seeding finishes
        }

        /// <summary>
        /// Seeds the database with a predefined set of age group entities if none exist.
        /// </summary>
        /// <remarks>This method should be called during application initialization to ensure that the
        /// required age group data is available. If age groups already exist in the database, the method performs no
        /// action.</remarks>
        private void SeedAgeGroups()
        {
            if (_context.Set<AgeGroup>().Any()) return;

            var ageGroups = new List<AgeGroup>();
            ageGroups.Add(new AgeGroup() { Id = "AG_0000", Name = "All Learners", MinAge = 0, MaxAge = 18 });
            ageGroups.Add(new AgeGroup() { Id = "AG_0001", Name = "Junior", MinAge = 6, MaxAge = 9 });
            ageGroups.Add(new AgeGroup() { Id = "AG_0002", Name = "Senior", MinAge = 10, MaxAge = 13 });
            ageGroups.Add(new AgeGroup() { Id = "AG_0003", Name = "Under 7", MinAge = 14, MaxAge = 18 });
            ageGroups.Add(new AgeGroup() { Id = "AG_0004", Name = "Under 8", MinAge = 19, MaxAge = 25 });
            ageGroups.Add(new AgeGroup() { Id = "AG_0005", Name = "Under 9", MinAge = 19, MaxAge = 25 });
            ageGroups.Add(new AgeGroup() { Id = "AG_0006", Name = "Under 10", MinAge = 19, MaxAge = 25 });
            ageGroups.Add(new AgeGroup() { Id = "AG_0007", Name = "Under 11", MinAge = 19, MaxAge = 25 });
            ageGroups.Add(new AgeGroup() { Id = "AG_0008", Name = "Under 12", MinAge = 19, MaxAge = 25 });
            ageGroups.Add(new AgeGroup() { Id = "AG_0009", Name = "Under 13", MinAge = 19, MaxAge = 25 });
            
            foreach (var ageGroup in ageGroups)
            {
                _context.Set<AgeGroup>().Add(ageGroup);
            }
        }

        /// <summary>
        /// Seeds the database with a predefined set of categories for activity groups if no such categories currently
        /// exist.
        /// </summary>
        /// <remarks>This method should be called during application initialization or database setup to
        /// ensure that essential activity group categories are available. If any categories already exist in the
        /// database, the method performs no action.</remarks>
        private void SeedCategoriesForActivityGroups()
        {
            if (_context.Set<Category<ActivityGroup>>().Any()) return;

            var activityGroupCategories = new List<Category<ActivityGroup>>();

            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0001", Name = "Sports", Active = true, Featured = true, DisplayCategoryInMainManu = true});
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0002", Name = "Cultural", Active = true, Featured = true, DisplayCategoryInMainManu = true });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0003", Name = "Academic", Active = true, Featured = true, DisplayCategoryInMainManu = true });

            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0004", Name = "Cricket", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0005", Name = "Mountain Biking", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0006", Name = "Hockey", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0007", Name = "Rugby", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0008", Name = "Tennis", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0009", Name = "Debate", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0010", Name = "Netball", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0011", Name = "Choir", Active = true, Featured = true, ParentCategoryId = "AGC_0002" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0012", Name = "Athletics", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0013", Name = "Chess", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0014", Name = "Cross Country", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0015", Name = "Swimming", Active = true, Featured = true, ParentCategoryId = "AGC_0001" });
            activityGroupCategories.Add(new Category<ActivityGroup>() { Id = "AGC_0016", Name = "Music", Active = true, Featured = true, ParentCategoryId = "AGC_0002" });
            
            foreach (var activityGroupCategory in activityGroupCategories)
            {
                _context.Set<Category<ActivityGroup>>().Add(activityGroupCategory);
            }
        }

        /// <summary>
        /// Seeds the data store with a predefined set of activity groups if none exist.
        /// </summary>
        /// <remarks>This method should be called during application initialization or database setup to
        /// ensure that the required activity group data is available. If activity groups already exist in the data
        /// store, this method performs no action.</remarks>
        private void SeedActivityGroups()
        {
            if (_context.Set<ActivityGroup>().Any()) return;

            var activityGroups = new List<ActivityGroup>();

            activityGroups.Add(new ActivityGroup() { Id = "AG_0001", Name = "Cricket U7", Gender = Gender.Male, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0004" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0002", Name = "Cricket U8", Gender = Gender.Male, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0004" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0003", Name = "Cricket U9", Gender = Gender.Male, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0004" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0004", Name = "Cricket U10", Gender = Gender.Male, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0004" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0005", Name = "Cricket U11", Gender = Gender.Male, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0004" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0006", Name = "Cricket U12", Gender = Gender.Male, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0004" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0007", Name = "Cricket U13", Gender = Gender.Male, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0004" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0008", Name = "Mountain Biking U7 Boys", Gender = Gender.Male, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0009", Name = "Mountain Biking U8 Boys", Gender = Gender.Male, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0010", Name = "Mountain Biking U9 Boys", Gender = Gender.Male, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0011", Name = "Mountain Biking U10 Boys", Gender = Gender.Male, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0012", Name = "Mountain Biking U11 Boys", Gender = Gender.Male, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0013", Name = "Mountain Biking U12 Boys", Gender = Gender.Male, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0014", Name = "Mountain Biking U13 Boys", Gender = Gender.Male, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0016", Name = "Mountain Biking U7 Girls", Gender = Gender.Female, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0017", Name = "Mountain Biking U8 Girls", Gender = Gender.Female, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0018", Name = "Mountain Biking U9 Girls", Gender = Gender.Female, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0019", Name = "Mountain Biking U10 Girls", Gender = Gender.Female, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0020", Name = "Mountain Biking U11 Girls", Gender = Gender.Female, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0021", Name = "Mountain Biking U12 Girls", Gender = Gender.Female, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0022", Name = "Mountain Biking U13 Girls", Gender = Gender.Female, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0005" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0023", Name = "Hockey U7 Boys", Gender = Gender.Male, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0024", Name = "Hockey U8 Boys", Gender = Gender.Male, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0025", Name = "Hockey U9 Boys", Gender = Gender.Male, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0026", Name = "Hockey U10 Boys", Gender = Gender.Male, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0027", Name = "Hockey U11 Boys", Gender = Gender.Male, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0028", Name = "Hockey U12 Boys", Gender = Gender.Male, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0029", Name = "Hockey U13 Boys", Gender = Gender.Male, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0030", Name = "Hockey U7 Girls", Gender = Gender.Female, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0031", Name = "Hockey U8 Girls", Gender = Gender.Female, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0032", Name = "Hockey U9 Girls", Gender = Gender.Female, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0033", Name = "Hockey U10 Girls", Gender = Gender.Female, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0034", Name = "Hockey U11 Girls", Gender = Gender.Female, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0035", Name = "Hockey U12 Girls", Gender = Gender.Female, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0036", Name = "Hockey U13 Girls", Gender = Gender.Female, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0006" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0037", Name = "Rugby U7", Gender = Gender.Male, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0007" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0038", Name = "Rugby U8", Gender = Gender.Male, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0007" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0039", Name = "Rugby U9", Gender = Gender.Male, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0007" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0040", Name = "Rugby U10", Gender = Gender.Male, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0007" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0041", Name = "Rugby U11", Gender = Gender.Male, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0007" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0042", Name = "Rugby U12", Gender = Gender.Male, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0007" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0043", Name = "Rugby U13", Gender = Gender.Male, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0007" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0044", Name = "Tennis U7 Boys", Gender = Gender.Male, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0045", Name = "Tennis U8 Boys", Gender = Gender.Male, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0046", Name = "Tennis U9 Boys", Gender = Gender.Male, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0047", Name = "Tennis U10 Boys", Gender = Gender.Male, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0048", Name = "Tennis U11 Boys", Gender = Gender.Male, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0049", Name = "Tennis U12 Boys", Gender = Gender.Male, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0050", Name = "Tennis U13 Boys", Gender = Gender.Male, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0051", Name = "Tennis U7 Girls", Gender = Gender.Female, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0052", Name = "Tennis U8 Girls", Gender = Gender.Female, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0053", Name = "Tennis U9 Girls", Gender = Gender.Female, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0054", Name = "Tennis U10 Girls", Gender = Gender.Female, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0055", Name = "Tennis U11 Girls", Gender = Gender.Female, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0056", Name = "Tennis U12 Girls", Gender = Gender.Female, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0057", Name = "Tennis U13 Girls", Gender = Gender.Female, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0008" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0058", Name = "Debate Junior", Gender = Gender.All, AgeGroupId = "AG_0001", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0009" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0059", Name = "Debate Senior", Gender = Gender.All, AgeGroupId = "AG_0002", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0009" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0060", Name = "Netball U7", Gender = Gender.Female, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0010" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0061", Name = "Netball U8", Gender = Gender.Female, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0010" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0062", Name = "Netball U9", Gender = Gender.Female, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0010" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0063", Name = "Netball U10", Gender = Gender.Female, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0010" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0064", Name = "Netball U11", Gender = Gender.Female, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0010" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0065", Name = "Netball U12", Gender = Gender.Female, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0010" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0066", Name = "Netball U13", Gender = Gender.Female, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0010" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0067", Name = "Choir Junior", Gender = Gender.All, AgeGroupId = "AG_0001", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0011" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0068", Name = "Choir Senior", Gender = Gender.All, AgeGroupId = "AG_0002", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0011" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0069", Name = "Athletics U7 Boys", Gender = Gender.Male, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0070", Name = "Athletics U8 Boys", Gender = Gender.Male, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0071", Name = "Athletics U9 Boys", Gender = Gender.Male, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0072", Name = "Athletics U10 Boys", Gender = Gender.Male, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0073", Name = "Athletics U11 Boys", Gender = Gender.Male, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0074", Name = "Athletics U12 Boys", Gender = Gender.Male, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0075", Name = "Athletics U13 Boys", Gender = Gender.Male, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0076", Name = "Athletics U7 Girls", Gender = Gender.Female, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0077", Name = "Athletics U8 Girls", Gender = Gender.Female, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0078", Name = "Athletics U9 Girls", Gender = Gender.Female, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0079", Name = "Athletics U10 Girls", Gender = Gender.Female, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0080", Name = "Athletics U11 Girls", Gender = Gender.Female, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0081", Name = "Athletics U12 Girls", Gender = Gender.Female, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0082", Name = "Athletics U13 Girls", Gender = Gender.Female, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0012" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0083", Name = "Chess U7", Gender = Gender.All, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0013" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0084", Name = "Chess U8", Gender = Gender.All, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0013" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0085", Name = "Chess U9", Gender = Gender.All, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0013" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0086", Name = "Chess U10", Gender = Gender.All, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0013" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0087", Name = "Chess U11", Gender = Gender.All, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0013" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0088", Name = "Chess U12", Gender = Gender.All, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0013" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0089", Name = "Chess U13", Gender = Gender.All, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0013" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0090", Name = "Cross Country U7 Boys", Gender = Gender.Male, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0091", Name = "Cross Country U8 Boys", Gender = Gender.Male, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0092", Name = "Cross Country U9 Boys", Gender = Gender.Male, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0093", Name = "Cross Country U10 Boys", Gender = Gender.Male, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0094", Name = "Cross Country U11 Boys", Gender = Gender.Male, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0095", Name = "Cross Country U12 Boys", Gender = Gender.Male, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0096", Name = "Cross Country U13 Boys", Gender = Gender.Male, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0097", Name = "Cross Country U7 Girls", Gender = Gender.Female, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0098", Name = "Cross Country U8 Girls", Gender = Gender.Female, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0099", Name = "Cross Country U9 Girls", Gender = Gender.Female, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0100", Name = "Cross Country U10 Girls", Gender = Gender.Female, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0101", Name = "Cross Country U11 Girls", Gender = Gender.Female, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0102", Name = "Cross Country U12 Girls", Gender = Gender.Female, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0103", Name = "Cross Country U13 Girls", Gender = Gender.Female, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0014" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0104", Name = "Swimming U7 Boys", Gender = Gender.Male, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0105", Name = "Swimming U8 Boys", Gender = Gender.Male, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0106", Name = "Swimming U9 Boys", Gender = Gender.Male, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0107", Name = "Swimming U10 Boys", Gender = Gender.Male, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0108", Name = "Swimming U11 Boys", Gender = Gender.Male, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0109", Name = "Swimming U12 Boys", Gender = Gender.Male, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0110", Name = "Swimming U13 Boys", Gender = Gender.Male, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0111", Name = "Swimming U7 Girls", Gender = Gender.Female, AgeGroupId = "AG_0003", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0112", Name = "Swimming U8 Girls", Gender = Gender.Female, AgeGroupId = "AG_0004", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0113", Name = "Swimming U9 Girls", Gender = Gender.Female, AgeGroupId = "AG_0005", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0114", Name = "Swimming U10 Girls", Gender = Gender.Female, AgeGroupId = "AG_0006", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0115", Name = "Swimming U11 Girls", Gender = Gender.Female, AgeGroupId = "AG_0007", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0116", Name = "Swimming U12 Girls", Gender = Gender.Female, AgeGroupId = "AG_0008", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0117", Name = "Swimming U13 Girls", Gender = Gender.Female, AgeGroupId = "AG_0009", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0015" } } });

            activityGroups.Add(new ActivityGroup() { Id = "AG_0118", Name = "Music Junior", Gender = Gender.All, AgeGroupId = "AG_0001", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0016" } } });
            activityGroups.Add(new ActivityGroup() { Id = "AG_0119", Name = "Music Senior", Gender = Gender.All, AgeGroupId = "AG_0002", 
                Categories = new List<EntityCategory<ActivityGroup>>() { new EntityCategory<ActivityGroup>() { CategoryId = "AGC_0016" } } });

            foreach (var activityGroup in activityGroups)
            {
                _context.Set<ActivityGroup>().Add(activityGroup);
            }
        }

        /// <summary>
        /// Seeds the database with a predefined set of school grades and their associated classes if no grades
        /// currently exist.
        /// </summary>
        /// <remarks>This method should be called during application initialization or database setup to
        /// ensure that the required school grade and class data is available. If any school grades already exist in the
        /// database, the method performs no action.</remarks>
        private void SeedSchoolGrades()
        {
            if (_context.Set<SchoolGrade>().Any()) return;

            var schoolGrades = new List<SchoolGrade>
            {
                new SchoolGrade { Id = "SG_0000", Name = "Grade R", Classes = new List<SchoolClass>()
                {
                    new SchoolClass { Id = "SC_0000", Name = "RA" },
                    new SchoolClass { Id = "SC_0001", Name = "RB" },
                    new SchoolClass { Id = "SC_0002", Name = "RC" },
                }},
                new SchoolGrade { Id = "SG_0001", Name = "Grade 1", Classes = new List<SchoolClass>()
                {
                    new SchoolClass { Id = "SC_0003", Name = "1A" },
                    new SchoolClass { Id = "SC_0004", Name = "1B" },
                    new SchoolClass { Id = "SC_0005", Name = "1C" },
                } },
                new SchoolGrade { Id = "SG_0002", Name = "Grade 2", Classes = new List<SchoolClass>()
                {
                    new SchoolClass { Id = "SC_0006", Name = "2A" },
                    new SchoolClass { Id = "SC_0007", Name = "2B" },
                    new SchoolClass { Id = "SC_0008", Name = "2C" },
                } },
                new SchoolGrade { Id = "SG_0003", Name = "Grade 3", Classes = new List<SchoolClass>()
                {
                    new SchoolClass { Id = "SC_0009", Name = "3A" },
                    new SchoolClass { Id = "SC_0010", Name = "3B" },
                    new SchoolClass { Id = "SC_0011", Name = "3C" },
                } },
                new SchoolGrade { Id = "SG_0004", Name = "Grade 4", Classes = new List<SchoolClass>()
                {
                    new SchoolClass { Id = "SC_0012", Name = "4A" },
                    new SchoolClass { Id = "SC_0013", Name = "4B" },
                    new SchoolClass { Id = "SC_0014", Name = "4C" },
                } },
                new SchoolGrade { Id = "SG_0005", Name = "Grade 5", Classes = new List<SchoolClass>()
                {
                    new SchoolClass { Id = "SC_0015", Name = "5A" },
                    new SchoolClass { Id = "SC_0016", Name = "5B" },
                    new SchoolClass { Id = "SC_0017", Name = "5C" },
                } },
                new SchoolGrade { Id = "SG_0006", Name = "Grade 6", Classes = new List<SchoolClass>()
                {
                    new SchoolClass { Id = "SC_0018", Name = "6A" },
                    new SchoolClass { Id = "SC_0019", Name = "6B" },
                    new SchoolClass { Id = "SC_0020", Name = "6C" },
                } },
                new SchoolGrade { Id = "SG_0007", Name = "Grade 7", Classes = new List<SchoolClass>()
                {
                    new SchoolClass { Id = "SC_0021", Name = "7A" },
                    new SchoolClass { Id = "SC_0022", Name = "7B" },
                    new SchoolClass { Id = "SC_0023", Name = "7C" },
                } },
            };

            foreach (var schoolGrade in schoolGrades)
            {
                _context.Set<SchoolGrade>().Add(schoolGrade);
            }
        }

        /// <summary>
        /// Seeds the database with default severity scale values if none exist.
        /// </summary>
        /// <remarks>This method should be called during application initialization to ensure that the
        /// required severity scale data is present. If severity scales already exist in the database, this method
        /// performs no action.</remarks>
        private void SeedSeverityScales()
        {
            if (_context.Set<SeverityScale>().Any()) return;

            var severityScales = new List<SeverityScale>
            {
                new SeverityScale() { Name = "Mild", Score = 1, Description = "This is just a minor infraction"},
                new SeverityScale() { Name = "Moderate", Score = 2, Description = "This is a moderate infraction"},
                new SeverityScale() { Name = "Severe", Score = 3, Description = "This is severe infraction"},
            };
            foreach (var severityScale in severityScales)
            {
                _context.Set<SeverityScale>().Add(severityScale);
            }
        }
    }
}
