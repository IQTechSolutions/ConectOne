using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;
using ProductsModule.Application.ViewModels;
using System.ComponentModel;
using GroupingModule.Application.ViewModels;
using LocationModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for managing lodging details.
    /// </summary>
    public class LodgingViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LodgingViewModel()
        {
        }

        /// <summary>
        /// Constructor that initializes the view model with a LodgingDto object.
        /// </summary>
        /// <param name="lodging">The lodging DTO.</param>
        public LodgingViewModel(LodgingDto lodging)
        {
            LodgingId = lodging.ProductId;
            UniqueProductPartnerId = lodging.UniqueProductPartnerId;

            // Initialize nested view models with data from the LodgingDto
            Details = new LodgingDetailsViewModel(lodging);
            Settings = new LodgingSettingsViewModel(lodging.Settings);
            Address = new LodgingAddressViewModel(lodging);
            Policies = new LodgingPoliciesViewModel(lodging);
            Pricing = new PricingViewModel(lodging.Pricing!);
            ContactInfo = new LodgingContactInfoViewModel(lodging);
            PageDetails = new LodgingPageDetailsViewModel(lodging);
            LodgingType = lodging.LodgingType;
            Country = lodging.Country;

            DefaultRateSheme = lodging.DefaultRateScheme;
            CustomRate = lodging.CustomRate;
            ProductCategoryId = lodging.ProductCategoryId;

            SelectedDestinations = lodging.SelectedDestinations?.ToList();
            Categories = lodging.Categories.Select(c => new CategoryViewModel(c));
            Amenities = lodging.Amenities.Select(c => new LodgingAmenityViewModel(c));
            ProductPackages = lodging.Packages.Select(p => new LodgingPackageViewModel(p));

            Rooms = lodging.Rooms.ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the lodging details.
        /// </summary>
        public LodgingDetailsViewModel Details { get; set; } = new LodgingDetailsViewModel();

        /// <summary>
        /// Gets or sets the lodging settings.
        /// </summary>
        public LodgingSettingsViewModel Settings { get; set; } = new LodgingSettingsViewModel();

        /// <summary>
        /// Gets or sets the lodging address.
        /// </summary>
        public LodgingAddressViewModel Address { get; set; } = new LodgingAddressViewModel();

        /// <summary>
        /// Gets or sets the lodging policies.
        /// </summary>
        public LodgingPoliciesViewModel Policies { get; set; } = new LodgingPoliciesViewModel();

        /// <summary>
        /// Gets or sets the pricing details.
        /// </summary>
        public PricingViewModel Pricing { get; set; } = new PricingViewModel();

        /// <summary>
        /// Gets or sets the contact information.
        /// </summary>
        public LodgingContactInfoViewModel ContactInfo { get; set; } = new LodgingContactInfoViewModel();

        /// <summary>
        /// Gets or sets the page details.
        /// </summary>
        public LodgingPageDetailsViewModel PageDetails { get; set; } = new LodgingPageDetailsViewModel();

        /// <summary>
        /// Gets or sets the collection of selected destinations.
        /// </summary>
        public IEnumerable<DestinationDto>? SelectedDestinations { get; set; }

        /// <summary>
        /// Gets or sets the lodging type associated with the entity.
        /// </summary>
        public LodgingTypeDto LodgingType { get; set; }

        /// <summary>
        /// Gets the country information associated with the entity.
        /// </summary>
        public CountryDto? Country { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the lodging.
        /// </summary>
        public string LodgingId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier for the product partner.
        /// </summary>
        [DisplayName("Unique Api Partner Id")]
        public string? UniqueProductPartnerId { get; set; }

        /// <summary>
        /// Gets a value indicating whether live rates apply based on the presence of a UniqueProductPartnerId.
        /// </summary>
        public bool LiveRatesApply => UniqueProductPartnerId != null;

        /// <summary>
        /// Gets or sets the default rate scheme for the lodging.
        /// </summary>
        [DisplayName("Default Rate Scheme")]
        public RateScheme DefaultRateSheme { get; set; }

        /// <summary>
        /// Gets or sets the custom rate for the lodging.
        /// </summary>
        public double CustomRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the pricing has changed.
        /// </summary>
        public bool PricingHasChanged { get; set; }

        /// <summary>
        /// Gets or sets the collection of product packages.
        /// </summary>
        public IEnumerable<LodgingPackageViewModel>? ProductPackages { get; set; } = [];

        /// <summary>
        /// Gets or sets the product category identifier.
        /// </summary>
        [DisplayName("Category")]
        public int? ProductCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the available product categories for selection.
        /// </summary>
        public IEnumerable<SelectListItem>? AvailableProductCategories { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modification date.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of categories.
        /// </summary>
        public IEnumerable<CategoryViewModel> Categories { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of amenities.
        /// </summary>
        public IEnumerable<LodgingAmenityViewModel> Amenities { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of services.
        /// </summary>
        public IEnumerable<ServiceViewmodel>? Services { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of room details represented as view models.
        /// </summary>
        public List<RoomDto> Rooms { get; set; } = [];

        /// <summary>
        /// Gets or sets the area associated with the current entity.
        /// </summary>
        public AreaDto Area { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current lodging entity into a <see cref="LodgingDto"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the lodging entity, including details, policies, 
        /// contact information, address, and other related data, to a <see cref="LodgingDto"/>  representation. The
        /// resulting DTO can be used for data transfer or serialization purposes.</remarks>
        /// <returns>A <see cref="LodgingDto"/> object containing the mapped data from the current lodging entity.</returns>
        public LodgingDto ToDto()
        {
            var lodging = new LodgingDto
            {
                ProductId = Details.LodgingId,
                Name = Details.Name,
                TeaserText = Details.TeaserText,
                Description = Details.Description,
                OnlineDescription = Details.OnlineDescription,
                RoomInformation = Details.RoomInformation,
                Attractions = Details.Attractions,
                Grading = Details.Grading,
                Rating = Details.Rating,
                Facilities = Details.Facilities,

                Images = Details.Images,

                Area = Area,
                LodgingType = LodgingType,
                Country = Country,

                // Pricing and rate schemes
                DefaultRateScheme = DefaultRateSheme,
                CustomRate = CustomRate,
                Pricing = Pricing.ToDto(),
                PriceHasChanged = PricingHasChanged,

                // Guest age policies
                LowestGuestAgeCutOff = Policies.LowestGuestAgeCutOff,
                MiddleGuestAgeCutOff = Policies.MiddleGuestAgeCutOff,
                HighestGuestAgeCutOff = Policies.HighestGuestAgeCutOff,
                TermsAndConditions = Policies.TermsAndConditions,
                DepositPolicy = Policies.DepositPolicy,
                ChildPolicy = Policies.ChildPolicy,

                SelectedDestinations = SelectedDestinations,

                // Cancellation rules
                CancellationRules = Policies.CancellationRules.Select(c => c.ToDto()),

                Contacts = ContactInfo?.Contacts,
                PhoneNr = ContactInfo?.PhoneNr,
                FaxNr = ContactInfo?.FaxNr,
                CellNr = ContactInfo?.CellNr,
                Email = ContactInfo?.Email,
                Website = ContactInfo?.Website,

                Settings = Settings is not null ? Settings.ToDto() : null,

                UniqueProductPartnerId = UniqueProductPartnerId,

                PageTitle = PageDetails.PageTitle,
                MetaKeys = PageDetails.MetaKeys,
                MetaDescription = PageDetails.MetaDescription,

                AreaId = Address?.AreaId ?? 0,
                AreaInfo = Address?.AreaInfo ?? "",
                Address = Address?.Address ?? "",
                Suburb = Address?.Suburb ?? "",
                City = Address?.City ?? "",
                Lat = Address?.Lat ?? 0,
                Lng = Address?.Lng ?? 0,
                MapLink = Address?.MapLink,
                Directions = Address?.Directions,

                ProductCategoryId = ProductCategoryId,

                Amenities = Amenities?.Select(c => c.ToDto()),
                Categories = Categories?.Select(c => c.ToDto())
            };
            return lodging;
        }

        #endregion
    }
}
