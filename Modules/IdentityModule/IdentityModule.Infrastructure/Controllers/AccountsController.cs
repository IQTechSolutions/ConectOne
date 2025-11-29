using System.IdentityModel.Tokens.Jwt;
using FilingModule.Domain.Enums;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using ConectOne.Domain.Mailing;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.Enums;

namespace IdentityModule.Infrastructure.Controllers
{
    /// <summary>
    /// AccountController handles user registration, authentication, password resets, user and role management, 
    /// as well as permissions. It integrates with Identity and a custom IAuthService for token generation and 
    /// authentication logic.
    /// </summary>
    [Route("api/account"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly DefaultEmailSender _emailSender;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IExportService _exportService;

        private readonly RoleManager<ApplicationRole> _roleManager;

        /// <summary>
        /// Constructor that uses dependency injection to set up required services.
        /// </summary>
        public AccountController(UserManager<ApplicationUser> userManager, ITokenService tokenService, IConfiguration configuration, DefaultEmailSender emailSender, 
            IUserService userService, RoleManager<ApplicationRole> roleManager, IExportService exportService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _userService = userService;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _exportService = exportService;
            _exportService=exportService;
        }

        #region Authentication

        /// <summary>
        /// Authenticates a user with given credentials. On success, returns JWT and refresh token.
        /// </summary>
        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthRequest userAuthentication)
        {
            try
            {
                ApplicationUser user;

                // Check if we authenticate by username or email
                if (userAuthentication.AuthenticationType == AuthenticationType.UserName)
                {
                    user = await _userManager.FindByNameAsync(userAuthentication.UserName);
                }
                else
                {
                    user = await _userManager.FindByEmailAsync(userAuthentication.UserName);
                    if (user == null)
                    {
                        var emailAddress = await _userService.GetUserInfoByEmailAsync(userAuthentication.UserName);
                        if (emailAddress.Succeeded)
                            user = _userManager.Users.FirstOrDefault(c => c.Id == emailAddress.Data.UserId);
                    }
                }

                if (user == null) return Unauthorized();

                if (!user.UserLoginAproved(_configuration)) return Unauthorized();
                //if (!user.IsActive || user.IsDeleted) return Unauthorized();
                if (!user.EmailConfirmed) return Ok(await Result<AuthResponse>.FailAsync("Email Address not confirmed"));

                var passwordValid = await _userManager.CheckPasswordAsync(user, userAuthentication.Password);
                if (!passwordValid) return Unauthorized();

                // Generate token and refresh token
                var token = await GetTokenAsync(user);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiryTime = DateTime.Now.AddDays(365);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                
                await _userManager.UpdateAsync(user);

                return Ok(new AuthResponse()
                {
                    PrivacyAndUsageTermsAcceptedTimeStamp = user.PrivacyAndUsageTermsAcceptedTimeStamp,
                    IsSuccessfulAuth = true,
                    Token = token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = refreshTokenExpiryTime,
                    UserId = user.Id,
                    UserImageURL = user.UserInfo.Images.Where(c => c.Image.ImageType == UploadType.Cover).FirstOrDefault()?.Image.RelativePath
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        /// <summary>
        /// Authenticates a user with given credentials. On success, returns JWT and refresh token.
        /// </summary>
        [HttpPost("loginV2"), AllowAnonymous]
        public async Task<IActionResult> LoginV2([FromBody] AuthRequest userAuthentication)
        {
            try
            {
                ApplicationUser user;

                // Check if we authenticate by username or email
                if (userAuthentication.AuthenticationType == AuthenticationType.UserName)
                {
                    user = await _userManager.FindByNameAsync(userAuthentication.UserName);
                }
                else
                {
                    user = await _userManager.FindByEmailAsync(userAuthentication.UserName);
                    if (user == null)
                    {
                        var emailAddress = await _userService.GetUserInfoByEmailAsync(userAuthentication.UserName);
                        if (emailAddress.Succeeded)
                            user = _userManager.Users.FirstOrDefault(c => c.Id == emailAddress.Data.UserId);
                    }
                }

                if (user == null) return Unauthorized();

                if (!user.UserLoginAproved(_configuration)) return Unauthorized();
                if (!user.IsActive || user.IsDeleted) return Unauthorized();
                if (!user.EmailConfirmed) return Ok(await Result<AuthResponse>.FailAsync("Email Address not confirmed"));

                var passwordValid = await _userManager.CheckPasswordAsync(user, userAuthentication.Password);
                if (!passwordValid) return Unauthorized();

                // Generate token and refresh token
                var token = await GetTokenAsync(user);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiryTime = DateTime.Now.AddDays(365);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

                await _userManager.UpdateAsync(user);

                var result = await Result<AuthResponse>.SuccessAsync(new AuthResponse()
                {
                    PrivacyAndUsageTermsAcceptedTimeStamp = user.PrivacyAndUsageTermsAcceptedTimeStamp,
                    IsSuccessfulAuth = true,
                    Token = token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = refreshTokenExpiryTime,
                    UserId = user.Id,
                    UserImageURL = user.UserInfo.Images.Where(c => c.Image.ImageType == UploadType.Cover).FirstOrDefault()?.Image.RelativePath
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(await Result<AuthResponse>.FailAsync(e.Message));
            }

        }

        /// <summary>
        /// Sets the privacy and usage terms accepted timestamp for a user.
        /// </summary>
        /// <param name="userId">The identity of the user's status that needs to be updated</param>
        [HttpPost("setPrivacyAndUsageTermsAcceptedTimeStamp/{userId}")]
        public async Task<IActionResult> SetPrivacyAndUsageTermsAcceptedTimeStamp(string userId)
        {
            var user = await _userService.SetPrivacyAndUsageTermsAcceptedTimeStamp(userId);
            return Ok(user);
        }

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// Automatically confirms their email and assigns them the provided role.
        /// </summary>
        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequest userForRegistration)
        {
            try
            {
                if (userForRegistration == null || !ModelState.IsValid)
                    return UnprocessableEntity(ModelState);

                var userNameForLogin = bool.TryParse(_configuration["UserNameForLogin"], out var useUserNameForLogin) && useUserNameForLogin;

                var user = userForRegistration.ToApplicationUser(userNameForLogin);
                // Auto-confirm email and mark user as active
                user.EmailConfirmed = userForRegistration.ConfirmEmail;
                user.IsActive = true;

                var result = await _userManager.CreateAsync(user, userForRegistration.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return Ok(await Result.FailAsync(errors.ToList()));
                }

                await _userManager.AddToRolesAsync(user, new List<string> { userForRegistration.Role });

                var userInfo = new UserInfoDto()
                {
                    UserId = user.Id,
                    FirstName = userForRegistration.FirstName,
                    LastName = userForRegistration.LastName,
                    PhoneNr = userForRegistration.PhoneNumber,
                    EmailAddress = userForRegistration.Email
                };

                if (userForRegistration.ConfirmEmail)
                {
                    var origin = userForRegistration.Origin;
                    if (string.IsNullOrWhiteSpace(origin) && Request.Headers.TryGetValue("origin", out var originHeaders))
                    {
                        origin = originHeaders.FirstOrDefault();
                    }

                    origin = string.IsNullOrWhiteSpace(origin) ? _configuration["ApplicationConfiguration:WebAddress"] : origin;

                    if (string.IsNullOrWhiteSpace(origin))
                    {
                        origin = Request.Host.HasValue ? $"{Request.Scheme}://{Request.Host.Value}" : string.Empty;
                    }

                    if (!string.IsNullOrWhiteSpace(origin))
                    {
                        origin = origin.EndsWith('/') ? origin : $"{origin}/";
                    }

                    var fromEmailAddress = string.IsNullOrWhiteSpace(_configuration["ApplicationConfiguration:AppliactionName"])
                        ? _configuration["EmailConfiguration:From"]
                        : _configuration["ApplicationConfiguration:DoNotReplyEmailAddress"];

                    var fromName = string.IsNullOrWhiteSpace(_configuration["ApplicationConfiguration:AppliactionName"])
                        ? _configuration["EmailConfiguration:caption"]
                        : _configuration["ApplicationConfiguration:DoNotReplyEmailAddress"];

                    fromEmailAddress ??= string.Empty;
                    fromName ??= string.Empty;

                    var confirmationLink = await GetConformationLinkAsync(user, origin);
                    var emailResult = await _emailSender.SendUserVerificationEmailAsync(
                        $"{userInfo.FirstName} {userInfo.LastName}".Trim(),
                        user.Email!,
                        fromName,
                        fromEmailAddress,
                        confirmationLink);

                    if (!emailResult.Succeeded)
                    {
                        return Ok(await Result.FailAsync(emailResult.Messages));
                    }
                }

                return Ok(await Result.SuccessAsync("Registration successful. Please check your email to confirm your account."));
            }
            catch (Exception ex)
            {
                return Ok(await Result.FailAsync(ex.Message));
            }
        }

        /// <summary>
        /// Checks if a user with the given email exists and returns their email if found.
        /// Used in "forgot password" flows to verify user existence.
        /// </summary>
        [HttpPost("forgot"), AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordRequest forgotPassword)
        {
            // Try finding user directly by email if not found by contact info
            var user = await _userManager.FindByEmailAsync(forgotPassword.EmailAddress);
            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = $"{forgotPassword.ReturnUrl}?UserId={user.Id}&ResetCode={code}";

                var emailResult = await _emailSender.SendForgotPasswordEmailAsync(forgotPassword.EmailAddress,
                    HtmlEncoder.Default.Encode(callbackUrl),
                    _configuration["EmailConfiguration:logoUrl"],
                    _configuration["EmailConfiguration:caption"],
                    _configuration["EmailConfiguration:logoLink"]
                );

                if (!emailResult.Succeeded) return Ok(await Result<string>.FailAsync(emailResult.Messages));
                return Ok(await Result<string>.SuccessAsync(data: user.Email));
            }

            return Ok(await Result<string>.FailAsync($"No user with email address '{forgotPassword.EmailAddress}' found"));
        }

        /// <summary>
        /// Resets the user's password using the provided reset code.
        /// </summary>
        [HttpPost("reset"), AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest emailAddress)
        {
            var user = await _userManager.FindByIdAsync(emailAddress.UserId);
            if (user == null) return Ok(await Result.FailAsync($"No user with Id '{emailAddress.UserId}' found"));

            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(emailAddress.ResetCode)), emailAddress.Password);
            if (!result.Succeeded)
                return Ok(await Result.FailAsync(result.Errors.FirstOrDefault()?.Description));

            return Ok(await Result.SuccessAsync("Password successfully updated"));
        }

        /// <summary>
        /// Generates a confirmation link for email verification.
        /// </summary>
        private async Task<string> GetConformationLinkAsync(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirmEmail/";
            var endpointUri = new Uri(string.Concat($"{origin}", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }

        /// <summary>
        /// Confirms a user's email using the provided userId and code.
        /// Redirects to a configured web address after successful confirmation.
        /// </summary>
        [HttpGet("confirmEmail"), AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Redirect($"{_configuration["ApplicationConfiguration:WebAddress"]}");
            }
            else
            {
                throw new Exception($"An error occurred while confirming {user?.Email}");
            }
        }
        
        /// <summary>
        /// Retrieves device tokens for given user IDs. Used for push notifications.
        /// </summary>
        [HttpPost("deviceTokens")]
        public async Task<IActionResult> GetDeviceToken([FromBody] List<string> userIds)
        {
            var result = await _tokenService.DeviceTokensAsync(userIds);
            return Ok(result);
        }

        /// <summary>
        /// Saves a device token for a user. Used to register devices for push notifications.
        /// </summary>
        [HttpPut("createDeviceToken"), AllowAnonymous]
        public async Task<IActionResult> SetDeviceToken([FromBody] DeviceTokenDto deviceToken)
        {
            var result = await _tokenService.CreateDeviceTokenAsync(deviceToken);
            return Ok(result);
        }

        /// <summary>
        /// Removes a saved device token for a user.
        /// </summary>
        [HttpDelete("removeDeviceToken/{userId}/{deviceToken}"), AllowAnonymous]
        public async Task<IActionResult> RemoveDeviceToken(string userId, string deviceToken)
        {
            var result = await _tokenService.RemoveDeviceTokenAsync(new DeviceTokenDto(userId, deviceToken));
            return Ok(result);
        }

        /// <summary>
        /// Refreshes the user's JWT token using a valid refresh token.
        /// Returns a new token and refresh token.
        /// </summary>
        [HttpPost("refresh"), AllowAnonymous]
        public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequest? model)
        {
            if (model is null)
            {
                return Unauthorized();
            }

            var userPrincipal = GetPrincipalFromExpiredToken(model.Token);
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return Unauthorized();
            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return Unauthorized();

            var token = GenerateEncryptedToken(await GetClaimsAsync(user));
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new AuthResponse() { IsSuccessfulAuth = true, Token = token, RefreshToken = refreshToken });
        }

        #endregion

        #region Export/Import

        /// <summary>
        /// Exports user data to Excel or another format based on the searchString provided.
        /// Returns the exported data.
        /// </summary>
        [HttpGet("exportNow")]
        public async Task<IActionResult> Export([FromQuery]UserPageParameters args)
        {
            var data = await _exportService.ExportToExcelAsync(args);
            return Ok(data);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Generates a JWT token string for the specified user, 
        /// including their claims and signing credentials.
        /// </summary>
        /// <param name="user">The <see cref="ApplicationUser"/> for whom the token is generated.</param>
        /// <returns>A JWT token string.</returns>
        private async Task<string> GetTokenAsync(ApplicationUser user)
        {
            // Force user info retrieval (in case it's needed for claims).
            var userInfo = await _userService.GetUserInfoAsync(user.Id);
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsAsync(user);

            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        /// <summary>
        /// Creates signing credentials using the configured security key and the HMAC SHA-256 algorithm.
        /// </summary>
        /// <returns>A <see cref="SigningCredentials"/> instance initialized with the configured symmetric security key and HMAC
        /// SHA-256 algorithm.</returns>
        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration.GetSection("JWTSettings").GetValue<string>("securityKey");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Asynchronously retrieves a list of claims associated with the specified user, including identity, role, and
        /// permission claims.
        /// </summary>
        /// <remarks>The returned claims include standard identity claims (such as name and email), as
        /// well as claims for each role and any additional claims assigned to those roles. This method aggregates
        /// claims from both the user and their roles.</remarks>
        /// <param name="user">The user for whom to retrieve claims. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of claims for the
        /// specified user. The list will be empty if the user has no claims.</returns>
        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(user.UserInfo.FirstName))
                    claims.Add(new Claim(ClaimTypes.GivenName, user.UserInfo.FirstName));
                if (!string.IsNullOrWhiteSpace(user.UserInfo.LastName))
                    claims.Add(new Claim(ClaimTypes.Surname, user.UserInfo.LastName));
                if (!string.IsNullOrWhiteSpace(user.Email))
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);
                claims.AddRange(allPermissionsForThisRoles);
            }

            return claims;
        }

        /// <summary>
        /// Creates a new JSON Web Token (JWT) with the specified signing credentials and claims.
        /// </summary>
        /// <remarks>The generated token uses the configured issuer and audience, and is set to expire two
        /// days from the time of creation.</remarks>
        /// <param name="signingCredentials">The credentials used to sign the generated JWT. Must not be null.</param>
        /// <param name="claims">A list of claims to include in the JWT payload. Cannot be null.</param>
        /// <returns>A JwtSecurityToken instance representing the generated JWT with the specified claims and signing
        /// credentials.</returns>
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            try
            {
                return new JwtSecurityToken
                (
                    issuer: _configuration.GetSection("JWTSettings").GetValue<string>("validIssuer"),
                    audience: _configuration.GetSection("JWTSettings").GetValue<string>("validAudience"),
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(2), // token expiration
                    signingCredentials: signingCredentials
                );
            }
            catch (Exception)
            {
                // Potentially log or rethrow as needed
                throw;
            }
        }

        /// <summary>
        /// Creates a simple "refresh token" as a base64 string, 
        /// used to generate new access tokens after expiry.
        /// </summary>
        /// <returns>A base64-encoded refresh token.</returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Validates a JWT token and retrieves its principal, 
        /// including handling of expired tokens.
        /// </summary>
        /// <param name="token">The JWT token string.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> extracted from the token.</returns>
        /// <exception cref="SecurityTokenException">Thrown if the token is invalid or has unexpected algorithm.</exception>
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("139796117134-f89i9blu0qwerqweqredfasdrt1234rasdfasdfasdfasdrqwerqwe")),
                ValidateLifetime = true,
                ValidIssuer = _configuration.GetSection("JWTSettings").GetValue<string>("validIssuer"),
                ValidAudience = _configuration.GetSection("JWTSettings").GetValue<string>("validAudience")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        /// <summary>
        /// Generates an encrypted token (similar to a standard JWT) from provided claims, 
        /// setting a 2-day expiry, and returning it as a string.
        /// </summary>
        private string GenerateEncryptedToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: GetSigningCredentials()
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

    }
}
