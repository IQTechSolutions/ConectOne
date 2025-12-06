using AccomodationModule.Domain.Entities;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FeedbackModule.Domain.Entities;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing repositories related to accommodation entities.
    /// Provides access to various repositories for CRUD operations on accommodation-related data.
    /// </summary>
    public interface IAccomodationRepositoryManager
    {
        #region Vacation Host

        /// <summary>
        /// Repository for managing vacation hosts.
        /// </summary>
        IRepository<VacationHost, string> VacationHosts { get; }

        /// <summary>
        /// Repository for managing images associated with vacation hosts.
        /// </summary>
        IRepository<EntityImage<VacationHost, string>, string> VacationHostImages { get; }

        #endregion

        #region Vacations

        /// <summary>
        /// Repository for managing vacations.
        /// </summary>
        IRepository<Vacation, string> Vacations { get; }

        /// <summary>
        /// Repository for managing images associated with vacations.
        /// </summary>
        IRepository<EntityImage<Vacation, string>, string> VacationImages { get; }

        /// <summary>
        /// Repository for managing vacation intervals.
        /// </summary>
        IRepository<VacationInterval, string> VacationIntervals { get; }

        /// <summary>
        /// Repository for managing vacation inclusions.
        /// </summary>
        IRepository<VacationExtensionAddition, string> VacationExtensionAdditions { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="ItineraryEntryItemTemplate"/> entities.
        /// </summary>
        IRepository<ItineraryEntryItemTemplate, string> ItineraryEntryItemTemplates { get; }

        /// <summary>
        /// Repository for managing vacation prices.
        /// </summary>
        IRepository<VacationPrice, string> VacationPrices { get; }

        /// <summary>
        /// Repository for managing vacation payment rules.
        /// </summary>
        IRepository<PaymentRule, string> VacationPaymentRules { get; }

        /// <summary>
        /// Gets the repository for managing vacation pricing groups.
        /// </summary>
        IRepository<VacationPriceGroup, string> VacationPricingGroups { get; }


        /// <summary>
        /// Gets the repository for managing vacation reviews.
        /// </summary>
        IRepository<VacationReview, string> VacationReviews { get; }

        /// <summary>
        /// Gets the repository for managing vacation reviews.
        /// </summary>
        IRepository<Review<Vacation>, string> Reviews { get; }

        #endregion

        /// <summary>
        /// Repository for managing vacation inclusions.
        /// </summary>
        IRepository<VacationCancellationRule, string> VacationCancellationRules { get; }

        /// <summary>
        /// Repository for managing vacation flights.
        /// </summary>
        IRepository<Flight, string> Flights { get; }

        /// <summary>
        /// Repository for managing meal additions.
        /// </summary>
        IRepository<MealAddition, string> MealAdditions { get; }

        /// <summary>
        /// Repository for managing vacation gifts.
        /// </summary>
        IRepository<RoomGift, string> Gifts { get; }

        /// <summary>
        /// Repository for managing golfer packages.
        /// </summary>
        IRepository<GolferPackage, string> GolferPackages { get; }

        /// <summary>
        /// Repository for managing Day Tour Activities.
        /// </summary>
        IRepository<DayTourActivity, string> DayTourActivities { get; }

        /// <summary>
        /// Repository for managing short description templates.
        /// </summary>
        IRepository<ShortDescriptionTemplate, string> ShortDescriptionTemplates { get; }

        /// <summary>
        /// Repository for managing itineraries.
        /// </summary>
        IRepository<Itinerary, string> Itineries { get; }

        /// <summary>
        /// Repository for managing itinerary items.
        /// </summary>
        IRepository<ItineraryItem, string> ItineraryItems { get; }

        /// <summary>
        /// Repository for managing vacation inclusions.
        /// </summary>
        IRepository<Inclusions, string> VacationInclusions { get; }

        /// <summary>
        /// Repository for managing vacation inclusion display type section information.
        /// </summary>
        IRepository<VacationInclusionDisplayTypeInformation, string> VacationInclusionDisplayTypeInfos { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="VacationContactUsInfo"/> entities.
        /// </summary>
        IRepository<VacationContactUsInfo, string> VacationContactUsInfos { get;  }

        #region Order and Booking Repositories

        /// <summary>
        /// Repository for managing orders.
        /// </summary>
        IRepository<Order, string> Orders { get; }

        /// <summary>
        /// Repository for managing bookings.
        /// </summary>
        IRepository<Booking, int> Bookings { get; }

        #endregion

        #region Lodging Repositories

        /// <summary>
        /// Repository for managing lodging listing requests.
        /// </summary>
        IRepository<LodgingListingRequest, string> LodgingListingRequests { get; }

        /// <summary>
        /// Repository for managing lodgings.
        /// </summary>
        IRepository<Lodging, string> Lodgings { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="LodgingType"/> entities.
        /// </summary>
        /// <remarks>Use this repository to perform operations such as querying, adding, updating, or
        /// deleting <see cref="LodgingType"/> entities.</remarks>
        IRepository<LodgingType, string> LodgingTypes { get; }

        /// <summary>
        /// Repository for managing packages.
        /// </summary>
        IRepository<LodgingPackage, int> Packages { get; }

        /// <summary>
        /// Repository for managing rooms.
        /// </summary>
        IRepository<Room, int> Rooms { get; }

        /// <summary>
        /// Repository for managing images associated with lodgings.
        /// </summary>
        IRepository<EntityImage<Lodging, string>, string> LodgingImages { get; }

        /// <summary>
        /// Repository for managing images associated with rooms.
        /// </summary>
        IRepository<EntityImage<Room, string>, string> RoomImages { get; }

        /// <summary>
        /// Repository for managing featured images.
        /// </summary>
        IRepository<FeaturedImage, string> FeaturedImages { get; }

        #endregion

        #region Policy Repositories

        /// <summary>
        /// Repository for managing cancellation rules.
        /// </summary>
        IRepository<CancellationRule, int> CancellationRules { get; }

        /// <summary>
        /// Repository for managing child policy rules.
        /// </summary>
        IRepository<ChildPolicyRule, string> ChildPolicyRules { get; }

        #endregion

        #region Meal and Bed Repositories

        /// <summary>
        /// Repository for managing meal plans.
        /// </summary>
        IRepository<MealPlan, string> MealPlans { get; }

        /// <summary>
        /// Repository for managing bed types.
        /// </summary>
        IRepository<BedType, string> BedTypes { get; }

        #endregion

        #region Airport Repositories

        /// <summary>
        /// Repository for managing airports.
        /// </summary>
        IRepository<Airport, string> Airports { get; }

        #endregion

        #region Amenity Repositories

        /// <summary>
        /// Repository for managing amenities associated with lodgings.
        /// </summary>
        IRepository<AmenityItem<Lodging, string>, string> LodgingAmenities { get; }

        /// <summary>
        /// Repository for managing amenities associated with rooms.
        /// </summary>
        IRepository<ServiceAmenity, int> RoomAmenities { get; }

        #endregion
    }
}
