using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the total number of guests, including adults and children, for a room.
    /// </summary>
    public class GuestTotals
    {
        #region Properties

        /// <summary>
        /// Gets or sets the room details.
        /// </summary>
        public RoomDto? Room { get; set; }

        /// <summary>
        /// Gets or sets the room option number.
        /// </summary>
        public int RoomOptionNr { get; set; }

        /// <summary>
        /// Gets or sets the number of adults.
        /// </summary>
        public int Adults { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of children.
        /// </summary>
        public int Kids { get; set; }

        /// <summary>
        /// Gets the total number of guests (adults + children).
        /// </summary>
        public int TotalGuests => Adults + Kids;

        /// <summary>
        /// Gets or sets the list of child ages.
        /// </summary>
        public List<int> ChildAges { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets a value indicating whether the room option is selected.
        /// </summary>
        public bool SelectedValue { get; set; }

        /// <summary>
        /// Gets or sets the rate scheme for the room.
        /// </summary>
        public RateScheme RateSheme { get; set; }

        /// <summary>
        /// Gets or sets the bed type for the room.
        /// </summary>
        public BedTypeDto BedType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the meal plan ID for the room.
        /// </summary>
        public string MealPlan { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Validates if the total number of guests does not exceed the maximum occupancy of the room.
        /// </summary>
        /// <returns>True if the total number of guests is valid; otherwise, false.</returns>
        public bool MaxOccupancyValid()
        {
            return Room is not null && Adults + Kids <= Room.MaxOccupancy;
        }

        /// <summary>
        /// Validates if the number of adults does not exceed the maximum allowed in the room.
        /// </summary>
        /// <returns>True if the number of adults is valid; otherwise, false.</returns>
        public bool AdultCountValid()
        {
            return Room is not null && Adults <= Room.MaxAdults;
        }

        /// <summary>
        /// Validates if the number of children does not exceed the remaining occupancy after adults.
        /// </summary>
        /// <returns>True if the number of children is valid; otherwise, false.</returns>
        public bool ChildCountValid()
        {
            return Room is not null && Kids <= Room.MaxOccupancy - Room.MaxAdults;
        }

        /// <summary>
        /// Gets the count of children within a specified age range.
        /// </summary>
        /// <param name="lowestCutOff">The lowest age in the range.</param>
        /// <param name="highestCutOff">The highest age in the range.</param>
        /// <returns>The count of children within the specified age range.</returns>
        public int ChildGroupCount(int lowestCutOff, int highestCutOff)
        {
            if (ChildAges != null)
                return ChildAges.Where(c => c >= lowestCutOff && c <= highestCutOff).Count();
            return 0;
        }

        #endregion
    }
}

