using System.ComponentModel.DataAnnotations;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for handling email registration data.
    /// </summary>
    public class EmailRegistrationViewModel
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the first name of the user.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "First Name is required")] public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Last Name is required")] public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the company name of the user.
        /// This field is optional.
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the currently selected option.
        /// </summary>
        public string SelectedOption { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the email address of the user.
        /// This field is required and must be a valid email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required"), EmailAddress] public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Phone Nr is required"), MinLength(11)] public string PhoneNumber { get; set; } 

        /// <summary>
        /// Gets or sets the unique URL associated with the user.
        /// This field is optional.
        /// </summary>
        public string? UniqueUrl { get; set; }

        /// <summary>
        /// Gets or sets the ID number of the user.
        /// This field is optional.
        /// </summary>
        public string? IdNumber { get; set; }

        /// <summary>
        /// Gets or sets the password for the user account.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Gets or sets the confirmation password.
        /// This field must match the Password field.
        /// </summary>
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = null!;

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public string Role { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the user accepts the terms and conditions.
        /// </summary>
        [Display(Name = "Accept T&C's")]
        public bool AcceptTerms { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account should be activated.
        /// Default value is true.
        /// </summary>
        [Display(Name = "Activate User")]
        public bool ActivateUser { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the user's email should be automatically confirmed.
        /// </summary>
        [Display(Name = "Auto Confirm Email")]
        public bool AutoConfirmEmail { get; set; }

        /// <summary>
        /// Gets or sets the team ID associated with the user.
        /// This field is optional.
        /// </summary>
        public string? TeamId { get; set; }

        #region Methods

        /// <summary>
        /// Converts the current object to a <see cref="RegistrationRequest"/> instance.
        /// </summary>
        /// <remarks>This method maps the properties of the current object to a new <see
        /// cref="RegistrationRequest"/> object. The resulting object contains the user's registration details,
        /// including their name, contact information,  and role.</remarks>
        /// <returns>A <see cref="RegistrationRequest"/> object populated with the corresponding properties of the current
        /// object.</returns>
        public RegistrationRequest ToRegistrationRequest(bool confirmEmailBeforeLogin = false)
        {
            return new RegistrationRequest()
            {
                UserName = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                CompanyName = this.CompanyName,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                Password = this.Password,
                Role = this.Role,
                ConfirmEmail = confirmEmailBeforeLogin
            };
        }

        #endregion
    }
}
