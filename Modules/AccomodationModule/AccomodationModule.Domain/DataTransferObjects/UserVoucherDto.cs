using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a Data Transfer Object (DTO) that associates a specific voucher with a particular user.
/// A User Voucher is essentially a voucher that has been assigned or purchased by a specific user,
/// potentially allowing them to redeem certain benefits, discounts, or accommodation services.
/// </summary>
public record UserVoucherDto
{
    /// <summary>
    /// Parameterless constructor required for serialization scenarios and for frameworks that require 
    /// an empty constructor. Useful if we need to create an instance and populate its properties later.
    /// </summary>
    public UserVoucherDto() { }

    /// <summary>
    /// Constructs a <see cref="UserVoucherDto"/> from a <see cref="UserVoucher"/> domain entity.
    /// This constructor maps all relevant properties from the database model/UserVoucher entity to a 
    /// transfer object suitable for sending to the client or other layers.
    /// </summary>
    /// <param name="userVoucher">The UserVoucher entity to convert.</param>
    public UserVoucherDto(UserVoucher userVoucher)
    {
        // Unique identifier for the user voucher
        UserVoucherId = userVoucher.Id;

        // The ID of the user who owns or has been assigned this voucher
        UserId = userVoucher.UserId;

        // Convert the associated Voucher entity into a VoucherDto for easier consumption
        Voucher = new VoucherDto(userVoucher.Voucher);

        // Retrieve the room from the voucher that matches the user's specified RoomId
        // This ensures the UserVoucherDto points to the exact room related to this voucher
        Room = new RoomDto(userVoucher.Voucher.Rooms.FirstOrDefault(r => r.Id == userVoucher.RoomId));
    }

    /// <summary>
    /// Constructs a <see cref="UserVoucherDto"/> from given user and voucher information, 
    /// along with a specific room the voucher applies to.
    /// This is useful when creating a new user voucher without a pre-existing UserVoucher entity.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who owns or is assigned the voucher.</param>
    /// <param name="voucher">A <see cref="VoucherDto"/> representing the voucher details.</param>
    /// <param name="room">A <see cref="RoomDto"/> representing the specific room the voucher applies to.</param>
    public UserVoucherDto(string userId, VoucherDto voucher, RoomDto room)
    {
        // Create a new unique identifier for this UserVoucher, typically a Guid represented as a string
        UserVoucherId = Guid.NewGuid().ToString();

        // Assign the provided user ID
        UserId = userId;

        // Assign the provided voucher and room DTOs
        Voucher = voucher;
        Room = room;
    }

    /// <summary>
    /// Unique identifier for the User Voucher. Typically generated as a GUID and stored as a string.
    /// This ID differentiates one UserVoucher record from another.
    /// </summary>
    public string UserVoucherId { get; set; }

    /// <summary>
    /// The unique identifier (e.g., a user ID) of the user who owns or has been assigned this voucher.
    /// This links the voucher to a specific user's account.
    /// </summary>
    public string UserId { get; init; }

    /// <summary>
    /// A Data Transfer Object containing the details of the voucher itself, including its rate, terms,
    /// and any lodging or room information associated with it.
    /// </summary>
    public VoucherDto Voucher { get; init; }

    /// <summary>
    /// A Data Transfer Object containing details about the specific room the voucher is linked to.
    /// This makes it clear which room within a lodging the voucher covers.
    /// </summary>
    public RoomDto Room { get; init; }
}