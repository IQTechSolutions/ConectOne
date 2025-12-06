using AccomodationModule.Application.RestServices;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace AccomodationModule.Application
{
    /// <summary>
    /// Registers the accommodation module services and their dependencies into the specified <see
    /// cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>This method registers a wide range of services related to the accommodation module, including
    /// services for amenities, bookings, lodgings, packages, vacations, and more. It also configures an HTTP client
    /// with the specified <paramref name="baseAddress"/> and enables HTTP message interception.</remarks>
    public static class AccommodationModule
    {
        /// <summary>
        /// Registers services related to the Accommodation module into the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method configures HTTP clients and registers a wide range of scoped services for
        /// the Accommodation module. The registered services include amenities, areas, bookings, lodgings, vacations,
        /// and other related services. The <paramref name="baseAddress"/> parameter is used to set the base URI for
        /// HTTP client requests.</remarks>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <param name="baseAddress">The base address used to configure HTTP clients for the module's services.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddAccommodationModuleServices(this IServiceCollection serviceCollection, string baseAddress)
        {
            serviceCollection.AddScoped<IAmenityService, AmenityRestService>()
                .AddScoped<IAreaService, AreaRestService>()
                .AddScoped<IAverageTemperatureService, AverageTemperatureRestService>()
                .AddScoped<IBookingService, BookingRestService>()
                .AddScoped<IContactService, ContactRestService>()
                .AddScoped<ILodgingService, LodgingRestService>()
                .AddScoped<ILodgingCategoryService, LodgingCategoryRestService>()
                .AddScoped<ILodgingTypeService, LodgingTypeRestService>()
                .AddScoped<ILodgingPackageService, LodgingPackageRestService>()
                .AddScoped<IRoomDataService, RoomDataRestService>()
                .AddScoped<IVoucherService, VoucherRestService>()
                .AddScoped<IVacationService, VacationRestService>()
                .AddScoped<IVacationPricingService, VacationPricingRestService>()
                .AddScoped<IFlightService, FlightRestService>()
                .AddScoped<IRestaurantService, RestaurantRestService>()
                .AddScoped<IMealAdditionTemplateService, MealAdditionTemplateRestService>()
                .AddScoped<IDayTourActivityTemplateService, DayTourActivityTemplateRestService>()
                .AddScoped<IGeneralInformationTemplateService, GeneralInformationTemplateRestService>()
                .AddScoped<IMeetAndGreetTemplateService, MeetAndGreetTemplateRestService>()
                .AddScoped<IMealAdditionService, MealAdditionRestService>()
                .AddScoped<IVacationIntervalService, VacationIntervalRestService>()
                .AddScoped<IGolferPackageService, GolferPackageRestService>()
                .AddScoped<IVacationHostService, VacationHostRestService>()
                .AddScoped<IDestinationService, DestinationRestService>()
                .AddScoped<IGolfCoursesService, GolfCoursesRestService>()
                .AddScoped<IGiftService, GiftRestService>()
                .AddScoped<IVacationRoomGiftService, VacationRoomGiftRestService>()
                .AddScoped<IVacationReviewService, VacationReviewRestService>()
                .AddScoped<IShortDescriptionTemplateService, ShortDescriptionTemplateRestService>()
                .AddScoped<IAirportService, AirportRestService>()
                .AddScoped<IBookingTermsDescriptionTemplateService, BookingTermsDescriptionTemplateRestService>()
                .AddScoped<IPaymentExclusionTemplateService, PaymentExclusionTemplateRestService>()
                .AddScoped<ITermsAndConditionsTemplateService, TermsAndConditionsTemplateRestService>()
                .AddScoped<ICancellationTermsTemplateService, CancellationTermsTemplateRestService>()
                .AddScoped<IVacationTitleTemplateService, VacationTitleTemplateRestService>()
                .AddScoped<IDayTourActivityService, DayTourActivityRestService>()
                .AddScoped<IDayTourActivityTemplateService, DayTourActivityTemplateRestService>()
                .AddScoped<ICustomVariableTagService, CustomVariableRestService>()
                .AddScoped<IItineraryItemTemplateService, ItineraryItemTemplateRestService>()
                .AddScoped<IVacationContactUsInfoService, VacationContactUsInfoRestService>();
            
            return serviceCollection;
        }
    }
}
