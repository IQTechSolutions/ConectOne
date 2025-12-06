using Microsoft.AspNetCore.Components;

namespace KiburiOnline.Blazor.Shared.Layouts
{
    /// <summary>
    /// Represents a menu component that applies specific styles based on the current page name.
    /// </summary>
    /// <remarks>The <see cref="InnerMenu"/> class is designed to dynamically adjust its visual style
    /// according to the specified page name. It supports various regional styles, each associated with a distinct color
    /// scheme. This class is typically used in web applications where visual differentiation of pages is
    /// required.</remarks>
    public partial class InnerMenu
    {
        private string _southAfricaStyle = "";
        private string _botswanaStyle = "";
        private string _zambiaStyle = "";
        private string _namibiaStyle = "";
        private string _tanzaniaStyle = "";
        private string _kenyaStyle = "";
        private string _rwandaStyle = "";

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        [Parameter] public string PageName { get; set; }

        /// <summary>
        /// Asynchronously initializes the component and applies specific styles based on the current page name.
        /// </summary>
        /// <remarks>This method sets a distinct color style for each supported page name, enhancing the
        /// visual representation of the component. It then calls the base implementation to ensure any additional
        /// initialization logic is executed.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            switch (PageName)
            {
                case "botswana":
                    _botswanaStyle = "color: #F79340;";
                    break;
                case "south-africa":
                    _southAfricaStyle = "color: #F79340;";
                    break;
                case "zambia":
                    _zambiaStyle = "color: #F79340;";
                    break;
                case "namibia":
                    _namibiaStyle = "color: #F79340;";
                    break;
                case "tanzania":
                    _tanzaniaStyle = "color: #F79340;";
                    break;
                case "kenya":
                    _kenyaStyle = "color: #F79340;";
                    break;
                case "rwanda":
                    _rwandaStyle = "color: #F79340;";
                    break;
            }

            await base.OnInitializedAsync();
        }
    }
}
