using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using ConectOne.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace IdentityModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides functionality to export user data to Excel files based on specified filtering and pagination criteria.
    /// </summary>
    /// <remarks>This service supports exporting user details such as ID, name, email, phone number, status,
    /// and creation dates. Filtering by user role is supported if specified. The export operation is performed
    /// asynchronously and may take longer for large datasets.</remarks>
    /// <param name="userManager">The user manager used to retrieve and manage user information for export operations. Cannot be null.</param>
    public class ExportService(UserManager<ApplicationUser> userManager) : IExportService
    {
        /// <summary>
        /// Exports a filtered list of users to an Excel file asynchronously.
        /// </summary>
        /// <remarks>The exported Excel file includes user details such as ID, name, email, phone number,
        /// status, and creation dates. If a role is specified in <paramref name="pageParameters"/>, only users in that
        /// role are exported; otherwise, all users are included. The operation is performed asynchronously and may take
        /// longer for large datasets.</remarks>
        /// <param name="pageParameters">The parameters used to filter and paginate the user list. The role filter is applied if specified;
        /// otherwise, all users are included.</param>
        /// <returns>A string containing the file path or identifier of the generated Excel file.</returns>
        public async Task<string> ExportToExcelAsync(UserPageParameters pageParameters)
        {
            var users = new List<ApplicationUser>();
            if (!string.IsNullOrEmpty(pageParameters.Role))
            {
                var roleUsers = await userManager.GetUsersInRoleAsync(pageParameters.Role);
                foreach (var item in roleUsers)
                {
                    if (users.All(c => c.Id != item.Id))
                    {
                        users.Add(await userManager.Users.Include(c => c.UserInfo).FirstOrDefaultAsync(c => c.Id == item.Id));
                    }

                }
            }
            else
            {
                users = userManager.Users.Include(c => c.UserInfo).ToList();
            }

            var result = await ExcelExtensions.ExportAsync(users, sheetName: "Users",
                mappers: new Dictionary<string, Func<ApplicationUser, object>>
                {
                    { "Id", item => item.Id },
                    { "FirstName", item => item.UserInfo?.FirstName == null ? "" : item.UserInfo.FirstName },
                    { "LastName", item => item.UserInfo?.LastName == null ? "" : item.UserInfo.LastName  },
                    { "UserName", item => item.UserName == null ? "" : item.UserName  },
                    { "Email", item => item.Email == null ? "" : item.Email  },
                    { "EmailConfirmed", item => item.EmailConfirmed },
                    { "PhoneNumber", item => item.PhoneNumber == null ? "" : item.PhoneNumber },
                    { "PhoneNumberConfirmed", item => item.PhoneNumberConfirmed },
                    { "IsActive", item => item.IsActive },
                    {
                        "CreatedOn (Local)",
                        item => item.CreatedOn == null ? "" :DateTime.SpecifyKind(item.CreatedOn.Value, DateTimeKind.Utc)
                            .ToLocalTime()
                            .ToString("G", CultureInfo.CurrentCulture)
                    },
                    {
                        "CreatedOn (UTC)",
                        item => item.CreatedOn == null ? "" :item.CreatedOn.Value.ToString("G", CultureInfo.CurrentCulture)
                    }
                }
            );

            return result;
        }
    }
}
