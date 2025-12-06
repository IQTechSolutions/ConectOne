using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// View model for <see cref="DayTourActivityTemplate"/> instances.
/// </summary>
public class DayTourActivityTemplateViewModel : EntityBase<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DayTourActivityTemplateViewModel"/> class.
    /// </summary>
    public DayTourActivityTemplateViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DayTourActivityTemplateViewModel"/> class from a DTO.
    /// </summary>
    /// <param name="dto">DTO to map from.</param>
    public DayTourActivityTemplateViewModel(DayTourActivityTemplateDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Summary = dto.Summary;
        Description = dto.Description;
        GuestType = dto.GuestType;
        DisplayInOverview = dto.DisplayInOverview;
    }

    /// <summary>
    /// Gets or sets the template identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the template name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the summary information.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the activity description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the guest type applicable to this activity.
    /// </summary>
    public GuestType GuestType { get; set; }

    /// <summary>
    /// Indicates if the activity should be shown in overview.
    /// </summary>
    public bool DisplayInOverview { get; set; } = true;
}
