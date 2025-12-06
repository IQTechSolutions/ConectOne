using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a template for storing general information with a name and optional details.
    /// </summary>
    /// <remarks>This class is designed to hold a name and associated information, which can be used in
    /// various contexts where general-purpose data needs to be stored or referenced. The <see cref="Name"/> property is
    /// required, while the <see cref="Information"/> property is optional.</remarks>
    public class GeneralInformationTemplate : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the informational message or data associated with the current context.
        /// </summary>
        public string? Information { get; set; }
    }
}
