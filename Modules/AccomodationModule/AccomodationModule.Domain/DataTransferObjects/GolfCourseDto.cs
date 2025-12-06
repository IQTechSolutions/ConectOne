using AccomodationModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a golf course.
    /// </summary>
    public record GolfCourseDto 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfCourseDto"/> class.
        /// </summary>
        public GolfCourseDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfCourseDto"/> class with the specified entity.
        /// </summary>
        /// <param name="golfCourse">The entity containing golf course data.</param>
        public GolfCourseDto(GolfCourse golfCourse)
        {
            GolfCourseId = golfCourse.Id;
            Name = golfCourse.Name;
            Description = golfCourse.Description;
            OnlineDescription = golfCourse.OnlineDescription;

            Address = golfCourse.Address ?? string.Empty;
            Lat = golfCourse.Lat;
            Lng = golfCourse.Lng;
            CourseType = golfCourse.CourseType;
            Carts = golfCourse.Carts;
            Caddies = golfCourse.Caddies;
            GolfClubs = golfCourse.GolfClubs;
            DesignedBy = golfCourse.DesignedBy ?? string.Empty;
            Ranking = golfCourse.Ranking;

            Destinations = golfCourse.Destinations.Select(d => new DestinationDto(d.Destination)).ToList() ?? [];

            Images = golfCourse?.Images == null ? [] : golfCourse.Images.Select(ImageDto.ToDto).ToList();
            Videos = golfCourse?.Videos == null ? [] : golfCourse.Videos.Select(VideoDto.ToDto).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or initializes the unique identifier for the golf course.
        /// </summary>
        public string GolfCourseId { get; init; } = null!;

        /// <summary>
        /// Gets or initializes the unique identifier for the associated vacation.
        /// </summary>
        public string? VacationId { get; init; } = null!;

        /// <summary>
        /// Gets or initializes the name of the golf course.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets or initializes the description of the golf course.
        /// </summary>
        public string Description { get; init; } = null!;

        /// <summary>
        /// Gets the online description associated with the entity.
        /// </summary>
        public string? OnlineDescription { get; init; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string? Address { get; set; } 

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; init; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether carts are enabled.
        /// </summary>
        public bool Carts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether caddies are enabled.
        /// </summary>
        public bool Caddies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether golf clubs are available.
        /// </summary>
        public bool GolfClubs { get; set; }

        /// <summary>
        /// Gets or sets the type of the course.
        /// </summary>
        public string? CourseType { get; init; } 

        /// <summary>
        /// Gets or sets the name of the designer associated with the object.
        /// </summary>
        public string? DesignedBy { get; init; } 

        /// <summary>
        /// Gets or sets the ranking value associated with the entity.
        /// </summary>
        public int Ranking { get; init; } 

        /// <summary>
        /// Gets the collection of destinations associated with the current entity.
        /// </summary>
        public ICollection<DestinationDto> Destinations { get; init; } = [];

        #region Images & Videos

        public ICollection<ImageDto> Images { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of video files to be displayed.
        /// </summary>
        public List<VideoDto> Videos { get; set; } = [];

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Converts this DTO into a <see cref="GolfCourse"/> entity.
        /// </summary>
        /// <returns>A <see cref="GolfCourse"/> entity with the DTO's data.</returns>
        public GolfCourse ToGolfCourse()
        {
            var destination = new GolfCourse()
            {
                Id = GolfCourseId,
                Name = Name,
                Description = Description,
                OnlineDescription = OnlineDescription,
                Address = Address,
                Lat = Lat,
                Lng = Lng,
                Carts = Carts,
                Caddies = Caddies,
                GolfClubs = GolfClubs,
                CourseType = CourseType,
                DesignedBy = DesignedBy,
                Ranking = Ranking,
            };

            return destination;
        }

        #endregion
    }
}