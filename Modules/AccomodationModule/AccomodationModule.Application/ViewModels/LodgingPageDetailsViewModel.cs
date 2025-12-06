using System.ComponentModel;
using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for lodging page details, including metadata and page title information.
    /// </summary>
    /// <remarks>This class is designed to encapsulate the details of a lodging page, such as its title, meta
    /// keys,  and meta description, which are commonly used for SEO purposes. It also provides functionality to  update
    /// the page details.</remarks>
    public class LodgingPageDetailsViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingPageDetailsViewModel"/> class.
        /// </summary>
        public LodgingPageDetailsViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingPageDetailsViewModel"/> class using the specified
        /// lodging details.
        /// </summary>
        /// <param name="lodging">The lodging details used to populate the page title, meta keys, and meta description. Cannot be <see
        /// langword="null"/>.</param>
        public LodgingPageDetailsViewModel(LodgingDto lodging)
        {
            PageTitle = lodging.PageTitle;
            MetaKeys = lodging.MetaKeys;
            MetaDescription = lodging.MetaDescription;
        }

        #endregion

        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        [DisplayName("Page Title")] public string? PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the meta keys associated with the entity.
        /// </summary>
        [DisplayName("Meta Keys")] public string? MetaKeys { get; set; }

        /// <summary>
        /// Gets or sets the meta description for the content.
        /// </summary>
        [DisplayName("Meta Description")] public string? MetaDescription { get; set; }

        /// <summary>
        /// Updates the current instance with the values from the specified lodging page details.
        /// </summary>
        /// <remarks>This method updates the <see cref="PageTitle"/>, <see cref="MetaKeys"/>, and <see
        /// cref="MetaDescription"/>  properties of the current instance using the corresponding values from the
        /// provided <paramref name="pageDetails"/>.</remarks>
        /// <param name="pageDetails">The <see cref="LodgingPageDetailsViewModel"/> containing the updated page details. This parameter cannot be
        /// <see langword="null"/>.</param>
        public void Update(LodgingPageDetailsViewModel pageDetails)
        {
            PageTitle = pageDetails.PageTitle;
            MetaKeys = pageDetails.MetaKeys;
            MetaDescription = pageDetails.MetaDescription;
        }
    }
}
