using ConectOne.Domain.RequestFeatures;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated requests related to age groups.
    /// </summary>
    /// <remarks>This class provides options for specifying the page number, page size, and sort order when
    /// retrieving paginated data for age groups. By default, the page number is set to 1, and the page size is set to
    /// 50.</remarks>
    public class AgeGroupPageParameters : RequestParameters { }
}
