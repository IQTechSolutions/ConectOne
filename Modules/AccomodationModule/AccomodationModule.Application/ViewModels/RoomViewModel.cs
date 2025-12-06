using System.ComponentModel;
using System.Text;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;
using Microsoft.AspNetCore.Http;
using ProductsModule.Application.ViewModels;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a room, encapsulating details such as room information, policies, pricing, 
    /// amenities, and associated images.
    /// </summary>
    /// <remarks>This class is designed to provide a comprehensive representation of a room, including its
    /// identity,  settings, policies, pricing, and associated collections such as meal plans, bed types, and amenities.
    /// It supports initialization from various data sources, such as DTOs or external APIs, to facilitate  integration
    /// with different systems.</remarks>
    public class RoomViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor initializes the <see cref="Pricing"/> property to a new instance of
        /// <see cref="PricingViewModel"/>.</remarks>
        public RoomViewModel() 
        { 
            Pricing = new PricingViewModel();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomViewModel"/> class using the provided room details.
        /// </summary>
        /// <remarks>This constructor maps the properties of the <see cref="RoomDto"/> object to the
        /// corresponding properties  of the <see cref="RoomViewModel"/> instance. It also initializes collections such
        /// as meal plans, bed types,  amenities, and images, converting them into their respective view model
        /// representations.  If the <paramref name="detail"/> contains multiple meal plans, the <see
        /// cref="SelectedMealPlan"/> property  is set to the default meal plan specified in the <paramref
        /// name="detail"/>. Similarly, if the <paramref name="detail"/>  contains bed types, the <see
        /// cref="SelectedBedType"/> property is set to the default bed type.</remarks>
        /// <param name="detail">A <see cref="RoomDto"/> object containing detailed information about the room, including its identifiers, 
        /// attributes, policies, pricing, and associated entities.</param>
        public RoomViewModel(RoomDto detail)
        {
            UniqueServicePartnerId = detail.UniqueServicePartnerId;
            PartnerRoomTypeId = detail.UniqueServicePartnerRoomTypeId;

            RoomTypeId = detail.RoomTypeId;
            PackageId = detail.PackageId;
            LodgingId = detail.LodgingId;
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

            ChildPolicyRules = detail.ChildPolicyRules.Select(c => new ChildPolicyRuleViewModel(c)).ToList();

            RateScheme = detail.RateScheme!.Value;
            CustomRate = detail.CustomRate;
            VoucherRate = detail.VoucherRate;
            Pricing = new PricingViewModel(detail.Pricing);

            if(detail.MealPlans.Count > 1)
                MealPlans = detail.MealPlans.Select(c => new MealplanViewModel(c)).ToList();
            
            BedTypes = detail.BedTypes.Select(c => new BedTypeViewModel(c)).ToList();

            if (detail.MealPlans.Count > 1)
                SelectedMealPlan = MealPlans?.FirstOrDefault(c => c.MealPlanId == detail.DefaultMealPlanId);

            if(detail.BedTypes.Count > 0)
                SelectedBedType = BedTypes.FirstOrDefault(c => c.BedTypeId == detail.DefaultBedTypeId);

            Amenities = detail.Amenities.Select(c => new ServiceAmenityViewModel(c)).ToList();
            GalleryImages = detail.GalleryImages;
            FeaturedImages = detail.FeaturedImages.Select(c => new FeaturedImageViewModel(c)).ToList();

            ImgPath = detail.ImgPath;
        }

        #endregion

        #region Identities

        /// <summary>
        /// Gets or sets the unique identifier for the service partner.
        /// </summary>
        [DisplayName("Unique Service Id")] public string? UniqueServicePartnerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the partner's room type.
        /// </summary>
        [DisplayName("Unique Service Room Id")] public int? PartnerRoomTypeId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the lodging entity.
        /// </summary>
        public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the package.
        /// </summary>
        public int? PackageId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the room type.
        /// </summary>
        public int? RoomTypeId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a voucher.
        /// </summary>
        public int? VoucherId { get; set; }

        #endregion

        #region RoomInfomation

        /// <summary>
        /// Gets or sets the name of the room.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description text associated with the object.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets additional information related to the current context.
        /// </summary>
        public string? AdditionalInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the first child in a group is exempt from charges.
        /// </summary>
        public bool FirstChildStaysFree { get; set; }

        #endregion

        #region Settings

        /// <summary>
        /// Gets or sets the number of rooms.
        /// </summary>
        [DisplayName("Rooms")] public int RoomCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of guests allowed.
        /// </summary>
        [DisplayName("Max Allowed Geusts")] public int MaxOccupancy { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of adults allowed.
        /// </summary>
        [DisplayName("Max Allowed Adults")] public int MaxAdults { get; set; }

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
        /// Gets a description of the child policy rules, formatted as a human-readable string.
        /// </summary>
        /// <remarks>The description is generated based on the defined child policy rules, including age
        /// ranges,  payment amounts, and custom descriptions. Each rule is listed in order of minimum age,  and the
        /// format varies depending on the type of rule.</remarks>
        public string ChildPolicyDescription
		{
			get
			{
				if (ChildPolicyRules.Count==0)
					return string.Empty;
				var returnString = new StringBuilder();

				foreach (var rule in ChildPolicyRules.OrderBy(c => c.MinAge))
				{
                    string stringToAdd;

                    if (string.IsNullOrEmpty(rule.CustomDescription))
                    {
						stringToAdd = rule.Rule switch
						{
							"N" => $"Children ages {rule.MinAge} - {rule.MaxAge} years are not allowed;" + Environment.NewLine,
							"R" => $"Children ages {rule.MinAge} - {rule.MaxAge} years pay a fixed amount of R{rule.Amount};" + Environment.NewLine,
							"P" => $"Children ages {rule.MinAge} - {rule.MaxAge} years pay {rule.Amount}% of price;" + Environment.NewLine,
							_ => "No child policy rule implemented;" + Environment.NewLine,
						};
					}
                    else
                    {
                        stringToAdd = rule.CustomDescription;
                    }

					

					returnString.AppendLine(stringToAdd);
				}

				return returnString.ToString();
			}
		}

        /// <summary>
        /// Gets or sets the collection of child policy rules.
        /// </summary>
		public List<ChildPolicyRuleViewModel> ChildPolicyRules { get; set; } = [];

        #endregion

        #region Pricing

        /// <summary>
        /// Gets or sets the cleaning fee associated with the booking.
        /// </summary>
        public double CleaningFee { get; set; }

        /// <summary>
        /// Gets or sets the rate scheme used to calculate pricing or fees.
        /// </summary>
        public RateScheme RateScheme { get; set; }

        /// <summary>
        /// Gets or sets the pricing details for the current view.
        /// </summary>
        public PricingViewModel Pricing { get; set; } = null!;

        /// <summary>
        /// Gets or sets the custom rate value.
        /// </summary>
        public double CustomRate { get; set; }

        /// <summary>
        /// Gets or sets the rate applied to vouchers.
        /// </summary>
        public double VoucherRate { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the currently selected meal plan.
        /// </summary>
        public MealplanViewModel? SelectedMealPlan { get; set; }

        /// <summary>
        /// Gets or sets the collection of meal plans.
        /// </summary>
        public List<MealplanViewModel>? MealPlans { get; set; } = [];

        /// <summary>
        /// Gets or sets the currently selected bed type.
        /// </summary>
        public BedTypeViewModel? SelectedBedType { get; set; }

        /// <summary>
        /// Gets or sets the collection of bed types available.
        /// </summary>
        public ICollection<BedTypeViewModel>? BedTypes { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of amenities associated with the service.
        /// </summary>
        public ICollection<ServiceAmenityViewModel>? Amenities { get; set; } = [];

        #endregion

        #region Images

        /// <summary>
        /// Gets or sets the file path to the image associated with this object.
        /// </summary>
        public string? ImgPath { get; set; }

        /// <summary>
        /// Gets or sets the collection of image URLs displayed in the gallery.
        /// </summary>
        public IList<string> GalleryImages { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of featured images associated with the current entity.
        /// </summary>
        public IList<FeaturedImageViewModel> FeaturedImages { get; set; } = [];

        /// <summary>
        /// Gets or sets the uploaded file representing the cover image.
        /// </summary>
        public IFormFile? CoverUpload { get; set; }

        /// <summary>
        /// Gets or sets the list of gallery image file paths to be uploaded.
        /// </summary>
        public List<string> GalleryImagesToUpload { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current room instance to a <see cref="RoomDto"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the current room to a new <see cref="RoomDto"/>
        /// instance, including details such as room type, package, lodging, policies, and associated collections like
        /// meal plans, bed types, and amenities. Collections and complex objects are converted to their respective DTO
        /// representations.</remarks>
        /// <returns>A <see cref="RoomDto"/> object containing the mapped data from the current room instance.</returns>
        public RoomDto ToDto()
        {
            return new RoomDto()
            {
                UniqueServicePartnerId = UniqueServicePartnerId,
                UniqueServicePartnerRoomTypeId = PartnerRoomTypeId,

                RoomTypeId = RoomTypeId,
                PackageId = PackageId,
                LodgingId = LodgingId,
                VoucherId = VoucherId,

                Name = Name,
                Description = Description,
                AdditionalInfo = AdditionalInfo,

                RoomCount = RoomCount,
                MaxOccupancy = MaxOccupancy,
                MaxAdults = MaxAdults,
                FirstChildStaysFree = FirstChildStaysFree,

                BookingTerms = BookingTerms,
                CancellationPolicy = CancellationPolicy,
                ChildPolicyDescription = ChildPolicyDescription,

                ChildPolicyRules = ChildPolicyRules.Select(c => c.ToDto()).ToList(),

                RateScheme = RateScheme,
                CustomRate = CustomRate,
                VoucherRate = VoucherRate,

                DefaultBedTypeId = !string.IsNullOrEmpty(SelectedBedType?.BedTypeId) ? SelectedBedType.BedTypeId : "1",
                DefaultMealPlanId = !string.IsNullOrEmpty(SelectedMealPlan?.MealPlanId)
                    ? SelectedMealPlan.MealPlanId
                    : "",

                MealPlans = MealPlans.Select(c => c.ToDto()).ToList(),
                BedTypes = BedTypes.Select(c => c.ToDto()).ToList(),
                Amenities = Amenities.Select(c => c.ToDto()).ToList(),
                ImgPath = ImgPath,

                GalleryImagesToUploadBase64 = GalleryImagesToUpload,
            };
        }

        #endregion
    }
}