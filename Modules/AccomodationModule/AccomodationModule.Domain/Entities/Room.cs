using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.Enums;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a room configuration, including its attributes, policies, pricing, and relationships.
    /// </summary>
    /// <remarks>The <see cref="Room"/> class provides detailed information about a room, including its name,
    /// description,  settings such as bed count and maximum occupancy, policies like cancellation and child policies, 
    /// pricing details, and associated relationships such as meal plans, bed types, and amenities. This class is
    /// designed to encapsulate all relevant data for managing room configurations in a booking system.</remarks>
    public class Room : FileCollection<Room, int>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see cref="Room"/> class with no
        /// initial properties set. Use this constructor when you need to create a room object without specifying any
        /// initial values.</remarks>
        public Room() { }   

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class by copying the properties from an existing <see
        /// cref="Room"/> instance.
        /// </summary>
        /// <remarks>This constructor creates a new <see cref="Room"/> object with the same values as the
        /// specified <paramref name="room"/>. All properties of the new instance are set to match the corresponding
        /// properties of the provided <paramref name="room"/>.</remarks>
        /// <param name="room">The <see cref="Room"/> instance to copy. Cannot be <see langword="null"/>.</param>
        public Room(Room room)
        {
            PartnerRoomTypeId = room.PartnerRoomTypeId;
            Name = room.Name;
            Description = room.Description;
            AdditionalInfo = room.AdditionalInfo;
            DefaultMealPlanId = room.DefaultMealPlanId;
            DefaultBedTypeId = room.DefaultBedTypeId;
            BedCount = room.BedCount;
            RoomCount = room.RoomCount;
            MaxOccupancy = room.MaxOccupancy;
            MaxAdults = room.MaxAdults;
            FirstChildStaysFree = room.FirstChildStaysFree;
            BookingTerms = room.BookingTerms;
            CancellationPolicy = room.CancellationPolicy;
            ChildPolicyRules = room.ChildPolicyRules;
            RateScheme = room.RateScheme;
            Commision = room.Commision; 
            MarkUp = room.MarkUp;
            SpecialRate = room.SpecialRate;
            VoucherRate = room.VoucherRate;
            MealPlans = room.MealPlans;
            BedTypes = room.BedTypes;
			Amneties = room.Amneties;
        }

        #endregion

        #region Room Information

        /// <summary>
        /// Gets or sets the identifier for the partner's room type.
        /// </summary>
        public int? PartnerRoomTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets additional information related to the current context.
        /// </summary>
        public string? AdditionalInfo { get; set; }

        #endregion

        #region Settings

        /// <summary>
        /// Gets or sets the number of beds available.
        /// </summary>
        public int BedCount { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms available.
        /// </summary>
        public int RoomCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of occupants allowed.
        /// </summary>
        public int MaxOccupancy { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of adults allowed.
        /// </summary>
        public int MaxAdults { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the first child in a group is exempt from charges.
        /// </summary>
        public bool FirstChildStaysFree { get; set; }

        #endregion

        #region Policies

        /// <summary>
        /// Gets or sets the terms and conditions associated with a booking.
        /// </summary>
        public string? BookingTerms { get; set; }

        /// <summary>
        /// Gets or sets the cancellation policy for the associated service or booking.
        /// </summary>
        public string? CancellationPolicy { get; set; }

        /// <summary>
        /// Gets or sets the description of the child policy.
        /// </summary>
        public string? ChildPolicyDescription { get; set; }

        /// <summary>
        /// Gets or sets the collection of child policy rules associated with the current policy.
        /// </summary>
		public ICollection<ChildPolicyRule> ChildPolicyRules { get; set; } = [];

        #endregion

        #region Settings

        /// <summary>
        /// Gets or sets the default bed type identifier.
        /// </summary>
        public string? DefaultBedTypeId { get; set; }

        /// <summary>
        /// Gets or sets the collection of bed types available.
        /// </summary>
        public ICollection<BedType> BedTypes { get; set; } = [];

        /// <summary>
        /// Gets or sets the identifier of the default meal plan.
        /// </summary>
        public string? DefaultMealPlanId { get; set; }

        /// <summary>
        /// Gets or sets the collection of meal plans associated with the entity.
        /// </summary>
        public ICollection<MealPlan> MealPlans { get; set; } = [];

        #endregion

        #region Pricing

        /// <summary>
        /// Gets or sets the rate scheme used for pricing calculations.
        /// </summary>
        public RateScheme? RateScheme { get; set; }

        /// <summary>
        /// Gets or sets the commission rate applied to transactions.
        /// </summary>
        public double Commision { get; set; } = 4;

        /// <summary>
        /// Gets or sets the markup percentage applied to the base price.
        /// </summary>
        public double MarkUp { get; set; } = 20;

        /// <summary>
        /// Gets or sets the special rate applied to calculations or transactions.
        /// </summary>
        public double SpecialRate { get; set; }

        /// <summary>
        /// Gets or sets the rate applied to vouchers.
        /// </summary>
        public double VoucherRate { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// Gets or sets the identifier of the associated package.
        /// </summary>
        [ForeignKey(nameof(Package))] public int? PackageId { get; set; }

        /// <summary>
        /// Gets or sets the package associated with the current operation.
        /// </summary>
        public LodgingPackage? Package { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated voucher.
        /// </summary>
        [ForeignKey(nameof(Voucher))] public int? VoucherId { get; set; }

        /// <summary>
        /// Gets or sets the voucher associated with the current transaction.
        /// </summary>
        public Voucher? Voucher { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated lodging.
        /// </summary>
        [ForeignKey(nameof(Lodging))] public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the voucher associated with the current transaction.
        /// </summary>
        public Lodging? Lodging { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the collection of featured images associated with the entity.
        /// </summary>
        public ICollection<FeaturedImage> FeaturedImages { get; set; } = []; 

        /// <summary>
        /// Gets or sets the collection of service amenities associated with the entity.
        /// </summary>
        public ICollection<ServiceAmenity> Amneties { get; set; } = [];

        #endregion
    }
}
