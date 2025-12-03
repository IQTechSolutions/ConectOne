using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityModule.Infrastructure.Implimentation
{
    public class TokenService(IRepository<DeviceToken, string> deviceTokenRepo, RoleManager<ApplicationRole> roleManager) : ITokenService
    {
        /// <summary>
        /// Retrieves device tokens for a collection of user IDs.
        /// </summary>
        public async Task<IBaseResult<List<DeviceTokenDto>>> DeviceTokensAsync(List<string> userIds)
        {
            var result = deviceTokenRepo.FindAll(false);
            if (result.Succeeded)
            {
                var deviceTokenList = new List<DeviceTokenDto>();
                if (userIds.Any())
                {
                    foreach (var userId in userIds)
                    {
                        deviceTokenList.AddRange(
                            result.Data
                                .Where(c => c.UserId == userId)
                                .Select(deviceToken => new DeviceTokenDto(deviceToken))
                        );
                    }
                }
                return await Result<List<DeviceTokenDto>>.SuccessAsync(deviceTokenList);
            }
            return await Result<List<DeviceTokenDto>>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Adds a new device token for a user if it does not already exist.
        /// </summary>
        public async Task<IBaseResult> CreateDeviceTokenAsync(DeviceTokenDto token)
        {
            var tokenResponse = deviceTokenRepo.FindByCondition(
                c => c.DeviceTokenContent.Equals(token.DeviceToken) && c.UserId.Equals(token.UserId), true
            );

            if (tokenResponse.Succeeded)
            {
                var existingToken = tokenResponse.Data.FirstOrDefault();
                if (existingToken == null)
                {
                    var deviceTokenResult = deviceTokenRepo.Create(new DeviceToken(token.UserId, token.DeviceToken));
                    if (!deviceTokenResult.Succeeded) return await Result.FailAsync(deviceTokenResult.Messages);

                    var saveResult = await deviceTokenRepo.SaveAsync();
                    if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

                    return await Result.SuccessAsync("Device Token added successfully");
                }
            }
            return await Result.FailAsync(tokenResponse.Messages);
        }

        /// <summary>
        /// Removes a specific device token for a user.
        /// </summary>
        public async Task<IBaseResult> RemoveDeviceTokenAsync(DeviceTokenDto token)
        {
            var deviceTokenResult = deviceTokenRepo.FindByCondition(
                c => c.UserId.Equals(token.UserId) && c.DeviceTokenContent.Equals(token.DeviceToken), false
            );

            if (deviceTokenResult.Succeeded)
            {
                var deviceToken = await deviceTokenResult.Data.FirstOrDefaultAsync();
                if (deviceToken is null)
                {
                    return await Result.FailAsync($"No device token matching id '{token.DeviceToken}' could be found in the database");
                }

                var removeResult = deviceTokenRepo.Delete(deviceToken);
                if (removeResult.Succeeded)
                    return await Result.SuccessAsync("Device token was removed successfully");
                return await Result.FailAsync(removeResult.Messages);
            }
            return await Result.FailAsync(deviceTokenResult.Messages);
        }

        
    }
}
