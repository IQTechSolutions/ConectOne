using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing a vacation host.
    /// </summary>
    public class VacationHostViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationHostViewModel"/> class.
        /// </summary>
        public VacationHostViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationHostViewModel"/> class with the specified DTO.
        /// </summary>
        /// <param name="vacationHost">The DTO containing vacation host data.</param>
        public VacationHostViewModel(VacationHostDto vacationHost)
        {
            VacationHostId = vacationHost.VacationHostId;
            Name = vacationHost.Name;
            Description = vacationHost.Description;
            Address = vacationHost.Address;
            Suburb = vacationHost.Suburb;
            Area = vacationHost.Area;
            Lat = vacationHost.Lat;
            Lng = vacationHost.Lng;
            RepresentativeTitle = vacationHost.RepresentativeTitle;
            RepresentativeName = vacationHost.RepresentativeName;
            RepresentativeSurname = vacationHost.RepresentativeSurname;
            RepresentativePhone = vacationHost.RepresentativePhone;
            RepresentativeEmail = vacationHost.RepresentativeEmail;
            RepresentativeBio = vacationHost.RepresentativeBio;
            ImgPath = vacationHost.ImgPath;
            Images = vacationHost.Images;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the vacation host.
        /// </summary>
        public string VacationHostId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the vacation host.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the vacation host.
        /// </summary>
        public string? Description { get; set; }

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
        public AreaDto Area { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// Gets or sets the image path of the vacation host.
        /// </summary>
        public string? ImgPath { get; set; }

        /// <summary>
        /// Gets or sets the title of the representative.
        /// </summary>
        public Title RepresentativeTitle { get; set; } = Title.Me;

        /// <summary>
        /// Gets or sets the name of the representative.
        /// </summary>
        public string? RepresentativeName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the surname of the representative.
        /// </summary>
        public string? RepresentativeSurname { get; set; } = null!;

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

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the vacation host to a <see cref="VacationHostDto"/>.
        /// </summary>
        /// <remarks>This method creates a new <see cref="VacationHostDto"/> object and populates it with
        /// the  corresponding property values from the current instance. It is typically used to transfer  vacation
        /// host data in a format suitable for data transfer or serialization.</remarks>
        /// <returns>A <see cref="VacationHostDto"/> containing the data from the current instance.</returns>
        public VacationHostDto ToDto()
        {
            return new VacationHostDto
            {
                VacationHostId = VacationHostId,
                Name = Name,
                Description = Description,
                Address = Address,
                Suburb = Suburb,
                Area = Area,
                Lat = Lat,
                Lng = Lng,
                RepresentativeName = RepresentativeName,
                RepresentativeSurname = RepresentativeSurname,
                RepresentativePhone = RepresentativePhone,
                RepresentativeEmail = RepresentativeEmail,
                RepresentativeBio = RepresentativeBio,
                Images = Images,

                ImgPath = ImgPath
            };
        }

        #endregion
    }
}