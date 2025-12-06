using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the availability information for a specific room on a given date, including pricing details, child
    /// policy rules, and meal plan options.
    /// </summary>
    /// <remarks>This view model is designed to encapsulate the availability data for a room, including its
    /// associated date, availability details, child policy rules,  and pricing information. It also provides properties
    /// to indicate whether the room is unavailable and to manage selection state.</remarks>
    /// <param name="date"></param>
    /// <param name="availability"></param>
    /// <param name="childPolicyRules"></param>
    public class RoomAvailabilityItemViewModel(DateTime date, AvailabilityDto availability, List<ChildPolicyRuleViewModel> childPolicyRules)
    {
        /// <summary>
        /// Gets or sets the date associated with the current instance.
        /// </summary>
        public DateTime Date { get; set; } = date;

        /// <summary>
        /// Gets or sets the availability details.
        /// </summary>
        public AvailabilityDto Availability { get; set; } = availability;

        /// <summary>
        /// Gets or sets the collection of child policy rules.
        /// </summary>
        public List<ChildPolicyRuleViewModel> ChildPolicyRules { get; set; } = childPolicyRules;

        /// <summary>
        /// Gets or sets a value indicating whether the current item is selected.
        /// </summary>
        public bool SelectedValue { get; set; } = false;

        /// <summary>
        /// Gets or sets the type of meal plan associated with the reservation.
        /// </summary>
        /// <remarks>Use this property to specify or retrieve the meal plan for a reservation. Common
        /// values include options such as "Full Board," "Half Board," or "Room Only," depending on the <see
        /// cref="MealPlanTypes"/> definition.</remarks>
        public MealPlanTypes MealPlan { get; set; }

        /// <summary>
        /// Gets or sets the collection of pricing items associated with room availability.
        /// </summary>
        public List<RoomAvailablitityItemPricingItem> PricingItems { get; set; } = [];

        /// <summary>
        /// Gets a value indicating whether no rooms are available.
        /// </summary>
        public bool UnAvailable => Availability.NrRooms == 0;    
    }
}
