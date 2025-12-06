using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for a vacation entity.
    /// This DTO is used to transfer vacation data between different layers of the application.
    /// </summary>
    public record VacationDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor for creating a new instance of <see cref="VacationDto"/>.
        /// </summary>
        public VacationDto() { }

        /// <summary>
        /// Initializes a new instance of <see cref="VacationDto"/> using a <see cref="Vacation"/> entity.
        /// </summary>
        /// <param name="vacation">The entity containing vacation details.</param>
        public VacationDto(Vacation vacation)
        {
            VacationId = vacation.Id;
            ReferenceNr = vacation.ReferenceNr;
            Slug = vacation.Slug;

            StartDate = vacation.StartDate;
            AvailabilityCutOffDate = vacation.AvailabilityCutOffDate;
            Nights = vacation.Nights;

            RoomCount = vacation.RoomCount;
            RoomCountFoc = vacation.RoomCountFoc;
            CurrencySymbol = string.IsNullOrEmpty(vacation.CurrencySymbol) ? "R" : vacation.CurrencySymbol;
            MaxBookingCount = vacation.MaxBookingCount;
            IsExtension = vacation.IsExtension;
            Published = vacation.Published;

            GeneralInformation = vacation.GeneralInformationTemplate != null ? new GeneralInformationTemplateDto(vacation.GeneralInformationTemplate) : new GeneralInformationTemplateDto();

            MeetAndGreet = vacation.MeetAndGreetTemplate is null ? new MeetAndGreetTemplateDto() : new MeetAndGreetTemplateDto(vacation.MeetAndGreetTemplate);
            GeneralInclusionsSummaryInformation = vacation.GeneralInclusionsSummaryInformation;
            MealsAndActivitiesInclusionsSummaryInformation = vacation.MealsAndActivitiesInclusionsSummaryInformation;
            TransportAndFlightsInclusionsSummaryInformation = vacation.TransportAndFlightsInclusionsSummaryInformation;
            AccommodationInclusionSummaryInformation = vacation.AccommodationInclusionSummaryInformation;
            GolfRoundSummaryInclusionInformation = vacation.GolfRoundSummaryInclusionInformation;

            VacationTitle = vacation.VacationTitleTemplate != null ? new VacationTitleTemplateDto(vacation.VacationTitleTemplate) : new VacationTitleTemplateDto();
            ShortDescription = vacation.ShortDescriptionTemplate != null ? new ShortDescriptionTemplateDto(vacation.ShortDescriptionTemplate) : new ShortDescriptionTemplateDto();
            
            PaymentExclusionTemplate = vacation.PaymentExclusionTemplate != null ? new PaymentExclusionTemplateDto(vacation.PaymentExclusionTemplate) : new PaymentExclusionTemplateDto();
            CancellationTerms = vacation.CancellationTermsTemplate != null ? new CancellationTermsTemplateDto(vacation.CancellationTermsTemplate) : new CancellationTermsTemplateDto();
            TermsAndConditions = vacation.TermsAndConditionsTemplate != null ? new TermsAndConditionsTemplateDto(vacation.TermsAndConditionsTemplate) : new TermsAndConditionsTemplateDto();
            
            Images = vacation.Images.Select(ImageDto.ToDto).ToList();
            Meals = vacation.MealAdditions?.Select(c => new MealAdditionDto(c)).ToList();
            
            if (vacation.VacationHost is not null)
                Host = new VacationHostDto(vacation.VacationHost);

            VacationPriceGroups = vacation.VacationPriceGroups?.Select(c => new VacationPriceGroupDto { Id = c.Id, Name = c.Name}).ToList() ?? [];
            VacationPrices = vacation.Prices?.Select(c => new VacationPricingItemDto(c)).ToList();
            VacationIntervals = vacation.Intervals?.Select(c => new VacationIntervalDto(c)).ToList();
            Flights = vacation.Flights?.Select(c => new FlightDto(c)).ToList();
            GolferPackages = vacation.GolferPackages?.Select(c => new GolferPackageDto(c)).ToList();

            DayTourActivities = vacation.DayTourActivities?.Select(c => new DayTourActivityDto(c)).ToList();
            Gifts = vacation.Gifts?.Select(c => new RoomGiftDto(c)).ToList();

            if (vacation.Intervals is not null && vacation.Intervals.Any())
            {
                foreach (var interval in vacation.Intervals)
                {
                    if (interval.Lodging is not null && interval.Lodging.Destinations is not null &&
                        interval.Lodging.Destinations.Any())
                    {
                        Destinations.AddRange(interval.Lodging.Destinations.Select(c => new DestinationDto(c.Destination)).ToList());
                    }
                }
            }

            References = vacation.Reviews?.Select(c => new VacationReviewDto(c)).ToList();
            PaymentRules = vacation.PaymentRules?.Select(c => new PaymentRuleDto(c)).ToList();
            VacationInclusionDisplayTypeInfos = vacation.VacationInclusionDisplayTypeInfos?.Select(c => new VacationInclusionDisplayTypeInformationDto(c)).ToList();
            ItineraryEntryItemTemplates = vacation.ItineraryEntryItemTemplates is null ? [] : vacation.ItineraryEntryItemTemplates?.Select(c => new ItineraryEntryItemTemplateDto(c)).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or initializes the ID of the vacation.
        /// </summary>
        public string? VacationId { get; init; } 

        /// <summary>
        /// Gets or initializes the reference number of the vacation.
        /// </summary>
        public string? ReferenceNr { get; init; }

        /// <summary>
        /// Gets or sets the URL-friendly identifier for the entity.
        /// </summary>
        [Required] public string Slug { get; set; }

        /// <summary>
        /// Gets the number of nights associated with a booking or reservation.
        /// </summary>
        public int Nights { get; init; }

        /// <summary>
        /// Gets the start date of the event or process.
        /// </summary>
        public DateTime? StartDate { get; init; }

        /// <summary>
        /// Gets the start date as a formatted string in the "dd-MMM-yyyy" format.
        /// </summary>
        public string StartDateString => StartDate != null ? StartDate.Value.Date.ToString("dd-MMM-yyyy") : string.Empty;

        /// <summary>
        /// Gets the end date of the reservation, calculated based on the start date and the number of nights.
        /// </summary>
        public DateTime? EndDate => StartDate?.AddDays(Nights) ?? StartDate;

        /// <summary>
        /// Gets the end date as a formatted string in the "dd-MMM-yyyy" format.
        /// </summary>
        public string EndDateString => EndDate != null ? EndDate.Value.Date.ToString("dd-MMM-yyyy") : string.Empty;

        /// <summary>
        /// Gets the date and time after which the item is no longer available.
        /// </summary>
        public DateTime? AvailabilityCutOffDate { get; init; }

        /// <summary>
        /// Gets or initializes the number of rooms available for the vacation.
        /// </summary>
        public int RoomCount { get; init; }

        /// <summary>
        /// Gets or initializes the number of rooms available for the vacation.
        /// </summary>
        public int RoomCountFoc { get; init; }

        /// <summary>
        /// Gets or sets the currency symbol for the vacation.
        /// </summary>
        public string? CurrencySymbol { get; init; }

        /// <summary>
        /// Gets or initializes the maximum number of bookings allowed for the vacation.
        /// </summary>
        public int MaxBookingCount { get; init; }

        /// <summary>
        /// Gets or initializes the booking process for the vacation.
        /// </summary>
        public string? BookingProcess { get; init; } 

        /// <summary>
        /// Gets or sets a value indicating whether the current object is an extension.
        /// </summary>
        public bool IsExtension { get; init; }

        /// <summary>
        /// Gets a value indicating whether the item is published.
        /// </summary>
        public bool Published { get; init; }

        /// <summary>
        /// Gets the summary information for general inclusions.
        /// </summary>
        public string? GeneralInclusionsSummaryInformation { get; init; }

        /// <summary>
        /// Gets or sets a brief description of the meals and activities inclusions summary information.
        /// </summary>
        public string? MealsAndActivitiesInclusionsSummaryInformation { get; init; }

        /// <summary>
        /// Gets or sets the summary information regarding transport and flight inclusions.
        /// </summary>
        public string? TransportAndFlightsInclusionsSummaryInformation { get; init; }

        /// <summary>
        /// Gets or sets the summary information for a golf round.
        /// </summary>
        public string? GolfRoundSummaryInclusionInformation { get; set; }

        /// <summary>
        /// Gets or sets the summary information regarding the inclusion of accommodations.
        /// </summary>
        public string? AccommodationInclusionSummaryInformation { get; set; }

        /// <summary>
        /// Gets the template for the vacation title.
        /// </summary>
        public VacationTitleTemplateDto? VacationTitle { get; init; }

        /// <summary>
        /// Gets or sets the short description template for the associated entity.
        /// </summary>
        public ShortDescriptionTemplateDto? ShortDescription { get; init; }

        /// <summary>
        /// Gets or sets the general information associated with the template.
        /// </summary>
        public GeneralInformationTemplateDto? GeneralInformation { get; init; }

        /// <summary>
        /// Gets or sets the booking terms for the booking form.
        /// </summary>
        public BookingTermsDescriptionTemplateDto? BookingTermTemplate { get; init; }

        /// <summary>
        /// Gets or sets the payment exclusion template associated with the current operation.
        /// </summary>
        public PaymentExclusionTemplateDto? PaymentExclusionTemplate { get; init; } 

        /// <summary>
        /// Gets or sets the cancellation terms associated with the current booking.
        /// </summary>
        public CancellationTermsTemplateDto? CancellationTerms { get; init; }

        /// <summary>
        /// Gets or sets the terms and conditions template.
        /// </summary>
        public TermsAndConditionsTemplateDto? TermsAndConditions { get; init; }

        /// <summary>
        /// Gets or sets the host details for the vacation.
        /// </summary>
        public VacationHostDto? Host { get; init; }

        /// <summary>
        /// Gets or sets the primary consultant for the vacation.
        /// </summary>
        public ContactDto? Consultant { get; init; } 

        /// <summary>
        /// Gets or sets the primary coordinator for the vacation.
        /// </summary>
        public ContactDto? Coordinator { get; init; } 

        /// <summary>
        /// Gets or sets the primary tour director for the vacation.
        /// </summary>
        public ContactDto? TourDirector { get; init; } 

        /// <summary>
        /// Gets or sets the duration allocated for the meet-and-greet session.
        /// </summary>
        public TimeSpan? MeetAndGreetTime { get; init; }

        /// <summary>
        /// Gets or sets the template for the meet-and-greet event.
        /// </summary>
        public MeetAndGreetTemplateDto? MeetAndGreet { get; init; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the collection of images to be displayed.
        /// </summary>
        public ICollection<ImageDto> Images { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of breakfasts associated with the vacation extension.
        /// Each item in the list represents a meal addition for breakfast, including details such as date, time, and location.
        /// </summary>
        public List<MealAdditionDto>? Meals { get; init; } = new List<MealAdditionDto>();

        /// <summary>
        /// Gets or sets the collection of vacation price groups.
        /// </summary>
        public List<VacationPriceGroupDto> VacationPriceGroups { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of vacation pricing items.
        /// </summary>
        public List<VacationPricingItemDto>? VacationPrices { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of vacation intervals.
        /// </summary>
        public List<VacationIntervalDto>? VacationIntervals { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of flights associated with the vacation.
        /// </summary>
        public List<FlightDto>? Flights { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of golf courses associated with the vacation.
        /// </summary>
        public List<GolferPackageDto>? GolferPackages { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of day tours/activities associated with the vacation.
        /// </summary>
        public List<DayTourActivityDto>? DayTourActivities { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of references for the vacation.
        /// </summary>
        public List<VacationReviewDto>? References { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of payment rules for the vacation.
        /// </summary>
        public List<PaymentRuleDto>? PaymentRules { get; init; } = [];

        /// <summary>
        /// Gets or sets the collection of room gifts associated with the vacation.
        /// </summary>
        public List<RoomGiftDto>? Gifts { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of day Vacation Inclusion Display Type Infos associated with the vacation.
        /// </summary>
        public List<VacationInclusionDisplayTypeInformationDto>? VacationInclusionDisplayTypeInfos { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of vacation inclusions.
        /// </summary>
        public List<VacationInclusionDto>? VacationInclusions { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of itineraries for the vacation.
        /// </summary>
        public List<ItineraryDto>? Itineraries { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of destinations for the vacation.
        /// </summary>
        public List<DestinationDto>? Destinations { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of lodgings for the vacation.
        /// </summary>
        public List<LodgingDto>? Lodgings { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of cancellation rules for the vacation.
        /// </summary>
        public List<VacationCancellationRuleDto>? CancellationRules { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of vacation extensions.
        /// </summary>
        public List<VacationDto> VacationExtensions { get; init; } = [];

        /// <summary>
        /// Gets or sets the collection of itinerary entry item templates.
        /// </summary>
        public List<ItineraryEntryItemTemplateDto> ItineraryEntryItemTemplates { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the DTO to a <see cref="Vacation"/> entity.
        /// </summary>
        /// <returns>A <see cref="Vacation"/> entity with the DTO's data.</returns>
        public Vacation ToVacation(string uploadPath)
        {
            var vacation = new Vacation
            {
                Id = VacationId,
                ReferenceNr = ReferenceNr,
                Slug = Slug,
                VacationTitleTemplateId = VacationTitle?.Id,
                ShortDescriptionTemplateId = ShortDescription?.Id,
                VacationHostId = Host?.VacationHostId,
                StartDate = StartDate,
                Nights = Nights,
                AvailabilityCutOffDate = AvailabilityCutOffDate,
                RoomCount = RoomCount,
                RoomCountFoc = RoomCountFoc,
                CurrencySymbol = CurrencySymbol,
                MaxBookingCount = MaxBookingCount,
                IsExtension = IsExtension,
                Published = Published,
                GeneralInformationTemplateId = GeneralInformation?.Id,
                GeneralInclusionsSummaryInformation = GeneralInclusionsSummaryInformation,
                MealsAndActivitiesInclusionsSummaryInformation = MealsAndActivitiesInclusionsSummaryInformation,
                TransportAndFlightsInclusionsSummaryInformation = TransportAndFlightsInclusionsSummaryInformation,
                AccommodationInclusionSummaryInformation = AccommodationInclusionSummaryInformation,
                GolfRoundSummaryInclusionInformation = GolfRoundSummaryInclusionInformation,
                CancellationTermsTemplateId = CancellationTerms?.Id,
                TermsAndConditionsTemplateId = TermsAndConditions?.Id,
                PaymentExclusionTemplateId = PaymentExclusionTemplate?.Id
            };

            foreach (var pricingItem in VacationPriceGroups)
            {
                vacation.VacationPriceGroups?.Add(new VacationPriceGroup() { Id = pricingItem.Id, Name = pricingItem.Name});
            }

            if (VacationPrices != null)
                foreach (var pricingItem in VacationPrices)
                {
                    vacation.Prices?.Add(pricingItem.ToVacationPrice());
                }

            if (VacationInclusionDisplayTypeInfos != null)
                foreach (var pricingItem in VacationInclusionDisplayTypeInfos)
                {
                    vacation.VacationInclusionDisplayTypeInfos?.Add(
                        pricingItem.ToVacationInclusionDisplayTypeInformation());
                }

            if (VacationIntervals != null)
                foreach (var interval in VacationIntervals)
                {
                    vacation.Intervals?.Add(interval.ToVacationInterval());
                }

            if (Flights != null)
                foreach (var flight in Flights)
                {
                    vacation.Flights?.Add(flight.ToFlight());
                }

            if (GolferPackages != null)
                foreach (var golferPackage in GolferPackages)
                {
                    vacation.GolferPackages?.Add(golferPackage.ToGolferPackage());
                }

            if (DayTourActivities != null)
                foreach (var dayTourActivity in DayTourActivities)
                {
                    vacation.DayTourActivities?.Add(dayTourActivity.ToDayTourActivity());
                }

            if (PaymentRules != null)
                foreach (var paymentRule in PaymentRules)
                {
                    vacation.PaymentRules?.Add(paymentRule.ToPaymentRule());
                }

            if (VacationInclusionDisplayTypeInfos != null)
                foreach (var mealAddition in VacationInclusionDisplayTypeInfos)
                {
                    vacation.VacationInclusionDisplayTypeInfos?.Add(mealAddition
                        .ToVacationInclusionDisplayTypeInformation());
                }

            if (Gifts == null) return vacation;
            foreach (var gift in Gifts)
            {
                vacation.Gifts?.Add(gift.ToVacationRoomGift());
            }

            return vacation;
        }

        /// <summary>
        /// Updates the specified <see cref="Vacation"/> object with the current vacation details.
        /// </summary>
        /// <remarks>This method assigns various properties from the current context to the provided
        /// <paramref name="vacation"/> object, including reference numbers, template IDs, and other vacation-specific
        /// details. Ensure that the <paramref name="vacation"/> object is properly initialized before calling this
        /// method.</remarks>
        /// <param name="vacation">The <see cref="Vacation"/> object to be updated. This parameter must not be null.</param>
        public void UpdateVacation(in Vacation vacation)
        {
            vacation.ReferenceNr = ReferenceNr;
            vacation.Slug = Slug;
            vacation.VacationTitleTemplateId = VacationTitle?.Id;
            vacation.ShortDescriptionTemplateId = ShortDescription?.Id;
            vacation.VacationHostId = Host?.VacationHostId;
            vacation.StartDate = StartDate;
            vacation.Nights = Nights;
            vacation.AvailabilityCutOffDate = AvailabilityCutOffDate;
            vacation.RoomCount = RoomCount;
            vacation.RoomCountFoc = RoomCountFoc;
            vacation.CurrencySymbol = CurrencySymbol;
            vacation.MaxBookingCount = MaxBookingCount;
            vacation.IsExtension = IsExtension;
            vacation.Published = Published;
            vacation.GeneralInformationTemplateId = GeneralInformation?.Id;
            vacation.MeetAndGreetTemplateId = MeetAndGreet?.Id;
            vacation.GeneralInclusionsSummaryInformation = GeneralInclusionsSummaryInformation;
            vacation.MealsAndActivitiesInclusionsSummaryInformation = MealsAndActivitiesInclusionsSummaryInformation;
            vacation.TransportAndFlightsInclusionsSummaryInformation = TransportAndFlightsInclusionsSummaryInformation;
            vacation.AccommodationInclusionSummaryInformation = AccommodationInclusionSummaryInformation;
            vacation.GolfRoundSummaryInclusionInformation = GolfRoundSummaryInclusionInformation;
            vacation.PaymentExclusionTemplateId = PaymentExclusionTemplate?.Id;
            vacation.CancellationTermsTemplateId = CancellationTerms?.Id;
            vacation.TermsAndConditionsTemplateId = TermsAndConditions?.Id;
        }

        #endregion
    }
}
