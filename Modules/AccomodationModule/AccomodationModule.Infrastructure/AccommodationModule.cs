using AccomodationModule.Domain.Interfaces;
using AccomodationModule.Infrastructure.Implimentation;
using AccomodationModule.Infrastructure.Managers;
using FilingModule.Infrastucture;
using Microsoft.Extensions.DependencyInjection;

namespace AccomodationModule.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering lodging-related services in an application's dependency injection
    /// container.
    /// </summary>
    /// <remarks>This static class contains methods to simplify the registration of services related to
    /// lodging, amenities, bookings,  and other vacation-related functionalities. It is intended to be used during
    /// application startup to configure  the dependency injection container.</remarks>
    public static class AccommodationModule
    {
        /// <summary>
        /// Registers lodging-related services into the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method adds a variety of services related to lodging, accommodations, and
        /// vacation planning. Each service is registered with a scoped lifetime, ensuring a new instance is created for
        /// each request within the scope. Call this method during application startup to configure dependency injection
        /// for lodging-related functionality.</remarks>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to which the lodging services will be added.</param>
        /// <returns>The <see cref="IServiceCollection"/> instance with the lodging services registered.</returns>
        public static IServiceCollection AddAccommodationModuleServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddFilingModule();
            serviceCollection.AddScoped<IAccomodationRepositoryManager, AccomodationRepositoryManager>();

            serviceCollection.AddScoped<IAmenityService, AmenityService>()
                .AddScoped<IAreaService, AreaService>()
                .AddScoped<IAverageTemperatureService, AverageTemperatureService>()
                .AddScoped<IContactService, ContactService>()
                .AddScoped<IBookingService, BookingService>()
                .AddScoped<ILodgingService, LodgingService>()
                .AddScoped<ILodgingCategoryService, LodgingCategoryService>()
                .AddScoped<ILodgingTypeService, LodgingTypeService>()
                .AddScoped<ILodgingPackageService, LodgingPackageService>()
                .AddScoped<IRoomDataService, RoomDataService>()
                .AddScoped<IVoucherService, VoucherService>()
                .AddScoped<IVacationService, VacationService>()
                .AddScoped<IVacationPricingService, VacationPricingService>()
                .AddScoped<IFlightService, FlightService>()
                .AddScoped<IRestaurantService, RestaurantService>()
                .AddScoped<IMealAdditionTemplateService, MealAdditionTemplateService>()
                .AddScoped<IDayTourActivityTemplateService, DayTourActivityTemplateService>()
                .AddScoped<IGeneralInformationTemplateService, GeneralInformationTemplateService>()
                .AddScoped<IMeetAndGreetTemplateService, MeetAndGreetTemplateService>()
                .AddScoped<IMealAdditionService, MealAdditionService>()
                .AddScoped<IVacationIntervalService, VacationIntervalService>()
                .AddScoped<IGolferPackageService, GolferPackageService>()
                .AddScoped<IVacationHostService, VacationHostService>()
                .AddScoped<IDestinationService, DestinationService>()
                .AddScoped<IGolfCoursesService, GolfCoursesService>()
                .AddScoped<IGiftService, GiftService>()
                .AddScoped<IVacationRoomGiftService, VacationRoomGiftService>()
                .AddScoped<IVacationReviewService, VacationReviewService>()
                .AddScoped<IShortDescriptionTemplateService, ShortDescriptionTemplateService>()
                .AddScoped<IAirportService, AirportService>()
                .AddScoped<IBookingTermsDescriptionTemplateService, BookingTermsDescriptionTemplateService>()
                .AddScoped<IPaymentExclusionTemplateService, PaymentExclusionTemplateService>()
                .AddScoped<ITermsAndConditionsTemplateService, TermsAndConditionsTemplateService>()
                .AddScoped<ICancellationTermsTemplateService, CancellationTermsTemplateService>()
                .AddScoped<IVacationTitleTemplateService, VacationTitleTemplateService>()
                .AddScoped<IDayTourActivityService, DayTourActivityService>()
                .AddScoped<IDayTourActivityTemplateService, DayTourActivityTemplateService>()
                .AddScoped<ICustomVariableTagService, CustomVariableTagService>()
                .AddScoped<IItineraryItemTemplateService, ItineraryItemTemplateService>()
                .AddScoped<IVacationContactUsInfoService, VacationContactUsInfoService>();
            
            return serviceCollection;
        }
    }
}
