using System.ComponentModel;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a meal plan, providing details such as rate, meal plan identifiers,  room
    /// association, and pricing information.
    /// </summary>
    /// <remarks>This class is typically used to transfer meal plan data between the application layers,  such
    /// as from a data source to the UI. It includes properties for identifying the meal plan,  its associated rate, and
    /// other relevant details.</remarks>
    public class MealplanViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MealplanViewModel"/> class.
        /// </summary>
        public MealplanViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MealplanViewModel"/> class using the specified meal plan data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="MealPlanDto"/> object
        /// to the corresponding properties of the <see cref="MealplanViewModel"/> instance. If <paramref
        /// name="mealplan"/> is <see langword="null"/>, no properties are initialized.</remarks>
        /// <param name="mealplan">The <see cref="MealPlanDto"/> object containing the meal plan data to initialize the view model. Must not be
        /// <see langword="null"/>.</param>
        public MealplanViewModel(MealPlanDto mealplan)
        {
            if (mealplan is not null)
            {
                RateId = mealplan.RateId;
                MealPlanId = mealplan.MealPlanId;
                PartnerMealPlanId = mealplan.PartnerMealPlanId;
                RoomId = mealplan.RoomId;
                MealPlan = mealplan.Description;
                Default = mealplan.Default;
                Rate = mealplan.Rate;
                OriginalRate = mealplan.OriginalRate;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the rate.
        /// </summary>
        [DisplayName("Rate Id")] public int RateId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the meal plan.
        /// </summary>
        [DisplayName("Meal Plan Id")] public string? MealPlanId { get; set; }

        /// <summary>
        /// Gets or sets the meal plan type associated with a partner.
        /// </summary>
        [DisplayName("Partner Meal Plan Id")] public MealPlanTypes PartnerMealPlanId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a room.
        /// </summary>
        [DisplayName("Room Id")] public int RoomId { get; set; }

        /// <summary>
        /// Gets or sets the meal plan associated with the entity.
        /// </summary>
        [DisplayName("Meal Plan")] public string MealPlan { get; set; } = null!;

        /// <summary>
        /// Gets a value indicating whether the default configuration is enabled.
        /// </summary>
        public bool Default { get; init; }

        /// <summary>
        /// Gets the rate value associated with the current instance.
        /// </summary>
        public double Rate { get; init; }

        /// <summary>
        /// Gets the original rate associated with the entity.
        /// </summary>
        [DisplayName("Original Rate")]  public double OriginalRate { get; init; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance to a <see cref="MealPlanDto"/>.
        /// </summary>
        /// <returns>A <see cref="MealPlanDto"/> object containing the data from the current instance.</returns>
        public MealPlanDto ToDto() => new()
        {
            RateId = RateId,
            MealPlanId = MealPlanId,
            PartnerMealPlanId = PartnerMealPlanId,
            RoomId = RoomId,
            Description = MealPlan,
            Default = Default,
            Rate = Rate,
            OriginalRate = OriginalRate
        };

        #endregion
    }
}
