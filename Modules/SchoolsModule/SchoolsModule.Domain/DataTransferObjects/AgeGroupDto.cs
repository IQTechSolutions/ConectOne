using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for an age group, containing information about the group's identifier,
    /// name, and age range.
    /// </summary>
    /// <remarks>This DTO is used to transfer age group data between different layers of the application. It
    /// includes properties for the age group's unique identifier, name, and the minimum and maximum ages that define
    /// the group. The <see cref="CreateAgeGroup"/> method can be used to convert this DTO into a domain model
    /// object.</remarks>
    public record AgeGroupDto
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AgeGroupDto"/> class.
        /// </summary>
        public AgeGroupDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgeGroupDto"/> class using the specified <see
        /// cref="AgeGroup"/>.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="AgeGroup"/> to the
        /// corresponding properties of the <see cref="AgeGroupDto"/>.</remarks>
        /// <param name="ageGroup">The <see cref="AgeGroup"/> instance from which to populate the DTO. Cannot be <see langword="null"/>.</param>
        public AgeGroupDto(AgeGroup ageGroup)
        {
            AgeGroupId = ageGroup.Id;
            Name = ageGroup.Name;
            MinAge = ageGroup.MinAge;
            MaxAge = ageGroup.MaxAge;
        }

		#endregion

        /// <summary>
        /// Gets the identifier for the age group.
        /// </summary>
		public string? AgeGroupId { get; init; }

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets or sets the minimum age required for eligibility.
        /// </summary>
        public int MinAge { get; set; }    

        /// <summary>
        /// Gets or sets the maximum allowable age, in years.
        /// </summary>
        public int MaxAge { get; set; }

        /// <summary>
        /// Creates a new <see cref="AgeGroup"/> instance based on the current object's properties.
        /// </summary>
        /// <remarks>The <see cref="AgeGroup.Id"/> property is automatically generated as a new GUID if 
        /// <see cref="AgeGroupId"/> is null or empty. Otherwise, the value of <see cref="AgeGroupId"/>  is used. The
        /// <see cref="AgeGroup.Name"/>, <see cref="AgeGroup.MinAge"/>, and  <see cref="AgeGroup.MaxAge"/> properties
        /// are populated from the corresponding properties  of the current object.</remarks>
        /// <returns>A new <see cref="AgeGroup"/> instance with the specified properties.</returns>
        public AgeGroup CreateAgeGroup()
        {
            return new AgeGroup
            {
                Id = string.IsNullOrEmpty(AgeGroupId) ? Guid.NewGuid().ToString() : AgeGroupId,
                Name = Name,
                MinAge = MinAge,
                MaxAge = MaxAge,
            };
        }
    }
}
