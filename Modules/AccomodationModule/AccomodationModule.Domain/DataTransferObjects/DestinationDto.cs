using AccomodationModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a destination.
    /// </summary>
    public record DestinationDto 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DestinationDto"/> class.
        /// </summary>
        public DestinationDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DestinationDto"/> class with the specified entity.
        /// </summary>
        /// <param name="destination">The entity containing destination data.</param>
        public DestinationDto(Destination destination)
        {
            DestinationId = destination.Id;
            Name = destination.Name;
            Description = destination.Description;
            OnlineDescription = destination.OnlineDescription;

            Address = destination.Address ?? string.Empty;
            Lat = destination.Lat;
            Lng = destination.Lng;

            Images = destination?.Images == null ? [] : destination.Images.Select(ImageDto.ToDto).ToList();
            Videos = destination?.Videos == null ? [] : destination.Videos.Select(VideoDto.ToDto).ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DestinationDto"/> class with the specified entity and vacation ID.
        /// </summary>
        /// <param name="destination">The entity containing destination data.</param>
        /// <param name="vacationId">The ID of the associated vacation.</param>
        public DestinationDto(Destination destination, string vacationId)
        {
            DestinationId = destination.Id;
            Name = destination.Name;
            Description = destination.Description;
            VacationId = vacationId;
            OnlineDescription = destination.OnlineDescription;

            Address = destination.Address ?? string.Empty;
            Lat = destination.Lat;
            Lng = destination.Lng;

            Images = destination?.Images == null ? [] : destination.Images.Select(ImageDto.ToDto).ToList();
            Videos = destination?.Videos == null ? [] : destination.Videos.Select(VideoDto.ToDto).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or initializes the unique identifier for the destination.
        /// </summary>
        public string? DestinationId { get; init; } 

        /// <summary>
        /// Gets or initializes the unique identifier for the associated vacation.
        /// </summary>
        public string? VacationId { get; init; } 

        /// <summary>
        /// Gets or initializes the name of the destination.
        /// </summary>
        public string? Name { get; init; } 

        /// <summary>
        /// Gets or initializes the description of the destination.
        /// </summary>
        public string? Description { get; init; } 

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

        #endregion

        #region Methods

        /// <summary>
        /// Converts this DTO into a <see cref="Destination"/> entity.
        /// </summary>
        /// <returns>A <see cref="Destination"/> entity with the DTO's data.</returns>
        public Destination ToDestination()
        {
            var destination = new Destination()
            {
                Id = DestinationId,
                Name = Name,
                Description = Description,
                OnlineDescription = OnlineDescription,

                Address = Address,
                Lat = Lat,
                Lng = Lng
            };

            return destination;
        }

        #endregion
    }
}
