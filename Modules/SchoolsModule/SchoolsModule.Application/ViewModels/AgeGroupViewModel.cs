using System.ComponentModel.DataAnnotations;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for an age group, including its identifier, name, and age range.
    /// </summary>
    /// <remarks>This class is typically used to transfer age group data between the application and the user
    /// interface. It provides properties for the age group's unique identifier, name, and minimum and maximum age
    /// limits.</remarks>
    public class AgeGroupViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AgeGroupViewModel"/> class.
        /// </summary>
        public AgeGroupViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgeGroupViewModel"/> class using the specified data transfer
        /// object.
        /// </summary>
        /// <param name="dto">The data transfer object containing the age group details. Cannot be <see langword="null"/>.</param>
        public AgeGroupViewModel(AgeGroupDto dto) 
        {
            AgeGroupId = dto.AgeGroupId;
            Name = dto.Name;
            MinAge = dto.MinAge;
            MaxAge = dto.MaxAge;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the age group.
        /// </summary>
        public string? AgeGroupId { get; set; } 

        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        [Required] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the minimum age value.
        /// </summary>
        /// <remarks>The value must be a positive integer. If a value less than 1 is provided, a
        /// validation error will occur.</remarks>
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")] public int MinAge { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowable age, in years.
        /// </summary>
        public int MaxAge { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Converts the current instance of the age group to its corresponding data transfer object (DTO).
        /// </summary>
        /// <returns>An <see cref="AgeGroupDto"/> representing the current age group, including its identifier, name, and age
        /// range.</returns>
        public AgeGroupDto ToDto() => new AgeGroupDto
        {
            AgeGroupId = AgeGroupId,
            Name = Name,
            MinAge = MinAge,
            MaxAge = MaxAge
        };  

        #endregion
    }
}
