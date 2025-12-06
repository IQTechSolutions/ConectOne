using AccomodationModule.Domain.DataTransferObjects;
using FilingModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing a destination.
    /// </summary>
    public class DestinationViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DestinationViewModel"/> class.
        /// </summary>
        public DestinationViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DestinationViewModel"/> class with the specified DTO.
        /// </summary>
        /// <param name="destination">The DTO containing destination data.</param>
        public DestinationViewModel(DestinationDto destination)
        {
            DestinationId = destination.DestinationId;
            VacationId = destination.VacationId;
            Name = destination.Name;
            Description = destination.Description;
            OnlineDescription = destination.OnlineDescription;

            Address = destination.Address ?? string.Empty;
            Lat = destination.Lat;
            Lng = destination.Lng;

            Images = destination.Images;
            Videos = destination.Videos;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the destination.
        /// </summary>
        public string DestinationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier for the associated vacation.
        /// </summary>
        public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the destination.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the destination.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the proposal.
        /// </summary>
        public string? OnlineDescription { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; set; }

        #endregion

        #region Videos & Images

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of video files to be displayed.
        /// </summary>
        public List<VideoDto> Videos { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance to a <see cref="DestinationDto"/>.
        /// </summary>
        /// <returns>A <see cref="DestinationDto"/> object containing the data from the current instance.</returns>
        public DestinationDto ToDto()
        {
            return new DestinationDto
            {
                DestinationId = DestinationId,
                Name = Name,
                Description = Description,
                OnlineDescription = OnlineDescription,
                Address = Address,
                Lat = Lat,
                Lng = Lng,
            };
        }

        #endregion
    }
}