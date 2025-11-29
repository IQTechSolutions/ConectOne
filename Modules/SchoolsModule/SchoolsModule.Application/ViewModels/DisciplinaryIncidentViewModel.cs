using System.ComponentModel.DataAnnotations;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels;

/// <summary>
/// Represents a view model for a disciplinary incident, providing properties to manage and display details such as the
/// incident's unique identifier, date, description, associated learner, disciplinary action, and severity score.
/// </summary>
/// <remarks>This class is designed to facilitate the transfer of disciplinary incident data between the
/// application layers, such as from the data layer to the presentation layer. It can be initialized directly or
/// populated using a data transfer object (<see cref="DisciplinaryIncidentDto"/>).</remarks>
public class DisciplinaryIncidentViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisciplinaryIncidentViewModel"/> class.
    /// </summary>
    public DisciplinaryIncidentViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisciplinaryIncidentViewModel"/> class using the specified data
    /// transfer object.
    /// </summary>
    /// <remarks>The properties of the <see cref="DisciplinaryIncidentViewModel"/> are populated based on the
    /// values provided in the <paramref name="dto"/>. Ensure that the <paramref name="dto"/> contains valid and
    /// complete data before passing it to this constructor.</remarks>
    /// <param name="dto">The data transfer object containing the disciplinary incident details. Cannot be null.</param>
    public DisciplinaryIncidentViewModel(DisciplinaryIncidentDto dto)
    {
        DisciplinaryIncidentId = dto.DisciplinaryIncidentId;
        Date = dto.Date;
        Description = dto.Description;
        LearnerId = dto.LearnerId;
        DisciplinaryActionId = dto.DisciplinaryActionId;
        SeverityScore = dto.SeverityScore;
    }

    /// <summary>
    /// Gets or sets the unique identifier for a disciplinary incident.
    /// </summary>
    public string? DisciplinaryIncidentId { get; set; }

    /// <summary>
    /// Gets or sets the date associated with the entity.
    /// </summary>
    [Required] public DateTime? Date { get; set; }

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the learner.
    /// </summary>
    public string? LearnerId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the disciplinary action.
    /// </summary>
    public string? DisciplinaryActionId { get; set; }

    /// <summary>
    /// Gets or sets the severity score associated with the current instance.
    /// </summary>
    public int SeverityScore { get; set; }

    #region Methods

    /// <summary>
    /// Converts the current disciplinary incident to its corresponding data transfer object (DTO).
    /// </summary>
    /// <returns>A <see cref="DisciplinaryIncidentDto"/> representing the current disciplinary incident,  including its ID, date,
    /// description, learner ID, disciplinary action ID, and severity score. If the <c>Date</c> property is null, the
    /// current UTC date and time is used.</returns>
    public DisciplinaryIncidentDto ToDto()
    {
        return new DisciplinaryIncidentDto
        {
            DisciplinaryIncidentId = DisciplinaryIncidentId,
            Date = Date ?? DateTime.UtcNow,
            Description = Description,
            LearnerId = LearnerId,
            DisciplinaryActionId = DisciplinaryActionId,
            SeverityScore = SeverityScore
        };
    }

    #endregion
}
