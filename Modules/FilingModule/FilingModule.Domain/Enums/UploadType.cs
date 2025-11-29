using System.ComponentModel;

namespace FilingModule.Domain.Enums
{
    /// <summary>
    /// Represents the types of uploads that can be processed or categorized within the system.
    /// </summary>
    /// <remarks>This enumeration defines various upload types, such as images, documents, and other media, 
    /// that can be used in different contexts, such as galleries, profiles, or banners. Each value  is associated with
    /// a specific purpose or category, which can be used to organize or filter  uploaded content.</remarks>
    public enum UploadType
    {
        /// <summary>
        /// Represents an image in the gallery.
        /// </summary>
        [Description("Gallery Image")]
        Image = 0,
        
        /// <summary>
        /// Represents the "Cover" option in the enumeration.
        /// </summary>
        [Description("Cover")]
        Cover = 1,
        
        /// <summary>
        /// Represents a slider control in the user interface.
        /// </summary>
        [Description("Slider")]
        Slider = 2,
        
        /// <summary>
        /// Represents a banner element in the system.
        /// </summary>
        [Description("Banner")]
        Banner = 3,
        
        /// <summary>
        /// Represents the profile access level or type within the system.
        /// </summary>
        [Description("Profile")]
        Profile = 4,
        
        /// <summary>
        /// Represents a store front location in the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify a store front in various operations.</remarks>
        [Description("Store Front")]
        StoreFront = 5,
        
        /// <summary>
        /// Represents a mockup value used for demonstration or testing purposes.
        /// </summary>
        [Description("Mockup")]
        Mockup = 6,
        
        /// <summary>
        /// Represents an icon element in the enumeration.
        /// </summary>
        /// <remarks>This value is typically used to specify or identify an icon in a given context.</remarks>
        [Description("Icon")]
        Icon = 7,
        
        /// <summary>
        /// Represents a proof of address document type.
        /// </summary>
        /// <remarks>This value is used to indicate that the document provided serves as proof of the user's address.</remarks>
        [Description("Proof Of Address")]
        ProofOfAddress = 8,
        
        /// <summary>
        /// Represents an identity document type.
        /// </summary>
        /// <remarks>This enumeration value is used to specify an identity document, such as a passport or national ID.</remarks>
        [Description("Identity Document")]
        IdDocument = 9,
        
        /// <summary>
        /// Represents the registration documents category.
        /// </summary>
        /// <remarks>This enumeration value is used to identify and categorize registration documents.</remarks>
        [Description("Registration Documents")]
        RegistrationDocuments = 10,
        
        /// <summary>
        /// Represents the BBE Documents category.
        /// </summary>
        /// <remarks>This enumeration value is used to identify and categorize BBE Documents.</remarks>
        [Description("BBE Documents")]
        BbeDocuments = 11,
        
        /// <summary>
        /// Represents a thumbnail image type.
        /// </summary>
        /// <remarks>This enumeration value is used to indicate that the associated item is a thumbnail.</remarks>
        [Description("Thumbnail")]
        Thumbnail = 12,
        
        /// <summary>
        /// Represents a document type with a value of 13.
        /// </summary>
        [Description("Document")]
        Document = 13,
        
        /// <summary>
        /// Represents the logo icon in the enumeration.
        /// </summary>
        /// <remarks>This value is typically used to identify or represent a logo in a collection of icons.</remarks>
        [Description("Logo")]
        Logo = 14,
        
        /// <summary>
        /// Represents the map view mode in the application.
        /// </summary>
        [Description("Map")]
        Map = 15,

        /// <summary>
        /// Represents a summary slider control.
        /// </summary>
        /// <remarks>This enumeration value is used to identify a summary slider component in the application.</remarks>
        [Description("Summary Slider")]
        SummarySlider = 16,

        /// <summary>
        /// Represents an extension slider control.
        /// </summary>
        /// <remarks>This enumeration value is used to identify an extension slider in the context of the application.</remarks>
        [Description("Extension Slider")]
        ExtensionSlider = 17
    }
}
