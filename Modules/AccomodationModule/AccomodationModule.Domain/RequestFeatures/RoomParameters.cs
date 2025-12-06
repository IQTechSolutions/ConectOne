namespace AccomodationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters for configuring a room, including guest counts and child age details.
    /// </summary>
    /// <remarks>This class is used to define the configuration of a room, including the number of adults,
    /// children,  and their respective ages. It also provides a calculated property to determine the total number of
    /// guests.</remarks>
    public class RoomParameters
    {
        /// <summary>
        /// Gets or sets the number of available room options.
        /// </summary>
        public int RoomOptionsNr { get; set; }

        /// <summary>
        /// Gets or sets the number of adults associated with the current context.
        /// </summary>
        public int AdultCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of child elements.
        /// </summary>
        public int ChildCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the list of ages for children.
        /// </summary>
        public List<int> ChildAges { get; set; } = new List<int>();

        /// <summary>
        /// Gets the total number of guests, including adults and children.
        /// </summary>
        public int TotalGuestCount => AdultCount + ChildCount;
    }
}
