using System.ComponentModel.DataAnnotations.Schema;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a vacation entity, including details such as name, description, dates, and associated collections.
    /// Inherits from <see cref="FileCollection{TEntity,TId}"/> to manage associated images.
    /// </summary>
    public class Vacation : FileCollection<Vacation, string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Vacation"/> class.
        /// </summary>
        public Vacation() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vacation"/> class by copying the details from an existing
        /// vacation.
        /// </summary>
        /// <remarks>This constructor creates a deep copy of the specified <paramref name="vacation"/>
        /// object.  All properties and collections are duplicated, ensuring that changes to the new instance do not
        /// affect the original instance.</remarks>
        /// <param name="vacation">The <see cref="Vacation"/> instance to copy from. This parameter cannot be <see langword="null"/>.</param>
        public Vacation(Vacation vacation)
        {
            ReferenceNr = vacation.ReferenceNr;
            Slug = vacation.Slug;
            StartDate = vacation.StartDate;
            Nights = vacation.Nights;
            RoomCount = vacation.RoomCount;
            RoomCountFoc = vacation.RoomCountFoc;
            MaxBookingCount = vacation.MaxBookingCount;
            GeneralInclusionsSummaryInformation = vacation.GeneralInclusionsSummaryInformation;
            MealsAndActivitiesInclusionsSummaryInformation = vacation.MealsAndActivitiesInclusionsSummaryInformation;
            TransportAndFlightsInclusionsSummaryInformation = vacation.TransportAndFlightsInclusionsSummaryInformation;
            AccommodationInclusionSummaryInformation = vacation.AccommodationInclusionSummaryInformation;
            GolfRoundSummaryInclusionInformation = vacation.GolfRoundSummaryInclusionInformation;
            IsExtension = vacation.IsExtension;
            Published = vacation.Published;
            MeetAndGreetTime = vacation.MeetAndGreetTime;
            MeetAndGreetTemplateId = vacation.MeetAndGreetTemplateId;
            VacationTitleTemplateId = vacation.VacationTitleTemplateId;
            BookingTermsTemplateId = vacation.BookingTermsTemplateId;
            CancellationTermsTemplateId = vacation.CancellationTermsTemplateId;
            TermsAndConditionsTemplateId = vacation.TermsAndConditionsTemplateId;
            PaymentExclusionTemplateId = vacation.PaymentExclusionTemplateId;
            ShortDescriptionTemplateId = vacation.ShortDescriptionTemplateId;
            GeneralInformationTemplateId = vacation.GeneralInformationTemplateId;
            VacationHostId = vacation.VacationHostId;
            CurrencySymbol = vacation.CurrencySymbol;

            if (vacation.Contacts != null)
            {
                foreach (var contact in vacation.Contacts)
                {
                    Contacts?.Add(new VacationContact() { Id = Guid.NewGuid().ToString(), ContactId = contact.ContactId, VacationId = Id });
                }
            }

            if (vacation.VacationPriceGroups != null)
            {
                foreach (var price in vacation.VacationPriceGroups)
                {
                    VacationPriceGroups?.Add(new VacationPriceGroup() { Id = Guid.NewGuid().ToString(), Name = price.Name, VacationId = Id});
                }
            }

            if (vacation.Prices != null)
            {
                foreach (var price in vacation.Prices)
                {
                    Prices?.Add(price.Clone());
                }
            }

            if (vacation.Intervals != null)
            {
                foreach (var interval in vacation.Intervals)
                {
                    Intervals?.Add(interval.Clone());
                }
            }

            if (vacation.Extensions != null)
            {
                foreach (var extension in vacation.Extensions)
                {
                    Extensions?.Add(new VacationExtensionAddition() { ParentVacationId = Id, ExtensioId = extension.ExtensioId});
                }
            }

            if (vacation.ParentVacations != null)
            {
                foreach (var parent in vacation.ParentVacations)
                {
                    Extensions?.Add(new VacationExtensionAddition() { ParentVacationId = parent.ParentVacationId, ExtensioId = Id });
                }
            }

            if (vacation.Flights != null)
            {
                foreach (var flight in vacation.Flights)
                {
                    Flights?.Add(flight.Clone());
                }
            }

            if (vacation.GolferPackages != null)
            {
                foreach (var golferPackage in vacation.GolferPackages)
                {
                    GolferPackages?.Add(golferPackage.Clone());
                }
            }

            if (vacation.DayTourActivities != null)
            {
                foreach (var dayTourActivity in vacation.DayTourActivities)
                {
                    DayTourActivities?.Add(dayTourActivity.Clone());
                }
            }

            if (vacation.MealAdditions != null)
            {
                foreach (var mealAddition in vacation.MealAdditions)
                {
                    MealAdditions?.Add(mealAddition.Clone());
                }
            }

            if (vacation.Reviews != null)
            {
                foreach (var review in vacation.Reviews)
                {
                    Reviews?.Add(new VacationReview() { Id = Guid.NewGuid().ToString(), VacationId = Id, ReviewId = review.ReviewId });
                }
            }

            if (vacation.PaymentRules != null)
            {
                foreach (var paymentRule in vacation.PaymentRules)
                {
                    PaymentRules?.Add(paymentRule.Clone());
                }
            }

            if (vacation.VacationInclusionDisplayTypeInfos != null)
            {
                foreach (var inclusionDisplayTypeInfo in vacation.VacationInclusionDisplayTypeInfos)
                {
                    VacationInclusionDisplayTypeInfos?.Add(inclusionDisplayTypeInfo.Clone());
                }
            }

            if (vacation.Gifts != null)
            {
                foreach (var gift in vacation.Gifts)
                {
                    Gifts?.Add(gift.Clone());
                }
            }

            if (vacation.ItineraryEntryItemTemplates != null)
            {
                foreach (var gift in vacation.ItineraryEntryItemTemplates)
                {
                    ItineraryEntryItemTemplates?.Add(new ItineraryEntryItemTemplate()
                    {
                        VacationId = vacation.Id,
                        Id = Guid.NewGuid().ToString(),
                        DayNr = gift.DayNr,
                        Content = gift.Content
                    });
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the vacation.
        /// </summary>
        public string? ReferenceNr { get; set; }

        /// <summary>
        /// Gets the URL-friendly version of the vacation name.
        /// </summary>
        public string Url => VacationTitleTemplate != null ? VacationTitleTemplate.VacationTitle.Replace(" ", "-") : "";

        /// <summary>
        /// Gets or sets the URL-friendly identifier for the entity.
        /// </summary>
        public string? Slug { get; set; }

        /// <summary>
        /// Gets or sets the start date of the vacation.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time after which the item is no longer available.
        /// </summary>
        public DateTime? AvailabilityCutOffDate { get; set; }

        /// <summary>
        /// Gets or sets the number of nights for a reservation or booking.
        /// </summary>
        public int Nights { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms available for the vacation.
        /// </summary>
        public int RoomCount { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms available for the vacation.
        /// </summary>
        public int RoomCountFoc { get; set; }

        /// <summary>
        /// Gets or sets the currency symbol for the vacation.
        /// </summary>
        public string? CurrencySymbol { get; set; } = "R";

        /// <summary>
        /// Gets or sets the maximum number of bookings allowed for the vacation.
        /// </summary>
        public int MaxBookingCount { get; set; }

        /// <summary>
        /// Gets or sets the general summary information.
        /// </summary>
        public string? GeneralInclusionsSummaryInformation { get; set; } 
        
        /// <summary>
        /// Gets or sets the summary information for meals and activities inclusions.
        /// </summary>
        public string? MealsAndActivitiesInclusionsSummaryInformation { get; set; } 
        
        /// <summary>
        /// Gets or sets the summary information for transport and flight inclusions.
        /// </summary>
        public string? TransportAndFlightsInclusionsSummaryInformation { get; set; } 

        /// <summary>
        /// Gets or sets the summary information for a golf round.
        /// </summary>
        public string? GolfRoundSummaryInclusionInformation { get; set; }

        /// <summary>
        /// Gets or sets the summary information regarding the inclusion of accommodations.
        /// </summary>
        public string? AccommodationInclusionSummaryInformation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current object is an extension.
        /// </summary>
        public bool IsExtension { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is published.
        /// </summary>
        public bool Published { get; set; }

        #endregion

        #region Meet & Greet

        /// <summary>
        /// Gets or sets the date for the meeting or gathering associated with the vacation.
        /// This property specifies the exact date when the meeting or gathering will take place.
        /// </summary>
        public TimeSpan? MeetAndGreetTime { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated Meet and Greet template.
        /// </summary>
        [ForeignKey(nameof(MeetAndGreetTemplate))] public string? MeetAndGreetTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template used for generating meet-and-greet messages.
        /// </summary>
        public MeetAndGreetTemplate? MeetAndGreetTemplate { get; set; }

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the identifier of the associated vacation title template.
        /// </summary>
        [ForeignKey(nameof(VacationTitleTemplate))] public string? VacationTitleTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template used to generate vacation titles.
        /// </summary>
        public VacationTitleTemplate? VacationTitleTemplate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated booking terms template.
        /// </summary>
        [ForeignKey(nameof(BookingTermsTemplate))] public string? BookingTermsTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template used for generating booking terms descriptions.
        /// </summary>
        public BookingTermsDescriptionTemplate? BookingTermsTemplate { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the cancellation terms template.
        /// </summary>
        [ForeignKey(nameof(CancellationTermsTemplate))] public string? CancellationTermsTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template for cancellation terms.
        /// </summary>
        public CancellationTermsTemplate? CancellationTermsTemplate { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the terms and conditions template.
        /// </summary>
        [ForeignKey(nameof(TermsAndConditionsTemplate))] public string? TermsAndConditionsTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template for terms and conditions.
        /// </summary>
        public TermsAndConditionsTemplate? TermsAndConditionsTemplate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated payment exclusion template.
        /// </summary>
        [ForeignKey(nameof(PaymentExclusionTemplate))] public string? PaymentExclusionTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template that defines payment exclusion rules for the transaction.
        /// </summary>
        public PaymentExclusionTemplate? PaymentExclusionTemplate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the short description template associated with this entity.
        /// </summary>
        [ForeignKey(nameof(ShortDescriptionTemplate))] public string? ShortDescriptionTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template used to generate a short description.
        /// </summary>
        public ShortDescriptionTemplate? ShortDescriptionTemplate { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier for the associated General Information Template.
        /// </summary>
        [ForeignKey(nameof(GeneralInformationTemplate))] public string? GeneralInformationTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template used to define general information.
        /// </summary>
        public GeneralInformationTemplate? GeneralInformationTemplate { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated vacation host.
        /// </summary>
        [ForeignKey(nameof(VacationHost))] public string? VacationHostId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation host.
        /// </summary>
        public VacationHost? VacationHost { get; set; }

        #endregion

        #region Many-To-One Relationships
        
        /// <summary>
        /// Gets or sets the collection of prices associated with the vacation.
        /// </summary>
        public ICollection<VacationContact>? Contacts { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of vacation price groups associated with this entity.
        /// </summary>
        public ICollection<VacationPriceGroup>? VacationPriceGroups { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of prices associated with the vacation.
        /// </summary>
        public ICollection<VacationPrice>? Prices { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of vacation periods associated with the vacation.
        /// </summary>
        public ICollection<VacationInterval>? Intervals { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of extensions associated with the vacation.
        /// </summary>
        public ICollection<VacationExtensionAddition>? Extensions { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of extensions associated with the vacation.
        /// </summary>
        public ICollection<VacationExtensionAddition>? ParentVacations { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of flights associated with the vacation.
        /// </summary>
        public ICollection<Flight>? Flights { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of itineraries associated with the vacation.
        /// </summary>
        public ICollection<GolferPackage>? GolferPackages { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of itinerary items associated with the vacation.
        /// </summary>
        public ICollection<DayTourActivity>? DayTourActivities { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of itinerary items associated with the vacation.
        /// </summary>
        public ICollection<MealAddition>? MealAdditions { get; set; } = [];
        
        /// <summary>
        /// Gets or sets the collection of reviews associated with the vacation.
        /// </summary>
        public ICollection<VacationReview>? Reviews { get; set; } = [];

        /// <summary>
        /// The payment rules for associated with this vacation
        /// </summary>
        public ICollection<PaymentRule>? PaymentRules { get; set; } = [];

        /// <summary>
        /// The information for the inclusion display types display section
        /// </summary>
        public ICollection<VacationInclusionDisplayTypeInformation>? VacationInclusionDisplayTypeInfos { get; set; } = [];
        
        /// <summary>
        /// Gets or sets the collection of room gifts associated with the vacation.
        /// </summary>
        public ICollection<RoomGift>? Gifts { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of itinerary entry item templates.
        /// </summary>
        public ICollection<ItineraryEntryItemTemplate> ItineraryEntryItemTemplates { get; set; } = [];

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new instance of the <see cref="Vacation"/> class that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="Vacation"/> object that is a copy of this instance, with a unique identifier.</returns>
        public Vacation Clone()
        {
            return new Vacation(this) { Id = Guid.NewGuid().ToString() };
        }

        #endregion
    }
}
