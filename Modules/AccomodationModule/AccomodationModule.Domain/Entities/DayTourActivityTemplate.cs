using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a template for a day tour activity, including details such as name, date, time, description, and
    /// guest type.
    /// </summary>
    /// <remarks>This class is designed to encapsulate the essential information required to define a day tour
    /// activity. It includes properties for specifying the activity's name, date, start time, summary, detailed
    /// description,  intended guest type, and whether the activity should be displayed in an overview.</remarks>
    public class DayTourActivityTemplate : EntityBase<string>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTourActivityTemplate"/> class.
        /// </summary>
        public DayTourActivityTemplate() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayTourActivityTemplate"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>The <see cref="Id"/> property is automatically initialized with a new unique
        /// identifier.</remarks>
        /// <param name="dto">The data transfer object containing the initial values for the activity template, including its name,
        /// summary, description, guest type, and display settings.</param>
        public DayTourActivityTemplate(DayTourActivityTemplateDto dto)
        {
            Id = Guid.NewGuid().ToString();
            Name = dto.Name;
            Summary = dto.Summary;
            Description = dto.Description;
            GuestType = dto.GuestType;
            DisplayInOverview = dto.DisplayInOverview;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the day tour activity.
        /// Maximum length: 1000 characters.
        /// </summary>
        [MaxLength(1000)] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the summary of the day tour activity.
        /// Maximum length: 5000 characters.
        /// </summary>
        [MaxLength(5000)] public string? Summary { get; set; }

        /// <summary>
        /// Gets or sets the detailed description of the day tour activity.
        /// Maximum length: 10000 characters.
        /// </summary>
        [MaxLength(10000)] public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the type of guest for whom the day tour activity is intended.
        /// </summary>
        public GuestType GuestType { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the day tour activity.
        /// </summary>
        public bool DisplayInOverview { get; set; } = true;

        #endregion
    }
}
