using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Specifies the type of contact within an organization or business context.
    /// </summary>
    /// <remarks>This enumeration is used to categorize contacts based on their roles or titles,  such as
    /// Founder, Owner, Consultant, etc. Each member of the enumeration is  associated with a description that provides
    /// a human-readable representation  of the contact type.</remarks>
    public enum ContactType
    {
        /// <summary>
        /// Represents the role of a founder within an organization or entity.
        /// </summary>
        /// <remarks>This enumeration value is used to identify individuals who have established or
        /// co-established an organization, company, or similar entity. It is typically used in contexts where the
        /// distinction of being a founder is relevant.</remarks>
        [Description("Founder")] Founder,

        /// <summary>
        /// Represents the role of an owner or founder within an organization.
        /// </summary>
        [Description("Founder/Owner")] OwnerFounder,

        /// <summary>
        /// Represents the owner of an entity or resource.
        /// </summary>
        [Description("Owner")] Owner,

        /// <summary>
        /// Represents a project manager role within the organization.
        /// </summary>
        /// <remarks>This enumeration value is used to identify individuals who are responsible for
        /// overseeing and managing projects.</remarks>
        [Description("Project Manager")] ProjectManager,

        /// <summary>
        /// Represents a consultant role within the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify users who are consultants.</remarks>
        [Description("Travel Consultant")] Consultant,

        /// <summary>
        /// Represents a senior consultant role within the organization.
        /// </summary>
        [Description("Senior Consultant")] SeniorConsultant,

        /// <summary>
        /// Represents a coordinator role within the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify and manage entities that act as
        /// coordinators.</remarks>
        [Description("Travel Co-ordinator")] Coordinator,

        /// <summary>
        /// Represents a tour guide role within the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify and manage users who are assigned the role
        /// of a tour guide.</remarks>
        [Description("Tour Guide")] TourGuide,

        /// <summary>
        /// Represents the accounts category in the system.
        /// </summary>
        [Description("Accounts")] Accounts,

        /// <summary>
        /// Represents a sales manager role within the organization.
        /// </summary>
        /// <remarks>This enumeration value is used to identify and manage permissions and
        /// responsibilities specific to a sales manager in the system.</remarks>
        [Description("Sales Manager")] SalesManager,

        /// <summary>
        /// Represents a tour director responsible for managing and overseeing tours.
        /// </summary>
        /// <remarks>This class is typically used in the context of organizing and conducting tours. It
        /// may include responsibilities such as planning itineraries, coordinating with tour guides, and ensuring
        /// customer satisfaction.</remarks>
        [Description("Tour Director")] TourDirector,

        /// <summary>
        /// Represents the Sales Director for North America.
        /// </summary>
        /// <remarks>This enumeration value is used to identify the role of the Sales Director responsible
        /// for overseeing sales operations in the North American region.</remarks>
        [Description("Sales Director North America")] SalesDirectorNorthAmerica,

        /// <summary>
        /// Represents the role of a Sales and Key Account Manager.
        /// </summary>
        /// <remarks>This role typically involves managing key client accounts, driving sales, and
        /// building strong customer relationships.</remarks>
        [Description("Sales & Key Account Manager")] SalesKeyAccountManager
    }
}
