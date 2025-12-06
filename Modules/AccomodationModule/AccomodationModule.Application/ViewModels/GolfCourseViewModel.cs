using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing a golf course.
    /// </summary>
    public class GolfCourseViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfCourseViewModel"/> class.
        /// </summary>
        public GolfCourseViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfCourseViewModel"/> class with the specified DTO.
        /// </summary>
        /// <param name="golfCourse">The DTO containing golf course data.</param>
        public GolfCourseViewModel(GolfCourseDto golfCourse)
        {
            GolfCourseId = golfCourse.GolfCourseId;
            Name = golfCourse.Name;
            Description = golfCourse.Description;
            OnlineDescription = golfCourse.OnlineDescription;
            Address = golfCourse.Address ?? string.Empty;
            Lat = golfCourse.Lat;
            Lng = golfCourse.Lng;
            Carts = golfCourse.Carts;
            Caddies = golfCourse.Caddies;
            GolfClubs = golfCourse.GolfClubs;
            CourseType = golfCourse.CourseType;
            DesignedBy = golfCourse.DesignedBy;
            Ranking = golfCourse.Ranking;

            Images = golfCourse.Images;
            Videos = golfCourse.Videos;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the golf course.
        /// </summary>
        public string GolfCourseId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the golf course.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the golf course.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets the online description associated with the entity.
        /// </summary>
        public string? OnlineDescription { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string? Address { get; set; } 

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; set; }

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
        public string? CourseType { get; set; } 

        /// <summary>
        /// Gets or sets the name of the designer associated with the object.
        /// </summary>
        public string? DesignedBy { get; set; } 

        /// <summary>
        /// Gets or sets the ranking value associated with the entity.
        /// </summary>
        public int Ranking { get; set; }

        /// <summary>
        /// Gets or sets the destination details for the golf course.
        /// </summary>
        public IEnumerable<DestinationDto>? Destinations { get; set; } = [];
        
        #endregion

        #region Image & Videos

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
        /// Converts the current <see cref="GolfCourse"/> instance to a <see cref="GolfCourseDto"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the <see cref="GolfCourse"/> instance to a new
        /// <see cref="GolfCourseDto"/> object. If the <see cref="Address"/> or <see cref="DesignedBy"/> properties are
        /// null, they are replaced with an empty string in the resulting DTO. If the <see cref="Destinations"/>
        /// collection is null, an empty list is assigned to the corresponding property in the DTO.</remarks>
        /// <returns>A <see cref="GolfCourseDto"/> object that represents the current golf course, including its details and
        /// associated destinations.</returns>
        public GolfCourseDto ToDto(){
            return new GolfCourseDto()
            {
                GolfCourseId = GolfCourseId,
                Name = Name,
                Description = Description,
                OnlineDescription = OnlineDescription,

                Address = Address ?? string.Empty,
                Lat = Lat,
                Lng = Lng,
                CourseType = CourseType,
                Carts = Carts,
                Caddies = Caddies,
                GolfClubs = GolfClubs,
                DesignedBy = DesignedBy ?? string.Empty,
                Ranking = Ranking,

                Destinations = Destinations?.Select(d => new DestinationDto(d.ToDestination())).ToList() ?? [],
            };
        }

        #endregion
    }
}
