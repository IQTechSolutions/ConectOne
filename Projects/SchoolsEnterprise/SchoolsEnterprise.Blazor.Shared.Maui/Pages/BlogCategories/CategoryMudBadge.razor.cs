
using GroupingModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.BlogCategories
{
    /// <summary>
    /// The CategoryMudBadge component is responsible for displaying a category badge with an icon and unread notifications count.
    /// </summary>
    public partial class CategoryMudBadge
    {
        #region Parameters

        /// <summary>
        /// The category data transfer object (DTO) representing the category to display.
        /// </summary>
        [Parameter] public CategoryDto Category { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the icon URL for the specified category name.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The URL of the icon image.</returns>
        private string GetCategoryIcon(string name)
        {
            if (name.ToLower().Contains("net"))
                return "/images/static/icons2/EVERNET.svg";
            if (name.ToLower().Contains("news feed"))
                return "/images/static/icons2/mydocuments.svg";
            if (name.ToLower().Contains("focus"))
                return "/images/static/icons2/everfocusicon.svg";
            if (name.ToLower().Contains("circular"))
                return "/images/static/icons2/EVERCIRCULAR.svg";
            if (name.ToLower().Contains("talk"))
                return "/images/static/icons2/talkicon.svg";
            return "/images/static/icons2/homeicon.svg";
        }

        #endregion
    }
}

