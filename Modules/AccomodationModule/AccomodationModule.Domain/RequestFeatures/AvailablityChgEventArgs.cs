
namespace AccomodationModule.Domain.Arguments
{
    /// <summary>
    /// Provides data for the availability change event, including the room type identifier and the updated number of
    /// available rooms.
    /// </summary>
    /// <remarks>This class is used to convey information about changes in room availability for a specific
    /// room type. It contains the room type identifier and the updated count of available rooms.</remarks>
    /// <param name="roomTypeId"></param>
    /// <param name="value"></param>
    public class AvailablityChgEventArgs(int roomTypeId, int value) : EventArgs
    {
        /// <summary>
        /// Gets or sets the unique identifier for the room type.
        /// </summary>
        public int RoomTypeId { get; set; } = roomTypeId;

        /// <summary>
        /// Gets or sets the number of rooms currently available.
        /// </summary>
        public int AvailableRooms { get; set; } = value;
    }
}