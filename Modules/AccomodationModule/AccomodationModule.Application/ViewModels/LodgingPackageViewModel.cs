using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for service amenities.
    /// </summary>
    /// <remarks>
    /// This class is used to transfer data related to service amenities between the application layer and the presentation layer.
    /// </remarks>
    public class LodgingPackageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingPackageViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see
        /// cref="LodgingPackageViewModel"/> class. Use this constructor when no initial data needs to be
        /// provided.</remarks>
        public LodgingPackageViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingPackageViewModel"/> class, representing a lodging
        /// package with associated details.
        /// </summary>
        /// <remarks>This constructor initializes the view model with details from the provided <paramref
        /// name="package"/> object, including package descriptions, special rate information, and room details. The
        /// <paramref name="bbid"/> parameter allows for optional association with a unique product partner.</remarks>
        /// <param name="package">The package data transfer object containing information about the lodging package. Cannot be null.</param>
        /// <param name="bbid">The unique identifier for the product partner. Can be null.</param>
        /// <param name="productId">The identifier for the product associated with the lodging package. Cannot be null or empty.</param>
        /// <param name="productName">The name of the product associated with the lodging package. Cannot be null or empty.</param>
        public LodgingPackageViewModel(LodgingPackageDto package, string? bbid, string productId, string productName)
        {
            PackageId = package.PackageId;
            ProductId = productId;
            ProductName = productName;
            UniqueProductPartnerId = bbid;
            SpecialRateId = package.SpecialRateId;
            PackageName = package.ShortDescription;
            PackageDescription = package.LongDescription;

            Details = package.Rooms?.Select(c => new RoomViewModel(c)).ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingPackageViewModel"/> class with the specified identifiers
        /// and product details.
        /// </summary>
        /// <param name="bbid">The unique identifier for the product's partner.</param>
        /// <param name="productId">The identifier for the product.</param>
        /// <param name="productName">The name of the product.</param>
        /// <param name="specialRateId">The identifier for the special rate associated with the product.</param>
        public LodgingPackageViewModel(string bbid, string productId, string productName, string specialRateId)
        {
            ProductId = productId;
            ProductName = productName;
            UniqueProductPartnerId = bbid;
            SpecialRateId = specialRateId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingPackageViewModel"/> class using the specified package
        /// data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="PackageDto"/> object
        /// to the corresponding properties of the <see cref="LodgingPackageViewModel"/>. It also initializes the <see
        /// cref="Details"/> property by transforming the room data from the package into a collection of <see
        /// cref="RoomViewModel"/> instances.</remarks>
        /// <param name="package">The package data used to populate the view model. Must not be <see langword="null"/>.</param>
        public LodgingPackageViewModel(LodgingPackageDto package)
        {
            PackageId = package.PackageId;
            ProductId = package.LodgingId;
            PackageName = package.ShortDescription;
            UniqueProductPartnerId = package.AvailablePartnerUid;
            PackageDescription = package.LongDescription;
            Active = !package.Deleted;
            SpecialRateId = package.SpecialRateId;
            Details = package.Rooms.Select(c => new RoomViewModel(c)).ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingPackageViewModel"/> class,  representing a view model
        /// for a lodging package.
        /// </summary>
        /// <remarks>This constructor maps the provided lodging product and package details to the
        /// corresponding  properties of the view model. It also initializes the <see cref="Details"/> property by 
        /// transforming the package's room data into a collection of <see cref="RoomViewModel"/> objects.</remarks>
        /// <param name="product">The lodging product details, represented by a <see cref="LodgingDto"/> object.  Must not be <see
        /// langword="null"/>.</param>
        /// <param name="package">The package details, represented by a <see cref="PackageDto"/> object.  Must not be <see langword="null"/>.</param>
        public LodgingPackageViewModel(LodgingDto product, LodgingPackageDto package)
        {
            PackageId = package.PackageId;

            ProductId = product.ProductId;
            ProductName = product.Name;
            UniqueProductPartnerId = product.UniqueProductPartnerId;
            SpecialRateId = package.SpecialRateId;
            PackageName = package.ShortDescription;
            PackageDescription = package.LongDescription;
            Active = !package.Deleted;

            Details = package.Rooms.Select(c => new RoomViewModel(c)).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the package.
        /// </summary>
        public int PackageId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public string? ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the product partner associated with this entity.
        /// </summary>
        public string? UniqueProductPartnerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the package.
        /// </summary>
        public string? PackageName { get; set; }

        /// <summary>
        /// Gets or sets the description of the package.
        /// </summary>
        public string? PackageDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all rooms are available.
        /// </summary>
        public bool AllRoomsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the identifier for a special rate associated with the current entity.
        /// </summary>
        public string SpecialRateId { get; set; }

        /// <summary>
        /// Gets or sets the collection of room details represented as view models.
        /// </summary>
        public List<RoomViewModel> Details { get; set; } = new List<RoomViewModel>();

        #endregion

        #region Properties

        /// <summary>
        /// Converts the current package instance to a <see cref="PackageDto"/>.
        /// </summary>
        /// <returns>A <see cref="PackageDto"/> object containing the mapped properties of the current package.</returns>
        public LodgingPackageDto ToDto()
        {
            return new LodgingPackageDto
            {
                PackageId = PackageId,
                AvailablePartnerUid = UniqueProductPartnerId,
                ShortDescription = PackageName!,
                LongDescription = PackageDescription!,
                SpecialRateId = SpecialRateId,
                LodgingId = ProductId
            };
        }

        #endregion
    }
}
