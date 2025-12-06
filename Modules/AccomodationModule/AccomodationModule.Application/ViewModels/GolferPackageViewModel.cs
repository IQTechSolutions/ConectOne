using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a ViewModel for managing golfer package details associated with a vacation.
    /// Provides properties for binding golfer package data in the UI.
    /// </summary>
    public class GolferPackageViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GolferPackageViewModel"/> class with default values.
        /// </summary>
        public GolferPackageViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GolferPackageViewModel"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>This constructor is typically used to create a view model instance from a <see
        /// cref="GolferPackageDto"/> object,  allowing the data to be displayed or manipulated in a user
        /// interface.</remarks>
        /// <param name="dto">The data transfer object containing the golfer package details.  The properties of the <see
        /// cref="GolferPackageViewModel"/> will be initialized based on the values in this object.</param>
        public GolferPackageViewModel(GolferPackageDto dto)
        {
            GolferPackageId = dto.GolferPackageId;
            DayNr = dto.DayNr;
            StartTime = dto.StartTime;
            Notes = dto.Notes;
            Carts = dto.Carts;
            Caddies = dto.Caddies;
            Halfway = dto.Halfway;
            GolfCourse = dto.GolfCourse;
            VacationId = dto.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the golfer package.
        /// </summary>
        public string GolferPackageId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the day number represented by this instance.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the start time of the golfer package.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Gets or sets additional notes for the golfer package.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether carts are included in the golfer package.
        /// </summary>
        public bool Carts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether caddies are included in the golfer package.
        /// </summary>
        public bool Caddies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether halfway refreshments are included in the golfer package.
        /// </summary>
        public bool Halfway { get; set; }

        /// <summary>
        /// Gets or sets the golf course details associated with the golfer package.
        /// </summary>
        public GolfCourseDto GolfCourse { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation details associated with the current entity.
        /// </summary>
        public string? VacationId { get; set; }

        #endregion

        #region Methodes

        /// <summary>
        /// Converts the current instance of the <see cref="GolferPackage"/> class to a <see cref="GolferPackageDto"/>
        /// object.
        /// </summary>
        /// <remarks>This method maps the properties of the <see cref="GolferPackage"/> instance to a new
        /// <see cref="GolferPackageDto"/> object. Use this method to transfer data between layers or to serialize the
        /// object for external use.</remarks>
        /// <returns>A <see cref="GolferPackageDto"/> object that contains the data from the current <see cref="GolferPackage"/>
        /// instance.</returns>
        public GolferPackageDto ToDto()
        {
            return new GolferPackageDto
            {
                GolferPackageId = this.GolferPackageId,
                DayNr = this.DayNr,
                StartTime = this.StartTime,
                Notes = this.Notes,
                Carts = this.Carts,
                Caddies = this.Caddies,
                Halfway = this.Halfway,
                GolfCourse = this.GolfCourse,
                VacationId = this.VacationId
            };
        }

        #endregion
    }
}
