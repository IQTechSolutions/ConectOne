using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the contact information for a lodging entity, including phone numbers, email address, and website,
    /// for use in view models.
    /// </summary>
    /// <remarks>Use this class to encapsulate and transfer lodging contact details between application
    /// layers, such as from data transfer objects to user interface components. The class provides properties for
    /// common contact fields and methods to initialize or update the contact information as needed.</remarks>
    public class LodgingContactInfoViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingContactInfoViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor creates an empty instance of the <see
        /// cref="LodgingContactInfoViewModel"/> class. Use this constructor when you need to initialize the view model
        /// without preloading any data.</remarks>
        public LodgingContactInfoViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingContactInfoViewModel"/> class using the contact
        /// information from the specified lodging data transfer object.
        /// </summary>
        /// <remarks>The constructor extracts contact details such as phone numbers, email, and website
        /// from the provided <see cref="LodgingDto"/> and initializes the corresponding properties of the <see
        /// cref="LodgingContactInfoViewModel"/> instance.</remarks>
        /// <param name="lodging">A <see cref="LodgingDto"/> object containing the contact information for the lodging. This parameter must
        /// not be <see langword="null"/>.</param>
        public LodgingContactInfoViewModel(LodgingDto lodging)
        {
            Contacts = lodging.Contacts;
            PhoneNr = lodging.PhoneNr;
            FaxNr = lodging.FaxNr;
            CellNr = lodging.CellNr;
            Email = lodging.Email;
            Website = lodging.Website;
        }
        
        #endregion

        /// <summary>
        /// Gets or sets the contact information associated with the entity.
        /// </summary>
        public string? Contacts { get; set; }

        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        public string? PhoneNr { get; set; }

        /// <summary>
        /// Gets or sets the fax number associated with the entity.
        /// </summary>
        public string? FaxNr { get; set; }

        /// <summary>
        /// Gets or sets the cell number associated with the entity.
        /// </summary>
        public string? CellNr { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the website URL associated with the entity.
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// Updates the lodging contact information with the values provided in the specified view model.
        /// </summary>
        /// <remarks>This method replaces the current contact information with the values from the
        /// provided view model. Ensure that the <paramref name="address"/> parameter is not null and contains valid
        /// data.</remarks>
        /// <param name="address">A <see cref="LodgingContactInfoViewModel"/> instance containing the updated contact information. The
        /// properties of this object are used to update the corresponding fields.</param>
        public void Update(LodgingContactInfoViewModel address)
        {
            Contacts = address.Contacts;
            PhoneNr = address.PhoneNr;
            FaxNr = address.FaxNr;
            CellNr = address.CellNr;
            Email = address.Email;
            Website = address.Website;            
        }
    }
}
