// ReSharper disable MustUseReturnValue

using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FeedbackModule.Domain.Entities;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Infrastructure.Managers
{
    /// <summary>
    /// Implementation of the <see cref="IAccomodationRepositoryManager"/> interface.
    /// Manages repositories for various accommodation-related entities.
    /// </summary>
    public class AccomodationRepositoryManager(GenericDbContextFactory context) : IAccomodationRepositoryManager
    {
        #region Private Fields

        private readonly Lazy<IRepository<VacationHost, string>> _vacationHosts = new(() => new Repository<VacationHost, string>(context));
        private readonly Lazy<IRepository<EntityImage<VacationHost, string>, string>> _vacationHostImages = new(() => new Repository<EntityImage<VacationHost, string>, string>(context));
        
        private readonly Lazy<IRepository<Vacation, string>> _vacations = new(() => new Repository<Vacation, string>(context));
        private readonly Lazy<IRepository<EntityImage<Vacation, string>, string>> _vacationImages = new(() => new Repository<EntityImage<Vacation, string>, string>(context));
        private readonly Lazy<IRepository<VacationInterval, string>> _vacationIntervals = new(() => new Repository<VacationInterval, string>(context));
        private readonly Lazy<IRepository<ItineraryEntryItemTemplate, string>> _itineraryEntryItemTemplate = new(() => new Repository<ItineraryEntryItemTemplate, string>(context));
        private readonly Lazy<IRepository<VacationExtensionAddition, string>> _vacationExtensionAdditions = new(() => new Repository<VacationExtensionAddition, string>(context));
        private readonly Lazy<IRepository<VacationPrice, string>> _vacationPrices = new(() => new Repository<VacationPrice, string>(context));
        private readonly Lazy<IRepository<VacationPriceGroup, string>> _vacationPriceGroups = new(() => new Repository<VacationPriceGroup, string>(context));
        private readonly Lazy<IRepository<PaymentRule, string>> _vacationPaymentRules = new(() => new Repository<PaymentRule, string>(context));
        private readonly Lazy<IRepository<VacationReview, string>> _vacationReviews = new(() => new Repository<VacationReview, string>(context));
        private readonly Lazy<IRepository<Review<Vacation>, string>> _reviews = new(() => new Repository<Review<Vacation>, string>(context));
        private readonly Lazy<IRepository<VacationContactUsInfo, string>> _vacationContactUsInfos = new(() => new Repository<VacationContactUsInfo, string>(context));

        private readonly Lazy<IRepository<VacationCancellationRule, string>> _vacationCancellationRules = new(() => new Repository<VacationCancellationRule, string>(context));
        private readonly Lazy<IRepository<Flight, string>> _flights = new(() => new Repository<Flight, string>(context));
        private readonly Lazy<IRepository<MealAddition, string>> _mealAdditions = new(() => new Repository<MealAddition, string>(context));
        private readonly Lazy<IRepository<RoomGift, string>> _roomGifts = new(() => new Repository<RoomGift, string>(context));
        private readonly Lazy<IRepository<GolferPackage, string>> _golferPackage = new(() => new Repository<GolferPackage, string>(context));
        private readonly Lazy<IRepository<DayTourActivity, string>> _dayTourActivities = new(() => new Repository<DayTourActivity, string>(context));
        private readonly Lazy<IRepository<ShortDescriptionTemplate, string>> _shortDescriptionTemplates = new(() => new Repository<ShortDescriptionTemplate, string>(context));
        private readonly Lazy<IRepository<Itinerary, string>> _itineries = new(() => new Repository<Itinerary, string>(context));
        private readonly Lazy<IRepository<ItineraryItem, string>> _itineraryItems = new(() => new Repository<ItineraryItem, string>(context));
        private readonly Lazy<IRepository<Inclusions, string>> _vacationInclusions = new(() => new Repository<Inclusions, string>(context));
        private readonly Lazy<IRepository<VacationInclusionDisplayTypeInformation, string>> _vacationInclusionDisplayTypeInfos = new Lazy<IRepository<VacationInclusionDisplayTypeInformation, string>>(() => new Repository<VacationInclusionDisplayTypeInformation, string>(context));
        
        private readonly Lazy<IRepository<Order, string>> _orders = new(() => new Repository<Order, string>(context));
        private readonly Lazy<IRepository<Booking, int>> _bookings = new(() => new Repository<Booking, int>(context));
        
        private readonly Lazy<IRepository<LodgingListingRequest, string>> _lodgingListingRequests = new(() => new Repository<LodgingListingRequest, string>(context));
        private readonly Lazy<IRepository<Lodging, string>> _lodgings = new(() => new Repository<Lodging, string>(context));
        private readonly Lazy<IRepository<LodgingType, string>> _lodgingTypes = new(() => new Repository<LodgingType, string>(context));
        private readonly Lazy<IRepository<LodgingPackage, int>> _packages = new(() => new Repository<LodgingPackage, int>(context));
        private readonly Lazy<IRepository<Room, int>> _rooms = new(() => new Repository<Room, int>(context));
        private readonly Lazy<IRepository<EntityImage<Lodging, string>, string>> _lodgingImages = new(() => new Repository<EntityImage<Lodging, string>, string>(context));
        private readonly Lazy<IRepository<EntityImage<Room, string>, string>> _roomImages = new(() => new Repository<EntityImage<Room, string>, string>(context));
        
        private readonly Lazy<IRepository<CancellationRule, int>> _cancellationRules = new(() => new Repository<CancellationRule, int>(context));
        private readonly Lazy<IRepository<ChildPolicyRule, string>> _childPolicyRules = new(() => new Repository<ChildPolicyRule, string>(context));
        
        private readonly Lazy<IRepository<AmenityItem<Lodging, string>, string>> _lodgingAmenities = new(() => new Repository<AmenityItem<Lodging, string>, string>(context));
        private readonly Lazy<IRepository<ServiceAmenity, int>> _roomAmenities = new(() => new Repository<ServiceAmenity, int>(context));
        private readonly Lazy<IRepository<FeaturedImage, string>> _featuredImages = new(() => new Repository<FeaturedImage, string>(context));
        private readonly Lazy<IRepository<MealPlan, string>> _mealPlans = new(() => new Repository<MealPlan, string>(context));
        private readonly Lazy<IRepository<BedType, string>> _bedTypes = new(() => new Repository<BedType, string>(context));
        private readonly Lazy<IRepository<Airport, string>> _airports = new(() => new Repository<Airport, string>(context));

        #endregion

        #region Vacation Host

        /// <summary>
        /// Gets the repository for managing vacation hosts.
        /// </summary>
        public IRepository<VacationHost, string> VacationHosts => _vacationHosts.Value;

        /// <summary>
        /// Gets the repository for managing images associated with vacation hosts.
        /// </summary>
        public IRepository<EntityImage<VacationHost, string>, string> VacationHostImages => _vacationHostImages.Value;

        #endregion

        #region Vacation

        /// <summary>
        /// Gets the repository for managing vacations.
        /// </summary>
        public IRepository<Vacation, string> Vacations => _vacations.Value;

        /// <summary>
        /// Gets the repository for managing images associated with vacations.
        /// </summary>
        public IRepository<EntityImage<Vacation, string>, string> VacationImages => _vacationImages.Value;

        /// <summary>
        /// Gets the repository for managing vacation intervals.
        /// </summary>
        public IRepository<VacationInterval, string> VacationIntervals => _vacationIntervals.Value;

        /// <summary>
        /// Gets the repository for managing vacation extension additions.
        /// </summary>
        public IRepository<VacationExtensionAddition, string> VacationExtensionAdditions => _vacationExtensionAdditions.Value;

        /// <summary>
        /// Gets the repository for managing <see cref="ItineraryEntryItemTemplate"/> entities.
        /// </summary>
        /// <remarks>Use this property to perform CRUD operations or query <see
        /// cref="ItineraryEntryItemTemplate"/> entities. The repository is lazily initialized and ensures efficient
        /// access to the underlying data source.</remarks>
        public IRepository<ItineraryEntryItemTemplate, string> ItineraryEntryItemTemplates => _itineraryEntryItemTemplate.Value;

        /// <summary>
        /// Gets the repository for managing vacation prices.
        /// </summary>
        public IRepository<VacationPrice, string> VacationPrices => _vacationPrices.Value;

        /// <summary>
        /// Gets the repository for managing vacation pricing groups.
        /// </summary>
        /// <remarks>This repository allows querying, adding, updating, and deleting vacation pricing
        /// groups. The key for each vacation pricing group is a <see langword="string"/>.</remarks>
        public IRepository<VacationPriceGroup, string> VacationPricingGroups => _vacationPriceGroups.Value;

        /// <summary>
        /// Repository for managing vacation payment rules.
        /// </summary>
        public IRepository<PaymentRule, string> VacationPaymentRules => _vacationPaymentRules.Value;

        /// <summary>
        /// Gets the repository for managing vacation reviews.
        /// </summary>
        public IRepository<VacationReview, string> VacationReviews => _vacationReviews.Value;

        /// <summary>
        /// Gets the repository for managing vacation reviews.
        /// </summary>
        public IRepository<Review<Vacation>, string> Reviews => _reviews.Value;

        #endregion

        /// <summary>
        /// Gets the repository for managing vacation cancellation rules.
        /// </summary>
        public IRepository<VacationCancellationRule, string> VacationCancellationRules => _vacationCancellationRules.Value;

        /// <summary>
        /// Gets the repository for managing flights.
        /// </summary>
        public IRepository<Flight, string> Flights => _flights.Value;

        /// <summary>
        /// Gets the repository for managing meal additions.
        /// </summary>
        public IRepository<MealAddition, string> MealAdditions => _mealAdditions.Value;

        /// <summary>
        /// Gets the repository for managing room gifts.
        /// </summary>
        public IRepository<RoomGift, string> Gifts => _roomGifts.Value;

        /// <summary>
        /// Gets the repository for golfer packages.
        /// </summary>
        public IRepository<GolferPackage, string> GolferPackages => _golferPackage.Value;

        /// <summary>
        /// Gets the repository for day tour activities.
        /// </summary>
        public IRepository<DayTourActivity, string> DayTourActivities => _dayTourActivities.Value;

        /// <summary>
        /// Gets the repository for managing short description templates.
        /// </summary>
        public IRepository<ShortDescriptionTemplate, string> ShortDescriptionTemplates => _shortDescriptionTemplates.Value;

        /// <summary>
        /// Gets the repository for managing itineraries.
        /// </summary>
        public IRepository<Itinerary, string> Itineries => _itineries.Value;

        /// <summary>
        /// Gets the repository for managing itinerary items.
        /// </summary>
        public IRepository<ItineraryItem, string> ItineraryItems => _itineraryItems.Value;

        /// <summary>
        /// Gets the repository for managing itinerary items.
        /// </summary>
        public IRepository<VacationInclusionDisplayTypeInformation, string> VacationInclusionDisplayTypeInfos => _vacationInclusionDisplayTypeInfos.Value;

        /// <summary>
        /// Gets the repository for managing <see cref="VacationContactUsInfo"/> entities.
        /// </summary>
        public IRepository<VacationContactUsInfo, string> VacationContactUsInfos => _vacationContactUsInfos.Value;

        /// <summary>
        /// Gets the repository for managing vacation inclusions.
        /// </summary>
        public IRepository<Inclusions, string> VacationInclusions => _vacationInclusions.Value;
        
        #region Order and Booking Repositories

        /// <summary>
        /// Gets the repository for managing orders.
        /// </summary>
        public IRepository<Order, string> Orders => _orders.Value;

        /// <summary>
        /// Gets the repository for managing bookings.
        /// </summary>
        public IRepository<Booking, int> Bookings => _bookings.Value;

        #endregion

        #region Lodging Repositories

        /// <summary>
        /// Gets the repository for managing lodging listing requests.
        /// </summary>
        public IRepository<LodgingListingRequest, string> LodgingListingRequests => _lodgingListingRequests.Value;

        /// <summary>
        /// Gets the repository for managing lodgings.
        /// </summary>
        public IRepository<Lodging, string> Lodgings => _lodgings.Value;

        /// <summary>
        /// Gets the repository for managing <see cref="LodgingType"/> entities.
        /// </summary>
        public IRepository<LodgingType, string> LodgingTypes => _lodgingTypes.Value;

        /// <summary>
        /// Gets the repository for managing packages.
        /// </summary>
        public IRepository<LodgingPackage, int> Packages => _packages.Value;

        /// <summary>
        /// Gets the repository for managing rooms.
        /// </summary>
        public IRepository<Room, int> Rooms => _rooms.Value;

        /// <summary>
        /// Gets the repository for managing images associated with lodgings.
        /// </summary>
        public IRepository<EntityImage<Lodging, string>, string> LodgingImages => _lodgingImages.Value;

        /// <summary>
        /// Gets the repository for managing images associated with rooms.
        /// </summary>
        public IRepository<EntityImage<Room, string>, string> RoomImages => _roomImages.Value;

        #endregion

        #region Policy Repositories

        /// <summary>
        /// Gets the repository for managing cancellation rules.
        /// </summary>
        public IRepository<CancellationRule, int> CancellationRules => _cancellationRules.Value;

        /// <summary>
        /// Gets the repository for managing child policy rules.
        /// </summary>
        public IRepository<ChildPolicyRule, string> ChildPolicyRules => _childPolicyRules.Value;

        #endregion

        #region Meal and Bed Repositories

        /// <summary>
        /// Gets the repository for managing meal plans.
        /// </summary>
        public IRepository<MealPlan, string> MealPlans => _mealPlans.Value;

        /// <summary>
        /// Gets the repository for managing bed types.
        /// </summary>
        public IRepository<BedType, string> BedTypes => _bedTypes.Value;

        #endregion

        #region Airport Repositories

        /// <summary>
        /// Gets the repository for managing airports.
        /// </summary>
        public IRepository<Airport, string> Airports => _airports.Value;

        #endregion

        #region Amenity Repositories

        /// <summary>
        /// Gets the repository for managing amenities associated with lodgings.
        /// </summary>
        public IRepository<AmenityItem<Lodging, string>, string> LodgingAmenities => _lodgingAmenities.Value;

        /// <summary>
        /// Gets the repository for managing amenities associated with rooms.
        /// </summary>
        public IRepository<ServiceAmenity, int> RoomAmenities => _roomAmenities.Value;

        #endregion

        #region Featured Image Repositories

        /// <summary>
        /// Gets the repository for managing featured images.
        /// </summary>
        public IRepository<FeaturedImage, string> FeaturedImages => _featuredImages.Value;

        #endregion
    }
}
