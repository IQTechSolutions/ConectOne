namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object for a lodging type, including its identifier, name, and description.
    /// </summary>
    /// <param name="Id">The unique identifier for the lodging type.</param>
    /// <param name="Name">The name of the lodging type.</param>
    /// <param name="Description">A description of the lodging type.</param>
    public record LodgingTypeDto(string Id, string Name, string? Description);
}
