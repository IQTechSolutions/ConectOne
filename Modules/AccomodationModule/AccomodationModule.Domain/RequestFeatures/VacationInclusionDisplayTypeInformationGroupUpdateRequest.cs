using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Request object for updating vacation inclusion display type information.
    /// </summary>
    /// <param name="VacationId">The identity of the vacation that the display type sections are being updated for </param>
    /// <param name="Items">The items being updated</param>
    public record VacationInclusionDisplayTypeInformationGroupUpdateRequest(string VacationId, List<VacationInclusionDisplayTypeInformationDto> Items);
}
