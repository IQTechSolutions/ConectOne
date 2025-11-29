using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using ConectOne.Infrastructure.Implementation;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Enums;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Enums;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeuralTech.Base.Enums;

namespace IdentityModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides a set of user management operations, including user creation, retrieval, updates, and deletion, as well
    /// as handling user-specific actions such as managing privacy terms, connection status, and registration requests.
    /// </summary>
    /// <remarks>This service acts as an abstraction layer for managing user-related operations, leveraging
    /// the <see cref="UserManager{TUser}"/> to interact with the underlying user store. It includes methods for
    /// retrieving user information, updating user details, managing user statuses, and handling privacy and usage terms
    /// acceptance.   The service is designed to handle common user management scenarios in applications that require
    /// user authentication and profile management.</remarks>
    /// <param name="userManager"></param>
    public class UserService(UserManager<ApplicationUser> userManager, IRepository<UserInfo, string> userInfoRepo, IRepository<ContactNumber<UserInfo>, string> contactNumberRepository,
        IRepository<EmailAddress<UserInfo>, string> emailAddressRepository, IRepository<Address<UserInfo>, string> addressRepository, IRepository<EntityImage<UserInfo, string>, string> userInfoImageRepository, 
        RoleManager<ApplicationRole> roleManager) : ContactIntoService<UserInfo>(contactNumberRepository, emailAddressRepository), IUserService
    {
        /// <summary>
        /// Sets the timestamp for when the user accepted the privacy and usage terms.
        /// </summary>
        public async Task<IBaseResult> SetPrivacyAndUsageTermsAcceptedTimeStamp(string userId)
        {
            try
            {
                var userResult = await userManager.Users.FirstOrDefaultAsync(c => c.Id == userId);

                if (userResult is null)
                    return await Result.FailAsync("No User Found");

                userResult.PrivacyAndUsageTermsAcceptedTimeStamp = DateTime.Now;
                await userManager.UpdateAsync(userResult);

                return await Result.SuccessAsync("First Time User Status Updated");
            }
            catch (Exception e)
            {
                return await Result<UserInfoDto>.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Removes a user from the system by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be removed.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        public async Task<IBaseResult> RemoveUserAsync(string userId)
        {
            try
            {
                var user = userManager.Users.FirstOrDefault(c => c.Id == userId);
                if (user is null)
                    return await Result.FailAsync("User not found");
                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded) await Result.FailAsync(result.Errors.Select(c => c.Description).ToList());
                return await Result.SuccessAsync("User removed successfully");
            }
            catch (Exception e)
            {
                return await Result.FailAsync(e.Message);
            }

        }

        /// <summary>
        /// Retrieves all application users in the system,
        /// </summary>
        /// <returns>A list of all users registered on the application</returns>
        public async Task<List<ApplicationUser>> AllApplicationUsersAsync()
        {
            return userManager.Users.ToList();
        }

        /// <summary>
        /// Retrieves all users in the system, including their <see cref="UserInfo"/> data.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<UserInfoDto>>> AllUsers(UserPageParameters pageParameters)
        {
            try
            {
                var result = userManager.Users.Include(c => c.UserInfo).ThenInclude(c => c.EmailAddresses).ToList();

                if (!string.IsNullOrEmpty(pageParameters.SearchText))
                {
                    result = result.Where(c => !string.IsNullOrEmpty(c.UserInfo.FirstName) && c.UserInfo.FirstName.Contains(pageParameters.SearchText)
                                               || !string.IsNullOrEmpty(c.UserInfo.LastName) && c.UserInfo.LastName.Contains(pageParameters.SearchText)).ToList();
                }

                return await Result<IEnumerable<UserInfoDto>>.SuccessAsync(result.Select(user => new UserInfoDto(user)));
            }
            catch (Exception e)
            {
                return await Result<IEnumerable<UserInfoDto>>.FailAsync(e.Message);
            }
           
        }

        /// <summary>
        /// Retrieves a paginated list of users based on the specified paging and search parameters.
        /// </summary>
        /// <remarks>If the search text is provided, only users whose first or last name contains the
        /// search text are included in the results. The returned PaginatedResult includes paging metadata such as the
        /// current page number and page size.</remarks>
        /// <param name="pageParameters">The parameters that define the paging options and search criteria for the user list. Must specify the page
        /// number and page size; the search text is optional and filters users by first or last name.</param>
        /// <returns>A PaginatedResult containing a list of UserInfoDto objects for the requested page. If an error occurs, the
        /// result contains error messages and no user data.</returns>
        public async Task<PaginatedResult<UserInfoDto>> PagedUsers(UserPageParameters pageParameters)
        {
            try
            {
                var result = userManager.Users.Include(c => c.UserInfo).ThenInclude(c => c.EmailAddresses).ToList();

                if (!string.IsNullOrEmpty(pageParameters.SearchText))
                {
                    result = result.Where(c => !string.IsNullOrEmpty(c.UserInfo.FirstName) && c.UserInfo.FirstName.Contains(pageParameters.SearchText)
                                               || !string.IsNullOrEmpty(c.UserInfo.LastName) && c.UserInfo.LastName.Contains(pageParameters.SearchText)).ToList();
                }

                var userInfoDtos = result.Select(user => new UserInfoDto(user));
                return PaginatedResult<UserInfoDto>.Success(userInfoDtos.ToList(), userInfoDtos.Count(), pageParameters.PageNr, pageParameters.PageSize);
            }
            catch (Exception e)
            {
                return PaginatedResult<UserInfoDto>.Failure(new List<string>() { e.Message });
            }

        }

        /// <summary>
        /// Retrieves the count of active, non-deleted users in the system.
        /// </summary>
        public async Task<IBaseResult<int>> UserCount()
        {
            var userCount = userManager.Users.Where(c => c.IsActive && !c.IsDeleted).Count();
            return await Result<int>.SuccessAsync(userCount);
        }

        /// <summary>
        /// Returns paged user results with optional role-based or registration-status filtering.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<UserInfoDto>>> GetAllUsersAsync(UserPageParameters pageParameters)
        {
            var result = new List<ApplicationUser>();

            // Filter by role if requested
            if (!string.IsNullOrEmpty(pageParameters.Role))
            {
                var roleUsers = await userManager.GetUsersInRoleAsync(pageParameters.Role);
                foreach (var item in roleUsers)
                {
                    if (result.All(c => c.Id != item.Id))
                    {
                        result.Add(await userManager.Users.Include(c => c.UserInfo).FirstOrDefaultAsync(c => c.Id == item.Id));
                    }

                }
            }
            else
            {
                result = userManager.Users.Include(c => c.UserInfo).ToList();
            }

            // Filter by registration status if requested
            if (pageParameters.RegistrationStatus != null)
            {
                result = result
                    .Where(c => c.RegistrationStatus == pageParameters.RegistrationStatus)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(pageParameters.SearchText))
            {
                result = result.Where(c => !string.IsNullOrEmpty(c.UserInfo.FirstName) && c.UserInfo.FirstName.ToLower().Contains(pageParameters.SearchText.ToLower())
                                           || !string.IsNullOrEmpty(c.UserInfo.LastName) && c.UserInfo.LastName.ToLower().Contains(pageParameters.SearchText.ToLower())).ToList();
            }

            return await Result<IEnumerable<UserInfoDto>>.SuccessAsync(result.Select(c => new UserInfoDto(c)));
        }

        /// <summary>
        /// Creates a minimal <see cref="UserInfo"/> record for the specified user info DTO.
        /// </summary>
        public async Task<IBaseResult?> CreateUserInfoAsync(UserInfoDto userInfo)
        {
            var user = new UserInfo
            {
                Id = userInfo.UserId,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                ContactNumbers =
                {
                    new ContactNumber<UserInfo>
                    {
                        InternationalCode = "27",
                        Number = userInfo.PhoneNr
                    }
                },
                EmailAddresses =
                {
                    new EmailAddress<UserInfo>
                    {
                        Email = userInfo.EmailAddress
                    }
                }
            };

            var folderPath = Path.Combine("StaticFiles", "activitygroup", userInfo.FirstName + " " + userInfo.LastName);
            //var imageFile = _imageProcessingService.CreateImage(folderPath, userInfo.CoverImageUrl);
            //user.Images.Add(imageFile.ToImageFile<UserInfo, string>(userInfo.UserId, UploadType.Cover));

            var userInfoResult = await userInfoRepo.CreateAsync(user);
            if (userInfoResult.Succeeded)
            {
                return await Result.SuccessAsync("User Info added successfully");
            }
            return await Result.FailAsync(userInfoResult.Messages);
        }

        /// <summary>
        /// Adds or updates an address for the specified user.
        /// </summary>
        public async Task<IBaseResult> AddUpdateUserInfoAddress(string userId, AddressDto dto)
        {
            var userSpec = new LambdaSpec<UserInfo>(c => c.Id == userId);
            userSpec.AddInclude(c => c.Include(c => c.Addresses));

            // Check if the user record exists
            var userResult = await userInfoRepo.FirstOrDefaultAsync(userSpec, false);
            if (!userResult.Succeeded)
                return await Result.FailAsync(userResult.Messages);

            var user = userResult.Data;

            // If the user already has addresses, try to update existing
            if (user.Addresses.Any())
            {
                var address = user.Addresses.FirstOrDefault(c => c.EntityId == dto.AddressId);
                if (address is not null)
                {
                    address.UnitNumber = address.UnitNumber;
                    address.Complex = address.Complex;
                    address.StreetNumber = address.StreetNumber;
                    address.StreetName = address.StreetName;
                    address.Suburb = address.Suburb;
                    address.PostalCode = address.PostalCode;
                    address.City = address.City;
                    address.Province = address.Province;
                    address.Country = address.Country;
                    address.Latitude = address.Latitude;
                    address.Longitude = address.Longitude;
                    address.AddressType = address.AddressType;
                    address.Default = address.Default;

                    addressRepository.Update(address);
                    var saveResult = await addressRepository.SaveAsync();
                    if (!saveResult.Succeeded)
                        return await Result.FailAsync(saveResult.Messages);
                    return await Result.SuccessAsync("Address was updated successfully");
                }
            }

            var createdAddress = dto.ToAddress<UserInfo>();
            createdAddress.EntityId = userId;
            var creationResult = await addressRepository.CreateAsync(createdAddress);
            if (!creationResult.Succeeded) return await Result.FailAsync(creationResult.Messages);

            var creationSaveResult = await addressRepository.SaveAsync();
            if (!creationSaveResult.Succeeded)
                return await Result.FailAsync(creationSaveResult.Messages);

            return await Result.SuccessAsync("Address was created successfully");
        }

        /// <summary>
        /// Retrieves detailed user info by user ID, including images, addresses, and social media data.
        /// </summary>
        public async Task<IBaseResult<UserInfoDto>> GetUserInfoAsync(string userId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<UserInfo>(c => c.Id == userId);
            spec.AddInclude(c => c.Include(g => g.Images).ThenInclude(b => b.Image));
            spec.AddInclude(c => c.Include(c => c.Addresses));
            spec.AddInclude(c => c.Include(c => c.EmailAddresses));
            spec.AddInclude(c => c.Include(c => c.ContactNumbers));

            var userResult = await userInfoRepo.FirstOrDefaultAsync(spec, false, cancellationToken);

            if (userResult.Data is not null)
                return await Result<UserInfoDto>.SuccessAsync(new UserInfoDto(userResult.Data));

            return await Result<UserInfoDto>.SuccessAsync();
        }

        /// <summary>
        /// Retrieves user info by matching an email address, or null if not found.
        /// </summary>
        public async Task<IBaseResult<UserInfoDto>> GetUserInfoByEmailAsync(string email)
        {
            try
            {
                var user = await userManager.Users
                    .Include(c => c.UserInfo.Images)
                    .FirstOrDefaultAsync(c => c.Email == email);

                if (user is not null)
                    return await Result<UserInfoDto>.SuccessAsync(new UserInfoDto(user));

                return await Result<UserInfoDto>.SuccessAsync(new UserInfoDto());
            }
            catch (Exception e)
            {
               return await Result<UserInfoDto>.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Toggles a user's 'IsConnected' property, often used for 
        /// real-time presence or connection states in an application.
        /// </summary>
        public async Task<IBaseResult> ChangeConnectionStatus(string userId, bool status)
        {
            try
            {
                var userResult = await userManager.Users.FirstOrDefaultAsync(c => c.Id == userId);

                if (userResult is null)
                    return await Result.FailAsync("No User Found");

                userResult.IsConnected = status;
                await userManager.UpdateAsync(userResult);

                return await Result.SuccessAsync("User Connection Status Updated");
            }
            catch (Exception e)
            {
                return await Result<UserInfoDto>.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Updates the core user info (name, company, addresses, etc.) for a given user.
        /// </summary>
        public async Task<IBaseResult> UpdateUserInfoAsync(UserInfoDto dto)
        {
            // Check that this user info record exists
            var userInfoResult = userInfoRepo.FindByCondition(c => c.Id.Equals(dto.UserId), false);
            if (!userInfoResult.Succeeded)
                return await Result.FailAsync(userInfoResult.Messages);

            var userInfo = await userInfoResult.Data
                .Include(c => c.UserAppSettings).Include(c => c.Images).Include(c => c.EmailAddresses)
                .Include(c => c.ContactNumbers).FirstOrDefaultAsync();

            // Update basic user info fields
            userInfo.FirstName = dto.FirstName;
            userInfo.OtherNames = dto.MiddleName;
            userInfo.LastName = dto.LastName;
            userInfo.Bio = dto.Description;
            userInfo.CompanyName = dto.CompanyName;
            userInfo.VatNr = dto.VatNr;

            userInfoRepo.Update(userInfo);


            if (userInfo.Images.Any(c => c.Image.ImageType == UploadType.Cover))
            {
                await userInfoImageRepository.DeleteAsync(userInfo.Images.First(c => c.Image.ImageType == UploadType.Cover).Id);
            }

            var folderPath = Path.Combine("StaticFiles", "userInfo", userInfo.FirstName + " " + userInfo.LastName);

            //var file = _imageProcessingService.CreateImage(dto.CoverImageUrl, folderPath);
            //if (file != null)
            //    await _userInfoImageRepository.CreateAsync(file.ToImageFile<UserInfo, string>(userInfo.Id, UploadType.Cover));

            var userInfoSaveResult = await userInfoRepo.SaveAsync();

            if (!userInfoSaveResult.Succeeded)
                return await Result.FailAsync(userInfoSaveResult.Messages);

            // Update or create the physical address
            var userAddressResult = addressRepository.FindByCondition(c => c.EntityId.Equals(userInfo.Id) && c.AddressType == AddressType.Physical, false);
            if (userAddressResult.Succeeded)
            {
                if (userAddressResult.Data.Any())
                {
                    // Update existing address
                    var userAddress = await userAddressResult.Data.FirstOrDefaultAsync();
                    userAddress.UnitNumber = dto.Address.UnitNumber;
                    userAddress.Complex = dto.Address.Complex;
                    userAddress.StreetNumber = dto.Address.StreetNumber;
                    userAddress.StreetName = dto.Address.StreetName;
                    userAddress.Suburb = dto.Address.Suburb;
                    userAddress.PostalCode = dto.Address.PostalCode;
                    userAddress.City = dto.Address.City;
                    userAddress.Province = dto.Address.Province;
                    userAddress.Country = dto.Address.Country;
                    userAddress.Longitude = dto.Address.Longitude;
                    userAddress.Latitude = dto.Address.Latitude;

                    addressRepository.Update(userAddress);
                    await addressRepository.SaveAsync();
                }
                else
                {
                    // Create new address if not found
                    if (dto.Address is not null)
                    {
                        var address = new Address<UserInfo>
                        {
                            Id = Guid.NewGuid().ToString(),
                            EntityId = userInfo.Id,
                            UnitNumber = dto.Address.UnitNumber,
                            Complex = dto.Address.Complex,
                            StreetNumber = dto.Address.StreetNumber,
                            StreetName = dto.Address.StreetName,
                            Suburb = dto.Address.Suburb,
                            PostalCode = dto.Address.PostalCode,
                            City = dto.Address.City,
                            Province = dto.Address.Province,
                            Country = dto.Address.Country,
                            Longitude = dto.Address.Longitude,
                            Latitude = dto.Address.Latitude,
                            AddressType = AddressType.Physical
                        };

                        addressRepository.Create(address);
                        await addressRepository.SaveAsync();
                    }
                }
            }

            // Similarly handle the billing/company address
            var companyAddressResult = addressRepository.FindByCondition(c => c.EntityId.Equals(userInfo.Id) && c.AddressType == AddressType.Billing, false);
            if (companyAddressResult.Succeeded)
            {
                if (companyAddressResult.Data.Any())
                {
                    var companyAddress = await companyAddressResult.Data.FirstOrDefaultAsync();
                    companyAddress.UnitNumber = dto.CompanyAddress.UnitNumber;
                    companyAddress.Complex = dto.CompanyAddress.Complex;
                    companyAddress.StreetNumber = dto.CompanyAddress.StreetNumber;
                    companyAddress.StreetName = dto.CompanyAddress.StreetName;
                    companyAddress.Suburb = dto.CompanyAddress.Suburb;
                    companyAddress.PostalCode = dto.CompanyAddress.PostalCode;
                    companyAddress.City = dto.CompanyAddress.City;
                    companyAddress.Province = dto.CompanyAddress.Province;
                    companyAddress.Country = dto.CompanyAddress.Country;
                    companyAddress.Longitude = dto.CompanyAddress.Longitude;
                    companyAddress.Latitude = dto.CompanyAddress.Latitude;

                    addressRepository.Update(companyAddress);
                    await addressRepository.SaveAsync();
                }
                else
                {
                    if (dto.CompanyAddress is not null)
                    {
                        var address = new Address<UserInfo>
                        {
                            Id = Guid.NewGuid().ToString(),
                            EntityId = userInfo.Id,
                            UnitNumber = dto.CompanyAddress!.UnitNumber,
                            Complex = dto.CompanyAddress.Complex,
                            StreetNumber = dto.CompanyAddress.StreetNumber,
                            StreetName = dto.CompanyAddress.StreetName,
                            Suburb = dto.CompanyAddress.Suburb,
                            PostalCode = dto.CompanyAddress.PostalCode,
                            City = dto.CompanyAddress.City,
                            Province = dto.CompanyAddress.Province,
                            Country = dto.CompanyAddress.Country,
                            Longitude = dto.CompanyAddress.Longitude,
                            Latitude = dto.CompanyAddress.Latitude,
                            AddressType = AddressType.Billing
                        };

                        addressRepository.Create(address);
                        await addressRepository.SaveAsync();
                    }
                }
            }

            var user = await userManager.Users.FirstOrDefaultAsync(c => c.Id == dto.UserId);

            // Sync user entity with updated fields
            user!.CompanyName = dto.CompanyName;
            user.JobTitle = dto.JobTitle;
            user.PhoneNumber = dto.PhoneNr;
            user.Email = dto.EmailAddress;
            user.Licenses = dto.AllocatedLicences;

            await userManager.UpdateAsync(user);

            return await Result.SuccessAsync("User was successfully updated");
        }

        /// <summary>
        /// Toggles the user's 'IsActive' property, effectively enabling or disabling the account.
        /// </summary>
        public async Task<IBaseResult> ChangeUserStatusAsync(string userId)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(c => c.Id == userId);

                user.IsActive = !user.IsActive;
                await userManager.UpdateAsync(user);
                return await Result.SuccessAsync();
            }
            catch (Exception e)
            {
                return await Result.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Changes the password for a user, requiring the current password as verification.
        /// </summary>
        public async Task<IBaseResult> ChangeUserPasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(c => c.Id == request.UserId);

                await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

                return await Result.SuccessAsync();
            }
            catch (Exception e)
            {
                return await Result.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Accepts a user's registration, marking them active and assigning the selected roles.
        /// </summary>
        public async Task<IBaseResult> AcceptRegistrationAsync(AcceptRegistrationRequest registrationRequest)
        {
            try
            {
                var user = await userManager.FindByIdAsync(registrationRequest.UserId);

                user.RegistrationStatus = RegistrationStatus.Accepted;
                user.IsActive = true;

                await userManager.UpdateAsync(user);

                foreach (var role in registrationRequest.SelectedRoles)
                {
                    var selectedRole = await roleManager.GetRoleNameAsync(role.ToRole());
                    await userManager.AddToRoleAsync(user, selectedRole);
                }

                return await Result.SuccessAsync();
            }
            catch (Exception e)
            {
                return await Result.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Rejects a user's registration, optionally storing a reason. 
        /// </summary>
        public async Task<IBaseResult> RejectRegistrationAsync(RejectRegistrationRequest request)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(c => c.Id == request.UserId);
                user.RegistrationStatus = RegistrationStatus.Rejected;
                user.ReasonForRejection = request.ReasonForRejection;
                userManager.UpdateAsync(user); // Fire and forget

                return await Result.SuccessAsync();
            }
            catch (Exception e)
            {
                return await Result.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of users eligible for global notifications, including their details and associated
        /// metadata.
        /// </summary>
        /// <remarks>This method queries all users managed by the application, retrieves additional user
        /// information such as profile images, and constructs a collection of <see cref="RecipientDto"/> objects
        /// representing the users. The resulting list can be used for sending global notifications.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="RecipientDto"/> objects. The result includes
        /// user details such as IDs, names, email addresses, and profile image paths.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> GlobalNotificationsUserList()
        {
            var roleUsers = userManager.Users;
            var list = new List<RecipientDto>();
            foreach (var user in roleUsers)
            {
                var spec = new LambdaSpec<UserInfo>(c => c.Id == user.Id);
                spec.AddInclude(c => c.Include(g => g.Images));
                var result = await userInfoRepo.FirstOrDefaultAsync(spec, false);
                if (result.Succeeded)
                    list.Add(new RecipientDto(user.Id, string.IsNullOrEmpty(result.Data?.FirstName) ? "" : result.Data?.FirstName, string.IsNullOrEmpty(result.Data?.FirstName) ? "" : result.Data?.FirstName, new List<string>() { user.Email }, true, true, result.Data?.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Profile)?.Image?.RelativePath, MessageType.RoleMessage));
            }

            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(list);
        }
    }
}
