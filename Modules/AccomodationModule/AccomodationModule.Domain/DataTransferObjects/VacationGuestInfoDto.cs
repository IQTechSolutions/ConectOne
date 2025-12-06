using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for vacation guest information.
    /// This DTO is used to transfer guest information data between different layers of the application.
    /// </summary>
    public class VacationGuestInfoDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor for creating a new instance of <see cref="VacationGuestInfoDto"/>.
        /// </summary>
        public VacationGuestInfoDto() { }

        /// <summary>
        /// Initializes a new instance of <see cref="VacationGuestInfoDto"/> using a <see cref="VacationGuestInfo"/>.
        /// </summary>
        /// <param name="info">The guest information entity containing guest details.</param>
        public VacationGuestInfoDto(VacationGuestInfo info)
        {
            Name = info.Name;
            Surname = info.Surname;
            EmailAddress = info.EmailAddress;
            PhoneNr = info.PhoneNr;
            Address = info.Address;
            City = info.City;
            State = info.State;
            ZipCode = info.ZipCode;
            Country = info.Country;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the first name of the guest.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the surname of the guest.
        /// </summary>
        public string? Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address of the guest.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the guest.
        /// </summary>
        public string? PhoneNr { get; set; }

        /// <summary>
        /// Gets or sets the address of the guest.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the city of the guest.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the state of the guest.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the zip code of the guest.
        /// </summary>
        public string? ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the country of the guest.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the vacation details associated with the guest.
        /// </summary>
        public VacationDto? Vacation { get; set; }

        /// <summary>
        /// Gets or sets the pricing details associated with the guest's vacation.
        /// </summary>
        public string? PricingId { get; set; }

        #endregion

        /// <summary>
        /// Converts the <see cref="VacationGuestInfoDto"/> to a <see cref="VacationGuestInfo"/>.
        /// </summary>
        /// <returns>A new <see cref="VacationGuestInfoDto"/> object</returns>
        public VacationGuestInfo ToVacationGuestInfo()
        {
            return new VacationGuestInfo
            {
                Name = Name,
                Surname = Surname,
                EmailAddress = EmailAddress,
                PhoneNr = PhoneNr,
                Address = Address,
                City = City,
                State = State,
                ZipCode = ZipCode,
                Country = Country
            };
        }
    }
}