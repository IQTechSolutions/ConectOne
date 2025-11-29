using System.ComponentModel.DataAnnotations;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels;

/// <summary>
/// Represents a view model for a disciplinary action, providing properties to manage its details and methods to convert
/// it to a data transfer object (DTO).
/// </summary>
/// <remarks>This class is designed to encapsulate the details of a disciplinary action, including its unique
/// identifier, name, description, and associated severity scale. It provides functionality to initialize the model from
/// a DTO and to convert it back to a DTO for data transfer purposes.</remarks>
public class DisciplinaryActionViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisciplinaryActionViewModel"/> class.
    /// </summary>
    public DisciplinaryActionViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisciplinaryActionViewModel"/> class using the specified data
    /// transfer object.
    /// </summary>
    /// <param name="dto">The data transfer object containing the disciplinary action details. Cannot be <see langword="null"/>.</param>
    public DisciplinaryActionViewModel(DisciplinaryActionDto dto)
    {
        DisciplinaryActionId = dto.DisciplinaryActionId;
        Name = dto.Name;
        Description = dto.Description;
        SeverityScaleId = dto.SeverityScaleId;
    }

    /// <summary>
    /// Gets or sets the unique identifier for the disciplinary action.
    /// </summary>
    public string? DisciplinaryActionId { get; set; }

    /// <summary>
    /// Gets or sets the name associated with the entity.
    /// </summary>
    [Required] public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier for the severity scale associated with this instance.
    /// </summary>
    public string? SeverityScaleId { get; set; }

    #region Methods

    /// <summary>
    /// Converts the current instance of the disciplinary action to its corresponding data transfer object (DTO).
    /// </summary>
    /// <returns>A <see cref="DisciplinaryActionDto"/> representing the current disciplinary action, including its ID, name,
    /// description, and severity scale ID.</returns>
    public DisciplinaryActionDto ToDto()
    {
        return new DisciplinaryActionDto
        {
            DisciplinaryActionId = DisciplinaryActionId,
            Name = Name,
            Description = Description,
            SeverityScaleId = SeverityScaleId
        };
    }

    #endregion
}
