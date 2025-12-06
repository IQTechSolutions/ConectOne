using AccomodationModule.Domain.Entities;
using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a vacation host, encapsulating essential information such as the
    /// host's identifier, name, description, and associated images.
    /// </summary>
    /// <remarks>This class is designed to facilitate the transfer of vacation host data between different
    /// layers of the application, such as the domain model, view model, and external services. It provides constructors
    /// for initializing the DTO from various sources, including domain entities and view models.</remarks>
    public record VacationHostDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationHostDto"/> class.
        /// </summary>
        public VacationHostDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationHostDto"/> class using the specified <see
        /// cref="VacationHost"/> object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="VacationHost"/>
        /// object to the corresponding properties of the <see cref="VacationHostDto"/>. If the <see
        /// cref="VacationHost.Images"/> collection contains an image with <see cref="UploadType.Cover"/>, its relative
        /// path is assigned to the <c>ImgPath</c> property. Otherwise, a default placeholder image path is
        /// used.</remarks>
        /// <param name="vacationHost">The <see cref="VacationHost"/> object containing the data to initialize the DTO.</param>
        public VacationHostDto(VacationHost vacationHost)
        {
            VacationHostId = vacationHost.Id;
            Name = vacationHost.Name;
            Description = vacationHost.Description;
            Address = vacationHost.Address;
            Suburb = vacationHost.Suburb;
            Area = vacationHost.Area == null ? new AreaDto() : new AreaDto(vacationHost.Area);
            Lat = vacationHost.Lat;
            Lng = vacationHost.Lng;
            RepresentativeName = vacationHost.RepresentativeName2;
            RepresentativeSurname = vacationHost.RepresentativeSurname2;
            RepresentativePhone = vacationHost.RepresentativePhone;
            RepresentativeEmail = vacationHost.RepresentativeEmail;
            RepresentativeBio = vacationHost.RepresentativeBio;
            Images = vacationHost?.Images == null ? [] : vacationHost.Images.Select(c => ImageDto.ToDto(c)).ToList();

            ImgPath = vacationHost.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover) is null ? "_content/Accomodation.Blazor/images/NoImage.jpg" : vacationHost.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath;
        }

        #endregion
        
        /// <summary>
        /// Gets the unique identifier of the vacation host.
        /// </summary>
        public string VacationHostId { get; init; } = null!;

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string? Name { get; init; } 

        /// <summary>
        /// Gets the description associated with the current object.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the name of the suburb associated with the address.
        /// </summary>
        public string? Suburb { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public AreaDto? Area { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; set; }

        #region Reprasentative

        /// <summary>
        /// Gets or sets the title of the representative.
        /// </summary>
        public Title RepresentativeTitle { get; set; } = Title.Me;

        /// <summary>
        /// Gets or sets the name of the representative.
        /// </summary>
        public string? RepresentativeName { get; set; } 

        /// <summary>
        /// Gets or sets the surname of the representative.
        /// </summary>
        public string? RepresentativeSurname { get; set; } 

        /// <summary>
        /// Gets or sets the phone number of the representative.
        /// </summary>
        public string? RepresentativePhone { get; set; }

        /// <summary>
        /// Gets or sets the email address of the representative.
        /// </summary>
        public string? RepresentativeEmail { get; set; }

        /// <summary>
        /// Gets or sets the biography of the representative.
        /// </summary>
        public string? RepresentativeBio { get; set; }

        #endregion

        #region Images

        /// <summary>
        /// Gets the file path to the associated image.
        /// </summary>
        public string? ImgPath { get; init; }

        /// <summary>
        /// Gets or sets the cover image associated with the entity.
        /// </summary>
        public ImageDto? CoverImage { get; set; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        #endregion

        /// <summary>
        /// Converts the current object to a <see cref="VacationHost"/> instance.
        /// </summary>
        /// <remarks>This method creates a new <see cref="VacationHost"/> object and populates its
        /// properties based on the current object's state. If a cover image is present, it is added to the  <see
        /// cref="VacationHost.Images"/> collection as a cover image.</remarks>
        /// <returns>A <see cref="VacationHost"/> instance with properties initialized from the current object.</returns>
        public VacationHost ToVacationHost()
        {
            var vacationHost = new VacationHost()
            {
                Id = VacationHostId,
                Name = Name ?? "",
                Description = Description,
                Address = Address,
                Suburb = Suburb,
                AreaId = Area?.Id,
                Lat = Lat,
                Lng = Lng,
                RepresentativeName2 = RepresentativeName,
                RepresentativeSurname2 = RepresentativeSurname,
                RepresentativeEmail = RepresentativeEmail,
                RepresentativePhone = RepresentativePhone,
                RepresentativeBio = RepresentativeBio
            };

            return vacationHost;
        }
    }
}
