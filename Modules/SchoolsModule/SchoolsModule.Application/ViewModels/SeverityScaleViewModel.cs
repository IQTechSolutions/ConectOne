using System.ComponentModel.DataAnnotations;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels;

/// <summary>
/// Represents a view model for a severity scale, providing properties to manage its identifier, name, score, and
/// description.
/// </summary>
/// <remarks>This class is designed to facilitate the interaction between the UI and the underlying data model for
/// severity scales. It includes methods for converting to and from a data transfer object (<see
/// cref="SeverityScaleDto">).</remarks>
public class SeverityScaleViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SeverityScaleViewModel"/> class.
    /// </summary>
    public SeverityScaleViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SeverityScaleViewModel"/> class using the specified data transfer
    /// object.
    /// </summary>
    /// <param name="dto">The data transfer object containing the severity scale details. Cannot be <see langword="null"/>.</param>
    public SeverityScaleViewModel(SeverityScaleDto dto)
    {
        SeverityScaleId = dto.SeverityScaleId;
        Name = dto.Name;
        Score = dto.Score;
        Description = dto.Description;
    }

    /// <summary>
    /// Gets or sets the identifier for the severity scale associated with this instance.
    /// </summary>
    public string? SeverityScaleId { get; set; }

    /// <summary>
    /// Gets or sets the name associated with the entity.
    /// </summary>
    [Required] public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the score associated with the current instance.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    #region Methods

    /// <summary>
    /// Converts the current instance of the severity scale to a data transfer object (DTO).
    /// </summary>
    /// <returns>A <see cref="SeverityScaleDto"/> representing the current severity scale, including its ID, name, score, and
    /// description.</returns>
    public SeverityScaleDto ToDto()
    {
        return new SeverityScaleDto
        {
            SeverityScaleId = SeverityScaleId,
            Name = Name,
            Score = Score,
            Description = Description
        };
    }

    #endregion
}
