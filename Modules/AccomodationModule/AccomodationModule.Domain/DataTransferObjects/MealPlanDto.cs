using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for meal plan information.
    /// </summary>
    /// <remarks>This record is used to encapsulate meal plan details for transfer between different layers of
    /// the application. It provides properties for identifying the meal plan, its associated rate, room, and
    /// description, as well as flags and values indicating default status and pricing.</remarks>
    public record MealPlanDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MealPlanDto"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see cref="MealPlanDto"/> class
        /// with no properties initialized. Use this constructor when you need to create an empty meal plan object and
        /// populate its properties later.</remarks>
        public MealPlanDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MealPlanDto"/> class using the specified <see cref="MealPlan"/>
        /// object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="MealPlan"/> object to
        /// the corresponding properties of the <see cref="MealPlanDto"/> instance. It is typically used to convert a
        /// domain model into a data transfer object.</remarks>
        /// <param name="mealPlan">The <see cref="MealPlan"/> object containing the data to initialize the DTO. Cannot be null.</param>
        public MealPlanDto(MealPlan mealPlan)
        {
            RateId = mealPlan.RateId;
            MealPlanId = mealPlan.Id;
            RoomId = mealPlan.RoomId;
            PartnerMealPlanId = mealPlan.PartnerMealPlanId;
            Description = mealPlan.Description;
            Default = mealPlan.Default;
            Rate = mealPlan.Rate;
            OriginalRate = mealPlan.OriginalRate;
            RoomId = mealPlan.RoomId;
        }

        #endregion

        /// <summary>
        /// Gets the unique identifier for the rate.
        /// </summary>
        public int RateId { get; init; } 

        /// <summary>
        /// Gets the unique identifier for the meal plan.
        /// </summary>
        public string? MealPlanId { get; init; }

        /// <summary>
        /// Gets the unique identifier for the room.
        /// </summary>
        public int RoomId { get; init; }

        /// <summary>
        /// Gets the identifier of the partner's meal plan.
        /// </summary>
        public MealPlanTypes PartnerMealPlanId { get; init; }

        /// <summary>
        /// Gets the description of the item.
        /// </summary>
        public string Description { get; init; } = null!;

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
        public double OriginalRate { get; init; }

        /// <summary>
        /// Converts the current object to a <see cref="MealPlan"/> instance.
        /// </summary>
        /// <remarks>This method creates a new <see cref="MealPlan"/> object and populates its properties
        /// based on the values of the current object's properties. If the <c>MealPlanId</c> property is null or empty,
        /// a new unique identifier is generated for the <c>Id</c> property.</remarks>
        /// <returns>A <see cref="MealPlan"/> instance with properties initialized from the current object.</returns>
        public MealPlan ToMealPlan()
        {
            return new MealPlan()
            {
                RateId = this.RateId,
                Id = string.IsNullOrEmpty(MealPlanId) ? Guid.NewGuid().ToString() : MealPlanId,
                PartnerMealPlanId = this.PartnerMealPlanId,
                RoomId = this.RoomId,
                Description = this.Description,
                Default = this.Default,
                Rate = this.Rate,
                OriginalRate = this.OriginalRate
            };
        }
    }
}
