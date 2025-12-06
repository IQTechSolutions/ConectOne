using ProductsModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the pricing details for a specific room option in availability data.
    /// </summary>
    /// <remarks>This class encapsulates the pricing information associated with a room option,  including the
    /// identifier for the room option and its corresponding price details.</remarks>
    public class RoomAvailablitityItemPricingItem
    {
        /// <summary>
        /// Gets or sets the number of available room options.
        /// </summary>
        public int RoomOptionsNr { get; set; }        

        /// <summary>
        /// Gets or sets the pricing details for the associated item.
        /// </summary>
        public PricingDto Price { get; set; } = null!;
        
    }
}
