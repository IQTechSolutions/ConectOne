using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a golfer package.
    /// Provides a lightweight representation of the <see cref="GolferPackage"/> entity
    /// for use in data transfer between layers or systems.
    /// </summary>
    public record GolferPackageDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolferPackageDto"/> class with default values.
        /// </summary>
        public GolferPackageDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GolferPackageDto"/> class using a <see cref="GolferPackage"/> entity.
        /// Copies the properties from the entity to the DTO.
        /// </summary>
        /// <param name="golferPackage">The <see cref="GolferPackage"/> entity to copy data from.</param>
        public GolferPackageDto(GolferPackage golferPackage)
        {
            GolferPackageId = golferPackage.Id;
            DayNr = golferPackage.DayNr;
            StartTime = golferPackage.StartTime;
            Notes = golferPackage.Notes;
            Carts = golferPackage.Carts;
            Caddies = golferPackage.Caddies;
            Halfway = golferPackage.Halfway;

            if(golferPackage.GolfCourse is not null)
                GolfCourse =  new GolfCourseDto(golferPackage.GolfCourse!);

            VacationId = golferPackage.VacationId;

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the golfer package.
        /// </summary>
        public string GolferPackageId { get; init; } = null!;

        /// <summary>
        /// Gets the day number represented by this instance.
        /// </summary>
        public int DayNr { get; init; }

        /// <summary>
        /// Gets the start time of the golfer package.
        /// </summary>
        public TimeSpan? StartTime { get; init; }

        /// <summary>
        /// Gets additional notes for the golfer package.
        /// </summary>
        public string? Notes { get; init; }

        /// <summary>
        /// Gets a value indicating whether carts are included in the golfer package.
        /// </summary>
        public bool Carts { get; init; }

        /// <summary>
        /// Gets a value indicating whether caddies are included in the golfer package.
        /// </summary>
        public bool Caddies { get; init; }

        /// <summary>
        /// Gets a value indicating whether halfway refreshments are included in the golfer package.
        /// </summary>
        public bool Halfway { get; init; }

        /// <summary>
        /// Gets the golf course details associated with the golfer package.
        /// </summary>
        public GolfCourseDto GolfCourse { get; init; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the associated vacation.
        /// </summary>
        public string? VacationId { get; set; }

        #endregion

        /// <summary>
        /// Converts this DTO into a <see cref="GolferPackage"/> entity for persistence in the database.
        /// </summary>
        /// <returns>A <see cref="GolferPackage"/> object with the same data.</returns>
        public GolferPackage ToGolferPackage()
        {
            return new GolferPackage()
            {
                Id = GolferPackageId,
                DayNr = DayNr,
                StartTime = StartTime,
                Notes = Notes,
                Carts = Carts,
                Caddies = Caddies,
                Halfway = Halfway,
                GolfCourseId = GolfCourse.GolfCourseId,
                VacationId = VacationId
            };
        }

        /// <summary>
        /// Updates the values of an existing <see cref="GolferPackage"/> entity with the properties of this DTO.
        /// </summary>
        /// <param name="golferPackage">The golfer package that needs to be updated</param>
        public void UpdateGolferPackageValues(in GolferPackage golferPackage)
        {
            golferPackage.DayNr = DayNr;
            golferPackage.StartTime = StartTime;
            golferPackage.Notes = Notes;
            golferPackage.Carts = Carts;
            golferPackage.Caddies = Caddies;
            golferPackage.Halfway = Halfway;
            golferPackage.GolfCourseId = GolfCourse.GolfCourseId;
        }
    }
}
