using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// View model for <see cref="CustomVariableTag"/> entities.
/// </summary>
public class CustomVariableTagViewModel
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomVariableTagViewModel"/> class.
    /// </summary>
    public CustomVariableTagViewModel() { }

    /// <summary>
    /// Initializes a new instance from the specified DTO.
    /// </summary>
    /// <param name="dto">The DTO source.</param>
    public CustomVariableTagViewModel(CustomVariableTagDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Value = dto.Value;
        Description = dto.Description;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name associated with the entity.
    /// </summary>
    [Required] public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the value associated with this instance.
    /// </summary>
    [Required] public string Value { get; set; } = null!;

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current instance of the <see cref="CustomVariableTag"/> class to a <see
    /// cref="CustomVariableTagDto"/> object.
    /// </summary>
    /// <returns>A <see cref="CustomVariableTagDto"/> object containing the data from the current instance.</returns>
    public CustomVariableTagDto ToDto()
    {
        return new CustomVariableTagDto()
        {
            Id = Id,
            Name = Name,
            Value = Value,
            Description = Description
        };
    }
       
    #endregion

}
