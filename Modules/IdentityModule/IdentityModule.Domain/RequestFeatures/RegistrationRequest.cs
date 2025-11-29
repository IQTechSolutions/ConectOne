using System.ComponentModel.DataAnnotations;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Data Transfer Object for handling user registration requests.
    /// </summary>
    public record RegistrationRequest
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Username is required")] public string UserName { get; init; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "First Name is required")] public string FirstName { get; init; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Last Name is required")] public string LastName { get; init; }

        /// <summary>
        /// Gets or sets the company name of the user.
        /// This field is optional.
        /// </summary>
        public string? CompanyName { get; init; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// This field is required and must be a valid email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required"), EmailAddress] public string Email { get; init; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Phone Nr is required")] public string PhoneNumber { get; init; }

        /// <summary>
        /// Gets or sets the ID number of the user.
        /// This field is optional.
        /// </summary>
        public string? IdNumber { get; set; }

        /// <summary>
        /// Gets or sets the password for the user account.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Password is required")] public string Password { get; init; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public string Role { get; init; }

        /// <summary>
        /// Gets or sets the origin of the registration request.
        /// This field is optional.
        /// </summary>
        public string? Origin { get; set; }

        /// <summary>
        /// Gets or sets the unique URL associated with the user.
        /// This field is optional.
        /// </summary>
        public string? UniqueUrl { get; set; }

        /// <summary>
        /// Gets or sets the team ID associated with the user.
        /// This field is optional.
        /// </summary>
        public string? TeamId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email confirmation process is completed.
        /// </summary>
        public bool ConfirmEmail { get; set; } = false;

        /// <summary>
        /// Converts the registration request to an <see cref="ApplicationUser"/> object.
        /// </summary>
        /// <param name="useUserNameForLogin">Indicates whether to use the username for login.</param>
        /// <returns>An <see cref="ApplicationUser"/> object.</returns>
        public ApplicationUser ToApplicationUser(bool useUserNameForLogin)
        {
            if (useUserNameForLogin)
                return new ApplicationUser(Guid.NewGuid().ToString(), this.Email, this.FirstName, this.LastName, this.PhoneNumber, this.Email, this.CompanyName, UniqueUrl);
            return new ApplicationUser(Guid.NewGuid().ToString(), this.Email, this.FirstName, this.LastName, this.PhoneNumber, this.Email, this.CompanyName, UniqueUrl);
        }
    }
}
