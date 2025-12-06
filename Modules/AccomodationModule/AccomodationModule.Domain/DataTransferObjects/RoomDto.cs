using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;
using ProductsModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data transfer object for a room, used to transfer room-related data between layers of the application.
    /// </summary>
    public record RoomDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomDto"/> class.
        /// </summary>
        public RoomDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomDto"/> class from a <see cref="Room"/> entity.
        /// </summary>
        /// <param name="detail">The room entity.</param>
        public RoomDto(Room detail)
        {
            UniqueServicePartnerId = detail.Package?.AvailablePartnerUid;
            UniqueServicePartnerRoomTypeId = detail.PartnerRoomTypeId;

            RoomTypeId = Convert.ToInt32(detail.Id);
            PackageId = detail.PackageId;
            LodgingId = detail.Package?.LodgingId;
            VoucherId = detail.VoucherId;

            Name = detail.Name;
            Description = detail.Description;
            AdditionalInfo = detail.AdditionalInfo;

            RoomCount = detail.RoomCount;
            MaxOccupancy = detail.MaxOccupancy;
            MaxAdults = detail.MaxAdults;
            FirstChildStaysFree = detail.FirstChildStaysFree;

            BookingTerms = detail.BookingTerms;
            CancellationPolicy = detail.CancellationPolicy;
            ChildPolicyDescription = detail.ChildPolicyDescription;

            ChildPolicyRules = detail.ChildPolicyRules.Select(c => new ChildPolicyRuleDto(c)).ToList();

            RateScheme = detail.RateScheme;
            CustomRate = detail.SpecialRate;
            VoucherRate = detail.VoucherRate;

            DefaultMealPlanId = detail.DefaultMealPlanId;
            DefaultBedTypeId = detail.DefaultBedTypeId;

            GalleryImages = detail.Images.Select(c => c.Image.RelativePath).ToList()!;
            FeaturedImages = detail.FeaturedImages.Select(c => new FeaturedImageDto(c)).ToList();
            MealPlans = detail.MealPlans.Select(c => new MealPlanDto(c)).ToList();
            BedTypes = detail.BedTypes.Select(c => new BedTypeDto(c)).ToList();
            Amenities = detail.Amneties.Select(c => new ServiceAmenityDto(c)).ToList();

            var imagePath = detail.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath;
            ImgPath = string.IsNullOrEmpty(imagePath) ? "_content/Accomodation.Blazor/images/NoImage.jpg" : imagePath;
        }

        #endregion

        #region Identities

        /// <summary>
        /// Gets or sets the unique service partner ID.
        /// </summary>
        public string? UniqueServicePartnerId { get; init; }

        /// <summary>
        /// Gets or sets the unique service partner room type ID.
        /// </summary>
        public int? UniqueServicePartnerRoomTypeId { get; init; }

        /// <summary>
        /// Gets or sets the lodging ID.
        /// </summary>
        public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the package ID.
        /// </summary>
        public int? PackageId { get; init; }

        /// <summary>
        /// Gets or sets the room type ID.
        /// </summary>
        public int? RoomTypeId { get; init; }

        /// <summary>
        /// Gets or sets the voucher ID.
        /// </summary>
        public int? VoucherId { get; init; }

        #endregion

        #region RoomInformation

        /// <summary>
        /// Gets or sets the name of the room.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets or sets the description of the room.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets the additional information about the room.
        /// </summary>
        public string? AdditionalInfo { get; init; }

        #endregion

        #region Settings

        /// <summary>
        /// Gets or sets the room count.
        /// </summary>
        public int RoomCount { get; init; }

        /// <summary>
        /// Gets or sets the maximum occupancy of the room.
        /// </summary>
        public int MaxOccupancy { get; init; }

        /// <summary>
        /// Gets or sets the maximum number of adults allowed in the room.
        /// </summary>
        public int MaxAdults { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the first child stays free in the room.
        /// </summary>
        public bool FirstChildStaysFree { get; init; }

        #endregion

        #region Policies

        /// <summary>
        /// Gets or sets the booking terms for the room.
        /// </summary>
        public string? BookingTerms { get; init; }

        /// <summary>
        /// Gets or sets the cancellation policy for the room.
        /// </summary>
        public string? CancellationPolicy { get; init; }

        /// <summary>
        /// Gets or sets the child policy description for the room.
        /// </summary>
        public string? ChildPolicyDescription { get; init; }

        /// <summary>
        /// Gets or sets the list of child policy rules for the room.
        /// </summary>
        public List<ChildPolicyRuleDto> ChildPolicyRules { get; init; } = [];

        #endregion

        #region Pricing

        /// <summary>
        /// Gets or sets the rate scheme for the room.
        /// </summary>
        public RateScheme? RateScheme { get; init; }

        /// <summary>
        /// Gets or sets the pricing details for the room.
        /// </summary>
        public PricingDto Pricing { get; init; } = null!;

        /// <summary>
        /// Gets or sets the custom rate for the room.
        /// </summary>
        public double CustomRate { get; init; }

        /// <summary>
        /// Gets or sets the voucher rate for the room.
        /// </summary>
        public double VoucherRate { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the default bed type ID.
        /// </summary>
        public string? DefaultBedTypeId { get; set; } = "";

        /// <summary>
        /// Gets or sets the collection of meal plans for the room.
        /// </summary>
        public ICollection<MealPlanDto> MealPlans { get; init; } = [];

        /// <summary>
        /// Gets or sets the default meal plan ID.
        /// </summary>
        public string? DefaultMealPlanId { get; set; }

        /// <summary>
        /// Gets or sets the collection of bed types for the room.
        /// </summary>
        public ICollection<BedTypeDto> BedTypes { get; init; } = [];

        /// <summary>
        /// Gets or sets the collection of amenities for the room.
        /// </summary>
        public ICollection<ServiceAmenityDto> Amenities { get; init; } = [];

        #endregion

        #region Images

        /// <summary>
        /// Gets or sets the image path for the room.
        /// </summary>
        public string? ImgPath { get; init; }

        /// <summary>
        /// Gets or sets the cover image for the room.
        /// </summary>
        public ImageDto? CoverImage { get; set; }

        /// <summary>
        /// Gets or sets the list of gallery images for the room.
        /// </summary>
        public List<string> GalleryImages { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of featured images for the room.
        /// </summary>
        public List<FeaturedImageDto> FeaturedImages { get; init; } = [];

        /// <summary>
        /// Gets or sets the list of gallery images to upload in base64 format.
        /// </summary>
        public List<string> GalleryImagesToUploadBase64 { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of gallery images to upload.
        /// </summary>
        public List<ImageDto> GalleryImagesToUpload { get; set; } = [];

        #endregion

        /// <summary>
        /// Converts the current instance to a <see cref="Room"/> entity.
        /// </summary>
        /// <returns>A new <see cref="Room"/> entity.</returns>
        public Room ToRoom()
        {
            return new Room()
            {
                PartnerRoomTypeId = this.UniqueServicePartnerRoomTypeId,
                PackageId = this.PackageId,
                VoucherId = this.VoucherId,
                LodgingId = this.LodgingId,

                Name = this.Name,
                Description = this.Description,
                AdditionalInfo = this.AdditionalInfo,

                RoomCount = this.RoomCount,
                MaxOccupancy = this.MaxOccupancy,
                MaxAdults = this.MaxAdults,

                BookingTerms = this.BookingTerms,
                CancellationPolicy = this.CancellationPolicy,

                RateScheme = this.RateScheme,
                SpecialRate = this.CustomRate,
                VoucherRate = this.VoucherRate,

                DefaultBedTypeId = !string.IsNullOrEmpty(this.DefaultBedTypeId) ? this.DefaultBedTypeId : "",
                DefaultMealPlanId = !string.IsNullOrEmpty(this.DefaultMealPlanId) ? this.DefaultMealPlanId : "",
            };
        }
    }
}
