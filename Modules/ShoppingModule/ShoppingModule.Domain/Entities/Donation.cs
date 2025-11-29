using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;
using ShoppingModule.Domain.Enums;
using ShoppingModule.Domain.RequestFeatures;

namespace ShoppingModule.Domain.Entities
{
    /// <summary>
    /// Represents a request to create a donation, including donor details, donation amount, and preferences.
    /// </summary>
    /// <remarks>This class is used to capture the necessary information for processing a donation. It
    /// includes required fields  such as the donor's full name, email address, and donation amount, as well as optional
    /// fields for additional  preferences like anonymity, recurring donations, and a custom message. Validation
    /// attributes are applied to  ensure the integrity of the data.</remarks>
    public class Donation : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Donation"/> class.
        /// </summary>
        /// <remarks>The constructor generates a unique identifier for the <see cref="Donation"/> instance
        /// by assigning a new GUID to the <see cref="Id"/> property.</remarks>
        public Donation()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Donation"/> class using the specified donation request.
        /// </summary>
        /// <remarks>The <paramref name="request"/> parameter must contain all required fields for the
        /// donation, such as the donor's full name, email, and amount. Optional fields, such as a message or
        /// designation, are also populated if provided.</remarks>
        /// <param name="request">The request containing the details of the donation, including the donor's information, donation amount, and
        /// preferences.</param>
        public Donation(CreateDonationRequest request)
        {
            FirstName = request.FirstName;
            LastName = request.LastName;
            Email = request.Email;
            PhoneNumber = request.PhoneNumber;
            Amount = request.Amount;
            Designation = request.Designation;
            Message = request.Message;
            IsAnonymous = request.IsAnonymous;
            IsRecurring = request.IsRecurring;
            Frequency = request.Frequency;
            AgreeToTerms = request.AgreeToTerms;
        }

        #endregion

        /// <summary>
        /// Gets or sets the full name of the individual.
        /// </summary>
        [Required] public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the person.
        /// </summary>
        [Required] public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        /// <remarks>This property uses the <see
        /// cref="System.ComponentModel.DataAnnotations.PhoneAttribute"/>          to validate that the value conforms
        /// to a valid phone number format.</remarks>
        [Phone] public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the monetary amount associated with the operation.
        /// </summary>
        [Range(typeof(double), "50", "1000000")] public double Amount { get; set; } = 100;

        /// <summary>
        /// Gets or sets the designation or title associated with an individual or entity.
        /// </summary>
        public string? Designation { get; set; }

        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        [MaxLength(1000)] public string? Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is anonymous.
        /// </summary>
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is recurring.
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Gets or sets the frequency of the donation.
        /// </summary>
        public DonationFrequency Frequency { get; set; } = DonationFrequency.OnceOff;

        /// <summary>
        /// Gets or sets a value indicating whether the user has agreed to the terms and conditions.
        /// </summary>
        /// <remarks>This property must be set to <see langword="true"/> to comply with the application's
        /// terms of use.</remarks>
        public bool AgreeToTerms { get; set; }
    }
}
