using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Extensions;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for managing vacation details.
    /// </summary>
    public record VacationViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor for creating a new instance of <see cref="VacationViewModel"/>.
        /// </summary>
        public VacationViewModel() { }

        /// <summary>
        /// Initializes a new instance of <see cref="VacationViewModel"/> using a <see cref="VacationDto"/>.
        /// </summary>
        /// <param name="vacation">The DTO containing vacation details.</param>
        public VacationViewModel(VacationDto vacation)
        {
            VacationId = vacation.VacationId;
            ReferenceNr = vacation.ReferenceNr;
            Slug = vacation.Slug;

            VacationTitle = vacation.VacationTitle;
            ShortDescription = vacation.ShortDescription;

            RoomCount = vacation.RoomCount;
            RoomCountFOC = vacation.RoomCountFoc;
            CurrencySymbol = vacation.CurrencySymbol;
            MaxBookingCount = vacation.MaxBookingCount;
            IsExtension = vacation.IsExtension;
            Published = vacation.Published;

            GeneralInformation = vacation.GeneralInformation;

            GeneralInclusionsSummaryInformation = vacation?.GeneralInclusionsSummaryInformation;
            MealsAndActivitiesInclusionsSummaryInformation = vacation?.MealsAndActivitiesInclusionsSummaryInformation;
            TransportAndFlightsInclusionsSummaryInformation = vacation?.TransportAndFlightsInclusionsSummaryInformation;
            GolfRoundSummaryInclusionInformation = vacation?.GolfRoundSummaryInclusionInformation;
            AccommodationInclusionSummaryInformation = vacation?.AccommodationInclusionSummaryInformation;
            MeetAndGreetTemplate = vacation.MeetAndGreet;

            StartDate = vacation.StartDate;
            AvailabilityCutOffDate = vacation.AvailabilityCutOffDate;
            Nights = vacation.ItineraryEntryItemTemplates.Any() ? vacation.ItineraryEntryItemTemplates.Count-1 : vacation.Nights;

            Images = vacation.Images.ToList();

            ShortDescription = vacation.ShortDescription;
            GeneralInformation = vacation.GeneralInformation;
            CancellationTerms = vacation.CancellationTerms;

            Host = vacation.Host;
            Consultant = vacation.Consultant;
            Coordinator = vacation.Coordinator;
            TourDirector = vacation.TourDirector;

            VacationIntervals = vacation.VacationIntervals;
            Flights = vacation.Flights;
            GolferPackages = vacation.GolferPackages;
            DayTourActivities = vacation.DayTourActivities;
            VacationInclusionDisplayTypeInfos = vacation.VacationInclusionDisplayTypeInfos;
            Gifts = vacation.Gifts;
            Meals = vacation.Meals;

            VacationPriceGroups = vacation.VacationPriceGroups;
            VacationPrices = vacation.VacationPrices;
            VacationInclusions = vacation.VacationInclusions;
            References = vacation.References;
            Itineraries = vacation.Itineraries;
            CancellationRules = vacation.CancellationRules;
            PaymentRules = vacation.PaymentRules;
            ItineraryEntryItemTemplates = vacation.ItineraryEntryItemTemplates;

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the reference number associated with the entity.
        /// </summary>
        public string? ReferenceNr { get; set; }

        /// <summary>
        /// Gets or sets the URL-friendly identifier for the entity.
        /// </summary>
        public string? Slug { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        public string VacationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the URL-friendly version of the vacation title, with spaces replaced by hyphens.
        /// </summary>
        public string? Url => VacationTitle?.VacationTitle?.Replace(" ", "-");

        /// <summary>
        /// Gets or sets the number of rooms available.
        /// </summary>
        public int RoomCount { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms provided free of charge (FOC).
        /// </summary>
        public int RoomCountFOC { get; set; }

        /// <summary>
        /// Gets or sets the symbol used to represent the currency.
        /// </summary>
        public string? CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of bookings allowed.
        /// </summary>
        public int MaxBookingCount { get; set; }

        /// <summary>
        /// Gets or sets the start date for the operation or event.
        /// </summary>
        public DateTime? StartDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets the calculated end date based on the start date and the number of nights.
        /// </summary>
        public DateTime? EndDate => StartDate?.AddDays(Nights);

        /// <summary>
        /// Gets or sets the date and time after which the item is no longer available.
        /// </summary>
        public DateTime? AvailabilityCutOffDate { get; set; }

        /// <summary>
        /// Gets or sets the number of nights associated with a booking or reservation.
        /// </summary>
        public int Nights { get; set; }

        /// <summary>
        /// Gets the total number of days for the stay, calculated as the number of nights plus one.
        /// </summary>
        public double DayCount => Nights + 1;

        /// <summary>
        /// Gets or sets a value indicating whether the current object is an extension.
        /// </summary>
        public bool IsExtension { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the item is published.
        /// </summary>
        public bool Published { get; set; } = false;

        /// <summary>
        /// Gets the summary information for general inclusions.
        /// </summary>
        public string? GeneralInclusionsSummaryInformation { get; set; } =
            "<p><ul><li>Qualified South African Tour Director / professional guide accompanying the group 24/7 from the date of arrival to the date of departure</li><li>Special arrival gifts</li><li>All porterage costs at the airports and hotels</li><li>All gratuities – Drivers, Guides, Tour Director, Meals, Safari staff</li></ul></p>";


        /// <summary>
        /// Gets or sets a brief description of the meals and activities inclusions summary information.
        /// </summary>
        public string? MealsAndActivitiesInclusionsSummaryInformation { get; set; } =
            "<p> <ul> <li>Full English breakfast daily</li> <li>All dinners during your stay at Fancourt</li> <li>3 special dinners during your stay in Cape Town</li> <li>2 special lunches during your stay in Cape Town</li> <li>- Selected wine and soft drinks at all hosted dinners</li> <li>Guided tour of Table Mountain</li> <li>- All meals and selected drinks during your stay at Jock Safari Lodge</li> <li>Full day guided tour of the Cape Winelands including private wine experience</li> <li>Full day guided tour of The Cape of Good Hope and the African Jackass Penguin colony</li> <li>Full non-golfers program on the day of golf</li> <li>Morning and afternoon 'Big 5' game experience in the Kruger National Park</li></ul> </p>";


        /// <summary>
        /// Gets or sets the summary information regarding transport and flight inclusions.
        /// </summary>
        public string? TransportAndFlightsInclusionsSummaryInformation { get; set; } =
            "<p><ul><li>All road transportation in luxury air-conditioned vehicles</li><br><em><strong>All domestic flights including allowances for golf equipment:</strong></em><li>Johannesburg to George</li><li>- George to Cape Town</li><li>Cape Town to the Kruger National Park</li><li>Kruger National Park to Johannesburg</li></ul></p>";

        /// <summary>
        /// Gets or sets the summary information for a golf round.
        /// </summary>
        public string? GolfRoundSummaryInclusionInformation { get; set; }

        /// <summary>
        /// Gets or sets the summary information regarding the inclusion of accommodations.
        /// </summary>
        public string? AccommodationInclusionSummaryInformation { get; set; }


        #region Images

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        /// <summary>
        /// Retrieves the URL of the main slider image if available; otherwise, returns a default placeholder image URL.
        /// </summary>
        /// <remarks>The method checks for an image with <see cref="CategoryTypes.Slider"/> and a selector
        /// value of "Main" in the <c>Images</c> collection. If such an image is found, its relative path is combined
        /// with the base API address from the configuration to form the full URL. If no matching image is found, a
        /// default placeholder image URL is returned.</remarks>
        /// <param name="configuration">The application configuration used to retrieve the base API address.</param>
        /// <returns>A string containing the URL of the main slider image if one exists; otherwise, a URL pointing to a default
        /// placeholder image.</returns>
        public string MainSliderImageUrl(IConfiguration configuration) => Images.Any(c => c.ImageType == UploadType.Slider && c.Selector == "Main") ? $"{configuration["ApiConfiguration:BaseApiAddress"]}/{Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Main").RelativePath}" : "_content/Accomodation.Blazor/images/NoSliderImageAvailable.jpg";
        
        /// <summary>
        /// Retrieves the URL of the summary slider image if available; otherwise, returns a default placeholder image
        /// URL.
        /// </summary>
        /// <param name="configuration">The application configuration used to retrieve the base API address.</param>
        /// <returns>The URL of the summary slider image if one exists; otherwise, a default placeholder image URL.</returns>
        public string SummarySliderImageUrl(IConfiguration configuration) => Images.Any(c => c.ImageType == UploadType.Slider && c.Selector == "Summary") ? $"{configuration["ApiConfiguration:BaseApiAddress"]}/{Images.FirstOrDefault(c => c.ImageType == UploadType.Slider && c.Selector == "Summary").RelativePath}" : "_content/Accomodation.Blazor/images/NoSliderImageAvailable.jpg";

        /// <summary>
        /// Retrieves the URL of the first banner image (Banner 1) if available; otherwise, returns a default "no image
        /// available" URL.
        /// </summary>
        /// <remarks>The method checks for an image with <see cref="UploadType.Banner"/> and an order of 1
        /// in the <c>Images</c> collection. If such an image exists, its relative path is combined with the base API
        /// address from the configuration to form the full URL. If no such image is found, a default placeholder image
        /// URL is returned.</remarks>
        /// <param name="configuration">The application configuration used to retrieve the base API address.</param>
        /// <returns>A string containing the URL of the first banner image if one exists; otherwise, a default URL pointing to a
        /// placeholder image.</returns>
        public string Banner1ImageUrl(IConfiguration configuration) => Images.Any(c => c.ImageType == UploadType.Banner && c.Order == 1) ? $"{configuration["ApiConfiguration:BaseApiAddress"]}/{Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 1).RelativePath}" : "_content/Accomodation.Blazor/images/NoBannerImageAvailable.jpg";
        
        /// <summary>
        /// Retrieves the URL of the second banner image if available; otherwise, returns a default placeholder image
        /// URL.
        /// </summary>
        /// <remarks>The method checks for an image with <see cref="UploadType.Banner"/> and an order of 2
        /// in the <c>Images</c> collection. If such an image exists, its relative path is combined with the base API
        /// address from the configuration to form the full URL. If no such image is found, a default placeholder image
        /// URL is returned.</remarks>
        /// <param name="configuration">The application configuration used to retrieve the base API address for constructing the image URL.</param>
        /// <returns>A string containing the URL of the second banner image if it exists; otherwise, a URL pointing to a default
        /// placeholder image.</returns>
        public string Banner2ImageUrl(IConfiguration configuration) => Images.Any(c => c.ImageType == UploadType.Banner && c.Order == 2) ? $"{configuration["ApiConfiguration:BaseApiAddress"]}/{Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 2).RelativePath}" : "_content/Accomodation.Blazor/images/NoBannerImageAvailable.jpg";
        
        /// <summary>
        /// Retrieves the URL of the third banner image if available; otherwise, returns a default placeholder image
        /// URL.
        /// </summary>
        /// <remarks>The method checks the collection of images for an entry with an <see
        /// cref="UploadType"/> of <see langword="Banner"/> and an order of 3. If such an image exists, its relative
        /// path is combined with the base API address from the configuration to form the full URL. If no matching image
        /// is found, a default placeholder image URL is returned.</remarks>
        /// <param name="configuration">The application configuration used to retrieve the base API address for constructing the image URL.</param>
        /// <returns>A string containing the URL of the third banner image if it exists; otherwise, a URL pointing to a default
        /// placeholder image.</returns>
        public string Banner3ImageUrl(IConfiguration configuration) => Images.Any(c => c.ImageType == UploadType.Banner && c.Order == 3) ? $"{configuration["ApiConfiguration:BaseApiAddress"]}/{Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 3).RelativePath}" : "_content /Accomodation.Blazor/images/NoBannerImageAvailable.jpg";
        
        /// <summary>
        /// Retrieves the URL for the fourth banner image if available; otherwise, returns a default "no image
        /// available" URL.
        /// </summary>
        /// <remarks>The method checks the collection of images for an entry with an <see
        /// cref="UploadType"/> of <see langword="Banner"/> and an order of 4. If such an image exists, its relative
        /// path is combined with the base API address from the configuration to form the full URL. If no matching image
        /// is found, a default placeholder URL is returned.</remarks>
        /// <param name="configuration">The application configuration used to retrieve the base API address for constructing the image URL.</param>
        /// <returns>A string containing the URL of the fourth banner image if it exists; otherwise, a default URL pointing to a
        /// "no image available" placeholder.</returns>
        public string Banner4ImageUrl(IConfiguration configuration) => Images.Any(c => c.ImageType == UploadType.Banner && c.Order == 4) ? $"{configuration["ApiConfiguration:BaseApiAddress"]}/{Images.FirstOrDefault(c => c.ImageType == UploadType.Banner && c.Order == 4).RelativePath}" : "_content /Accomodation.Blazor/images/NoBannerImageAvailable.jpg";

        /// <summary>
        /// Retrieves the URL of the map image associated with the current object.
        /// </summary>
        /// <remarks>This method checks the collection of images for an entry with an <see
        /// cref="UploadType"/> of <see cref="UploadType.Map"/>. If such an image is found, its relative path is
        /// combined with the base API address from the configuration to form the full URL. If no map image is
        /// available, a default placeholder image URL is returned.</remarks>
        /// <param name="configuration">The application configuration used to retrieve the base API address.</param>
        /// <returns>The URL of the map image if one exists; otherwise, a default placeholder image URL.</returns>
        public string MapImageUrl(IConfiguration configuration) => Images.Any(c => c.ImageType == UploadType.Map) ? $"{configuration["ApiConfiguration:BaseApiAddress"]}/{Images.FirstOrDefault(c => c.ImageType == UploadType.Map).RelativePath}" : "_content/Accomodation.Blazor/images/NoBannerImageAvailable.jpg";

        #endregion

        /// <summary>
        /// Gets or sets the template for the vacation title.
        /// </summary>
        public VacationTitleTemplateDto? VacationTitle { get; set; }

        /// <summary>
        /// Gets or sets the short description template for the associated entity.
        /// </summary>
        public ShortDescriptionTemplateDto ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the general information associated with the template.
        /// </summary>
        public GeneralInformationTemplateDto GeneralInformation { get; set; }

        /// <summary>
        /// Gets or sets the template containing the booking terms description.
        /// </summary>
        public BookingTermsDescriptionTemplateDto? BookingTerms { get; set; }

        /// <summary>
        /// Gets or sets the cancellation terms associated with the current booking.
        /// </summary>
        public CancellationTermsTemplateDto? CancellationTerms { get; set; }

        /// <summary>
        /// Gets or sets the terms and conditions template.
        /// </summary>
        public TermsAndConditionsTemplateDto? TermsAndConditions { get; set; }

        /// <summary>
        /// Gets or sets the payment exclusion template data transfer object.
        /// </summary>
        public PaymentExclusionTemplateDto? PaymentExclusion { get; set; }

        /// <summary>
        /// Gets or sets the vacation host details.
        /// </summary>
        public VacationHostDto? Host { get; set; } = null!;

        /// <summary>
        /// Gets or sets the primary consultant for the vacation.
        /// </summary>
        public ContactDto Consultant { get; set; } = null!;

        /// <summary>
        /// Gets or sets the primary coordinator for the vacation.
        /// </summary>
        public ContactDto Coordinator { get; set; } = null!;

        /// <summary>
        /// Gets or sets the primary tour director for the vacation.
        /// </summary>
        public ContactDto TourDirector { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date for the meeting or gathering associated with the vacation.
        /// </summary>
        public TimeSpan? MeetAndGreetTime { get; set; }

        /// <summary>
        /// Gets or sets the template used for meet-and-greet events.
        /// </summary>
        public MeetAndGreetTemplateDto MeetAndGreetTemplate { get; set; }

        #endregion

        #region Collections
        
        /// <summary>
        /// Gets or sets the list of breakfasts associated with the vacation extension.
        /// Each item in the list represents a meal addition for breakfast, including details such as date, time, and location.
        /// </summary>
        public List<MealAdditionDto>? Meals { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of vacation price groups.
        /// </summary>
        public List<VacationPriceGroupDto>? VacationPriceGroups { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of vacation pricing items.
        /// </summary>
        public List<VacationPricingItemDto>? VacationPrices { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of vacation intervals.
        /// </summary>
        public List<VacationIntervalDto>? VacationIntervals { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of flights associated with the vacation.
        /// </summary>
        public List<FlightDto>? Flights { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of golf courses associated with the vacation.
        /// </summary>
        public List<GolferPackageDto>? GolferPackages { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of day tours/activities associated with the vacation.
        /// </summary>
        public List<DayTourActivityDto>? DayTourActivities { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of day Vacation Inclusion Display Type Infos associated with the vacation.
        /// </summary>
        public List<VacationInclusionDisplayTypeInformationDto>? VacationInclusionDisplayTypeInfos { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of room gifts associated with the vacation.
        /// </summary>
        public List<RoomGiftDto>? Gifts { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of vacation inclusions.
        /// </summary>
        public List<VacationInclusionDto>? VacationInclusions { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of references (reviews) for the vacation.
        /// </summary>
        public List<VacationReviewDto>? References { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of itineraries for the vacation.
        /// </summary>
        public List<ItineraryDto>? Itineraries { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of payment rules for the vacation.
        /// </summary>
        public List<PaymentRuleDto>? PaymentRules { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of cancellation rules for the vacation.
        /// </summary>
        public List<VacationCancellationRuleDto>? CancellationRules { get; set; } = [];

        /// <summary>
        /// Gets the detailed itinerary for the vacation, organized by day.
        /// </summary>
        /// <remarks>The itinerary is dynamically generated based on the vacation's start date, end date,
        /// and associated details such as  meals, flights, accommodations, and activities. Each day includes relevant
        /// details such as check-ins, check-outs,  meals, and scheduled activities. If the start date is not specified,
        /// the itinerary will not include specific dates.</remarks>
        public List<VacationItineraryDay>? Itinerary
        {
            get
            {
                List<VacationItineraryDay> listItems = new List<VacationItineraryDay>();
                var dates = new List<DateTime>();
                if (StartDate is not null)
                {
                    dates = DateTimeExtensions.GetDatesBetween(StartDate.Value, EndDate.Value);
                }
                for (int i = 1; i <= DayCount; i++)
                {
                    var itemToAdd = new VacationItineraryDay() { Title = "Day " + (i), Date = StartDate is not null ? dates[i - 1] : null };

                    var checkForIncludedBreakfast = false;
                    if (i != 1)
                    {
                        var meal = Meals.FirstOrDefault(c => !c.IntervalInclusion && c.DayNr == i + 1 && c.MealAdditionTemPlate.MealType == MealType.Breakfast);
                        if (meal != null)
                        {
                            itemToAdd.Details.Add(new VacationItineraryDayItem(meal.StartTime, $"Special Breakfast at {meal.MealAdditionTemPlate.Restaurant.Name}"));
                        }
                        else
                        {
                            checkForIncludedBreakfast = true;
                        }
                    }


                    var dayNr = 1;
                    foreach (var interval in VacationIntervals.OrderBy(c => c.SortOrderNr))
                    {
                        if (checkForIncludedBreakfast && interval.BreakfastIncluded && i != 1 && i > dayNr && i <= (dayNr + interval.NightCount))
                        {
                            itemToAdd.Details.Add(new VacationItineraryDayItem(new TimeSpan(8, 0, 0), $"Breakfast at hotel"));
                        }

                        dayNr = dayNr + interval.NightCount;

                        if (dayNr == i)
                        {
                            itemToAdd.Details.Add(new VacationItineraryDayItem(interval.CheckOutTime.Value, $"Check-out at the {interval.Lodging.Name}"));
                            break;
                        }
                    }

                    if (Flights.Any(c => c.ArrivalDayNr == i))
                    {
                        foreach (var flight in Flights.Where(c => c.ArrivalDayNr == i))
                        {
                            if (string.IsNullOrEmpty(flight?.DepartureAirport?.Name))
                            {
                                itemToAdd.Details.Add(new VacationItineraryDayItem(flight?.ArrivalTime, $"Arrive at {flight?.ArrivalAirport?.Name} in {flight.ArrivalAirport.City.Name}"));
                            }
                            else if (flight.ArrivalTime is not null)
                            {
                                itemToAdd.Details.Add(new VacationItineraryDayItem(flight?.ArrivalTime, $"Flight to {flight?.ArrivalAirport?.Name} ({(flight.ArrivalTime - flight.DepartureTime).Value.Hours}h{(flight.ArrivalTime - flight.DepartureTime).Value.Minutes.ToString("D2")} flight) "));
                            }
                            else
                            {
                                itemToAdd.Details.Add(new VacationItineraryDayItem(new TimeSpan(10, 0, 0), $"Flight to {flight?.ArrivalAirport?.Name} in {flight.ArrivalAirport.City.Name}"));
                            }
                            if (flight.DepartureMovementTime is not null)
                            {
                                itemToAdd.Details.Add(new VacationItineraryDayItem(flight.DepartureTime.Value, $"Depart for {(string.IsNullOrEmpty(flight.DepartureMovementDestination) ? flight.DepartureAirport.Name : flight.DepartureMovementDestination)} by {(flight.DepartureMovementMode?.ToLowerInvariant().Trim() == "walk" ? "foot" : flight.DepartureMovementMode)} ({flight.DepartureMovementNotes})"));
                            }
                        }
                    }

                    if (GolferPackages.Any(c => c.DayNr == i))
                    {
                        foreach (var golferPackage in GolferPackages.Where(c => c.DayNr == i))
                        {
                            itemToAdd.Details.Add(new VacationItineraryDayItem(golferPackage.StartTime, $"Golfer: Round of golf at {golferPackage.GolfCourse.Name}"));
                        }
                    }

                    if (DayTourActivities.Any(c => c.DayNr == i))
                    {
                        foreach (var activity in DayTourActivities.Where(c => c.DayNr == i))
                        {
                            itemToAdd.Details.Add(new VacationItineraryDayItem(activity.StartTime, activity.DayTourActivityTemplate.Summary));
                        }
                    }

                    if (Meals.Any(c => !c.IntervalInclusion && c.DayNr == i && c.MealAdditionTemPlate.MealType == MealType.Lunch))
                    {
                        foreach (var lunch in Meals.Where(c => !c.IntervalInclusion && c.DayNr == i && c.MealAdditionTemPlate.MealType == MealType.Lunch))
                        {
                            if (lunch is { MealAdditionTemPlate.GuestType: (int)GuestType.All })
                            {
                                itemToAdd.Details.Add(new VacationItineraryDayItem(lunch.StartTime, $"Special Lunch at {lunch.MealAdditionTemPlate.Restaurant.Name}"));
                            }
                        }
                    }

                    var dayNr10 = 1;
                    foreach (var interval in VacationIntervals.OrderBy(c => c.SortOrderNr))
                    {
                        if (dayNr10 == i)
                        {
                            itemToAdd.Details.Add(new VacationItineraryDayItem(interval.CheckInTime.Value, $"Check-in at the {interval.Lodging.Name} for {(interval.StartDate(this.ToDto())?.AddDays(interval.NightCount) - interval.StartDate(this.ToDto()))?.Days} nights"));
                            break;
                        }
                        dayNr10 = dayNr10 + interval.NightCount;
                    }

                    if (Meals.Any(c => !c.IntervalInclusion && c.DayNr == i && c.MealAdditionTemPlate.MealType == MealType.Dinner))
                    {
                        foreach (var dinner in Meals.Where(c => !c.IntervalInclusion && c.DayNr == i && c.MealAdditionTemPlate.MealType == MealType.Dinner))
                        {
                            if (dinner.MealAdditionTemPlate.GuestType == (int)GuestType.All)
                                itemToAdd.Details.Add(new VacationItineraryDayItem(dinner.StartTime ?? new TimeSpan(19, 0, 0), $"Special group Dinner at {dinner.MealAdditionTemPlate.Restaurant.Name}"));
                        }
                    }
                    else
                    {
                        if (i != DayCount)
                        {
                            itemToAdd.Details.Add(new VacationItineraryDayItem(new TimeSpan(19, 0, 0), $"Dinner at leisure"));
                        }
                    }

                    var dayNr11 = 1;
                    foreach (var interval in VacationIntervals.OrderBy(c => c.SortOrderNr))
                    {
                        if (i >= dayNr11 && i < (dayNr11 + interval.NightCount))
                        {
                            itemToAdd.Details.Add(new VacationItineraryDayItem(new TimeSpan(23, 59, 59), $"Overnight at the {interval.Lodging.Name}"));
                        }
                        dayNr11 = dayNr11 + interval.NightCount;
                    }

                    listItems.Add(itemToAdd);
                }

                return listItems;
            }
        }

        /// <summary>
        /// Gets or sets the list of vacation extensions.
        /// </summary>
        public List<VacationDto> VacationExtensions { get; set; } = [];

        /// <summary>
        /// 
        /// </summary>
        public List<ItineraryEntryItemTemplateDto> ItineraryEntryItemTemplates { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current vacation entity to a <see cref="VacationDto"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the current vacation entity to a new instance of 
        /// <see cref="VacationDto"/>. The resulting DTO contains detailed information about the vacation,  including
        /// its metadata, inclusions, pricing, and associated entities.</remarks>
        /// <returns>A <see cref="VacationDto"/> object that represents the current vacation entity.</returns>
        public VacationDto ToDto()
        {
            return new VacationDto()
            {
                VacationId = VacationId,
                ReferenceNr = ReferenceNr,
                Slug = Slug,

                VacationTitle = VacationTitle,
                ShortDescription = ShortDescription,

                StartDate = StartDate,
                Nights = Nights,
                AvailabilityCutOffDate = AvailabilityCutOffDate,

                RoomCount = RoomCount,
                RoomCountFoc = RoomCountFOC,
                CurrencySymbol = CurrencySymbol,
                MaxBookingCount = MaxBookingCount,
                IsExtension = IsExtension,
                Published = Published,

                GeneralInformation = GeneralInformation,

                GeneralInclusionsSummaryInformation = GeneralInclusionsSummaryInformation,
                MealsAndActivitiesInclusionsSummaryInformation = MealsAndActivitiesInclusionsSummaryInformation,
                TransportAndFlightsInclusionsSummaryInformation = TransportAndFlightsInclusionsSummaryInformation,
                AccommodationInclusionSummaryInformation = AccommodationInclusionSummaryInformation,
                GolfRoundSummaryInclusionInformation = GolfRoundSummaryInclusionInformation,
                MeetAndGreet = MeetAndGreetTemplate,

                CancellationTerms = CancellationTerms,
                TermsAndConditions = TermsAndConditions,

                Host = Host,
                Consultant = Consultant,
                Coordinator = Coordinator,
                TourDirector = TourDirector,

                Images = Images,
                VacationPriceGroups = VacationPriceGroups,
                VacationPrices = VacationPrices,
                VacationIntervals = VacationIntervals,
                Flights = Flights,
                GolferPackages = GolferPackages,
                DayTourActivities = DayTourActivities,
                PaymentRules = PaymentRules,
                VacationInclusionDisplayTypeInfos = VacationInclusionDisplayTypeInfos,
                References = References,
                Itineraries = Itineraries,
                CancellationRules = CancellationRules,
                Gifts = Gifts,
                Meals = Meals,
                VacationExtensions = VacationExtensions,
                ItineraryEntryItemTemplates = ItineraryEntryItemTemplates
            };
        }

        #endregion
    }

    /// <summary>
    /// Represents a single day in the vacation itinerary.
    /// This class includes the title, date, and a list of detailed activities or events for the day.
    /// </summary>
    public class VacationItineraryDay
    {
        /// <summary>
        /// Gets or sets the title of the itinerary day.
        /// Typically, this is a descriptive label such as "Day 1" or "Arrival Day."
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the date of the itinerary day.
        /// This represents the specific calendar date for the activities or events.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the list of detailed activities or events for the day.
        /// Each item in the list represents a specific event, such as a flight, meal, or activity.
        /// </summary>
        public List<VacationItineraryDayItem> Details { get; set; } = [];
    }

    /// <summary>
    /// Represents a specific activity or event in a vacation itinerary day.
    /// This class includes the time and description of the activity or event.
    /// </summary>
    public class VacationItineraryDayItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VacationItineraryDayItem"/> class.
        /// </summary>
        /// <param name="time">The time of the activity or event.</param>
        /// <param name="description">A brief description of the activity or event.</param>
        public VacationItineraryDayItem(TimeSpan? time, string description)
        {
            Time = time;
            Description = description;
        }

        /// <summary>
        /// Gets or sets the time of the activity or event.
        /// This represents when the activity is scheduled to occur.
        /// </summary>
        public TimeSpan? Time { get; set; }

        /// <summary>
        /// Gets or sets the description of the activity or event.
        /// This provides details about what the activity entails.
        /// </summary>
        public string Description { get; set; }
    }
}