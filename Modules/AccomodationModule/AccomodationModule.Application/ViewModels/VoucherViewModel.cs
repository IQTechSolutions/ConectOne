using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a voucher, containing details about the voucher's attributes,  associated lodging,
    /// and available rooms.
    /// </summary>
    /// <remarks>This class is typically used to transfer voucher-related data between the application layers,
    /// such as from the data access layer to the presentation layer. It encapsulates information  about the voucher,
    /// including its descriptions, pricing details, features, terms, and  associated lodging and rooms.</remarks>
    public class VoucherViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherViewModel"/> class.
        /// </summary>
        public VoucherViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherViewModel"/> class using the specified voucher data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="VoucherDto"/> to the
        /// corresponding properties of the <see cref="VoucherViewModel"/>. It also initializes nested objects such as
        /// <see cref="LodgingViewModel"/> and a collection of <see cref="RoomViewModel"/> instances.</remarks>
        /// <param name="voucher">The <see cref="VoucherDto"/> object containing the data to populate the view model. This parameter must not
        /// be null, and its properties should be properly initialized.</param>
        public VoucherViewModel(VoucherDto voucher)
        {
            VoucherId = voucher.VoucherId!.Value;
            Title = voucher.Title;
            ShortDescription = voucher.ShortDescription;
            LongDescription = voucher.LongDescription;
            Rate = voucher.Rate;
            MarkupPercentage = voucher.MarkupPercentage;
            Commission = voucher.Commission;
            Features = voucher.Features;
            Terms = voucher.Terms;
            Active = voucher.Active;
            Featured = voucher.Featured;

            Lodging = new LodgingViewModel(voucher.Lodging!);
            Rooms = voucher.Rooms.Select(c => new RoomViewModel(c)).ToList();

        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for a voucher.
        /// </summary>
        public int VoucherId { get; set; }

        /// <summary>
        /// Gets or sets the title of the item.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the item.
        /// </summary>
        public string ShortDescription { get; set; } = null!;

        /// <summary>
        /// Gets or sets the detailed description of the item.
        /// </summary>
		public string LongDescription { get; set; } = null!;

        /// <summary>
        /// Gets or sets the rate value.
        /// </summary>
		public double Rate { get; set; }

        /// <summary>
        /// Gets or sets the markup percentage applied to the base price.
        /// </summary>
        public double MarkupPercentage { get; set; }

        /// <summary>
        /// Gets or sets the commission percentage applied to a transaction.
        /// </summary>
        public double Commission { get; set; }

        /// <summary>
        /// Gets or sets the features associated with the current object.
        /// </summary>
        public string Features { get; set; } = null!;

        /// <summary>
        /// Gets or sets the terms and conditions associated with the current entity.
        /// </summary>
		public string Terms { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
		public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
		public bool Featured { get; set; }

        /// <summary>
        /// Gets or sets the lodging information associated with the current context.
        /// </summary>
		public LodgingViewModel Lodging { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of rooms represented by <see cref="RoomViewModel"/> instances.
        /// </summary>
		public ICollection<RoomViewModel> Rooms { get; set; } = new List<RoomViewModel>();
    }
}
