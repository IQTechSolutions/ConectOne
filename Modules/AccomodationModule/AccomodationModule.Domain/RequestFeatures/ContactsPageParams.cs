using ConectOne.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Arguments
{
    /// <summary>
    /// Represents the parameters used for paginating, sorting, and filtering contact data in API requests.
    /// Inherits from <see cref="RequestParameters"/> to include common request parameters.
    /// </summary>
    public class ContactsPageParams : RequestParameters
    {
        /// <summary>
        /// Gets or sets the type of contact to filter by.
        /// </summary>
        public int? ContactType { get; set; }
    }
}
