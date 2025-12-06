using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using LocationModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a lodging entity, including details about accommodations, policies, pricing, contact information,
    /// location, and related services.
    /// </summary>
    /// <remarks>The <see cref="Lodging"/> class provides a comprehensive representation of a lodging entity,
    /// including its descriptive details, policies, pricing, and relationships to other entities such as amenities,
    /// vouchers, and services. This class is designed to be used in scenarios where detailed information about a
    /// lodging is required, such as booking systems or accommodation management platforms.</remarks>
    public class Lodging : FileCollection<Lodging,string>
    {
        #region Lodging Details

        /// <summary>
        /// Gets or sets the unique identifier associated with a partner.
        /// </summary>
        public string? UniquePartnerId { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string? Name { get; set; } 

        /// <summary>
        /// Gets or sets the description of the web resource.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the description of the proposal.
        /// </summary>
        public string? OnlineDescription { get; set; }

        /// <summary>
        /// Gets or sets information about the room, such as its name, description, or other relevant details.
        /// </summary>
        public string? RoomInformation { get; set; }

        /// <summary>
        /// Gets or sets the teaser text for the content.
        /// </summary>
        public string? Teaser { get; set; }

        /// <summary>
        /// Gets or sets the facilities available at the location.
        /// </summary>
        public string? Facilities { get; set; }

        /// <summary>
        /// Gets or sets the list of attractions associated with the entity.
        /// </summary>
        public string? Attractions { get; set; }

        /// <summary>
        /// Gets or sets the grading value, typically representing a score or evaluation metric.
        /// </summary>
        public int Grading { get; set; }

        /// <summary>
        /// Gets or sets the rating value.
        /// </summary>
        public int Rating { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the lodging settings for the application.
        /// </summary>
        public LodgingSettings Settings { get; set; } = null!;

        #region Policies

        /// <summary>
        /// Gets or sets the terms and conditions associated with the current policy.
        /// </summary>
        public string? TermsAndConditions { get; set; }

        /// <summary>
        /// Gets or sets the deposit policy for the current transaction or account.
        /// </summary>
        public string? DepositPolicy { get; set; }

        /// <summary>
        /// Gets or sets the policy applied to child elements or entities.
        /// </summary>
        public string? ChildPolicy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the minimum age required for a guest to qualify for certain services or eligibility criteria.
        /// </summary>
        public int LowestGuestAgeCutOff { get; set; }

        /// <summary>
        /// Gets or sets the age cutoff used to classify guests as middle-aged.
        /// </summary>
        public int MiddleGuestAgeCutOff { get; set; }

        /// <summary>
        /// Gets or sets the maximum age cutoff for a guest.
        /// </summary>
        public int HighestGuestAgeCutOff { get; set; }
        
        /// <summary>
        /// Gets the cancellation policy as a formatted string based on the defined cancellation rules.
        /// </summary>
        /// <remarks>The cancellation policy is generated dynamically based on the collection of
        /// cancellation rules. Each rule specifies the number of days before arrival and the corresponding forfeiture
        /// amount.</remarks>
        public string? CancellationPolicy
        {
            get
            {
                if (CancellationRules.Count==0)
                    return string.Empty;

                var returnString = new StringBuilder();

                foreach (var rule in CancellationRules.OrderBy(c => c.DaysBeforeBookingThatCancellationIsAvailable))
                {
                    returnString.AppendLine($"If cancelling {rule.DaysBeforeBookingThatCancellationIsAvailable} days before arrival, forfeit " +
                        $"{rule.CancellationFormualaType.GetCancellationRuleAbbreviation(rule.CancellationFormualaValue)}");
                }

                return returnString.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the collection of cancellation rules associated with the entity.
        /// </summary>
        public virtual ICollection<CancellationRule> CancellationRules { get; set; } = [];

        #endregion

        #region Pricing

        /// <summary>
        /// Gets or sets the default rate scheme used for pricing calculations.
        /// </summary>
        public RateScheme DefaultRateScheme { get; set; }

        /// <summary>
        /// Gets or sets the default commission percentage applied to transactions.
        /// </summary>
        public double DefaultCommissionPercentage { get; set; } = 20;

        /// <summary>
        /// Gets or sets the default markup percentage applied to calculations.
        /// </summary>
        public double DefaultMarkupPercentage { get; set; } = 4;

        /// <summary>
        /// Gets or sets the discount percentage applied to a transaction.
        /// </summary>
        public double Discount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the rate value.
        /// </summary>
        public double Rate { get; set; }

        #endregion

        #region Contact Information

        /// <summary>
        /// Gets or sets the contact information for the lodging entity.
        /// </summary>
        public string? Contacts { get; set; }

        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        public string? PhoneNr { get; set; }

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

        #endregion

        #region PageDetails

        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        public string? PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords for the page.
        /// </summary>
        public string? MetaKeys { get; set; }

        /// <summary>
        /// Gets or sets the description of the meteorological data.
        /// </summary>
        public string? MeteDescription { get; set; }

        #endregion

        #region Location

        /// <summary>
        /// Gets or sets information about the area.
        /// </summary>
        public string? AreaInfo { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the name of the suburb associated with the address.
        /// </summary>
        public string? Suburb { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// Gets or sets the zoom level for the view.
        /// </summary>
        public int Zoom { get; set; }

        /// <summary>
        /// Gets or sets the directions for completing a task or navigating a process.
        /// </summary>
        public string? Directions { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the province.
        /// </summary>
        public int? ProvinceId { get; set; }

        #endregion

        #region VacationProperties
        
        /// <summary>
        /// Gets or sets the type of a single room available for vacation properties.
        /// </summary>
        public string? SingleRoomType { get; set; }

        /// <summary>
        /// Gets or sets the meal plan type associated with a single room.
        /// </summary>
        public MealPlanTypes? SingleRoomMealPlanType { get; set; }

        /// <summary>
        /// Gets or sets the type of double room available.
        /// </summary>
        public string? DoubleRoomType { get; set; }

        /// <summary>
        /// Gets or sets the meal plan type associated with a double room.
        /// </summary>
        public MealPlanTypes? DoubleRoomMealPlanType { get; set; }
        
        /// <summary>
        /// Gets or sets the type of twin room associated with the booking or accommodation.
        /// </summary>
        public string? TwinRoomType { get; set; }

        /// <summary>
        /// Gets or sets the meal plan type associated with a twin room.
        /// </summary>
        public MealPlanTypes? TwinRoomMealPlanType { get; set; }

        /// <summary>
        /// Gets or sets the time of day when check-in is allowed.
        /// </summary>
        public TimeSpan CheckInTime { get; set; }

        /// <summary>
        /// Gets or sets the time at which the checkout process is completed.
        /// </summary>
        public DateTime CheckOutTime { get; set; }

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the identifier for the lodging type associated with this entity.
        /// </summary>
        [ForeignKey(nameof(LodgingType))] public string? LodgingTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of lodging associated with this entity.
        /// </summary>
        public LodgingType? LodgingType { get; set; }

        [ForeignKey(nameof(Country))]  public string? CountryId { get; set; }
        public Country? Country { get; set; }

        #endregion

        #region Many-To-One Relationships

        /// <summary>
        /// Gets or sets the collection of lodging destinations associated with this entity.
        /// </summary>
        public ICollection<LodgingDestination> Lodgings { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of rooms associated with the entity.
        /// </summary>
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

        /// <summary>
        /// Gets or sets the collection of lodging destinations associated with this entity.
        /// </summary>
        public virtual ICollection<LodgingDestination> Destinations { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of categories associated with the lodging entity.
        /// </summary>
        public virtual ICollection<EntityCategory<Lodging>> Categories { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of account types associated with the package.
        /// </summary>
        public virtual ICollection<LodgingPackage> AccountTypes { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of amenities associated with the lodging.
        /// </summary>
        public virtual ICollection<AmenityItem<Lodging, string>> Amneties { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of vouchers associated with this entity.
        /// </summary>
        public virtual ICollection<Voucher> Vouchers { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of services associated with the entity.
        /// </summary>
        public virtual ICollection<LodgingService> Services { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current lodging instance with the values provided in the specified <see cref="LodgingDto"/>
        /// object.
        /// </summary>
        /// <remarks>This method modifies the current instance by copying values from the provided <see
        /// cref="LodgingDto"/> object. Ensure that the <paramref name="lodging"/> parameter contains valid data before
        /// calling this method.</remarks>
        /// <param name="lodging">An object containing the updated lodging details. All properties of the <see cref="LodgingDto"/> are used to
        /// update the corresponding fields in the lodging instance.</param>
        /// <returns>The updated <see cref="Lodging"/> instance.</returns>
        public Lodging Update(LodgingDto lodging)
        {
            UniquePartnerId = lodging.UniqueProductPartnerId;
            Name = lodging.Name;
            Teaser = lodging.TeaserText;
            Description = lodging.Description;
            OnlineDescription = lodging.OnlineDescription;
            Attractions = lodging.Attractions;
            Grading = lodging.Grading;
            Rating = lodging.Rating;
            LowestGuestAgeCutOff = lodging.LowestGuestAgeCutOff;
            MiddleGuestAgeCutOff = lodging.MiddleGuestAgeCutOff;
            HighestGuestAgeCutOff = lodging.HighestGuestAgeCutOff;
            TermsAndConditions = lodging.TermsAndConditions;
            ChildPolicy = string.IsNullOrEmpty(lodging.ChildPolicy) ? "" : lodging.ChildPolicy;
            Contacts = lodging.Contacts;
            PhoneNr = lodging.PhoneNr;
            CellNr = lodging.CellNr;
            Email = lodging.Email;
            Website = lodging.Website;
            PageTitle = lodging.Name;
            AreaInfo = lodging.AreaInfo;
            Address = lodging.Address;
            Suburb = lodging.Suburb;
            Lat = lodging.Lat;
            Lng = lodging.Lng;
            Directions = lodging.Directions;
            CountryId = lodging.Country.CountryId;

            Settings.ApiPartner = ApiPartners.NightsBridge;

            return this;
        }

        #endregion
    }
}
