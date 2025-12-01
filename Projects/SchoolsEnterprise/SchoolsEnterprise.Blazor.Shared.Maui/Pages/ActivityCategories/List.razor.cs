using Microsoft.AspNetCore.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.ActivityCategories
{
    /// <summary>
    /// Represents a component that displays a list of grocery product items and provides navigation to product detail
    /// pages.
    /// </summary>
    /// <remarks>This component initializes a predefined set of grocery products and exposes functionality to
    /// navigate to a specific product detail view. It is intended for use within a Blazor application and relies on
    /// dependency injection for navigation services.</remarks>
    public partial class List
    {
        public bool _loaded;
        private List<ProductItemInfo> _productItemList;
        Func<Menu, string> converter = p => p?.Name;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework to enable programmatic
        /// navigation and to access the current URI. It should not be set manually outside of dependency injection
        /// scenarios.</remarks>
        [Inject] private NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Asynchronously initializes the component and loads the initial list of product items.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during the component's initialization
        /// phase. Override this method to perform asynchronous operations such as data loading before the component is
        /// rendered.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1500);
            _productItemList = new List<ProductItemInfo>
        {
            new ProductItemInfo
            {
                Id = 1,
                Image = "/images/grocery/sm/1.png",
                ItemName = "Tomatoes",
                Descriptions = "Imported from Asia",
                Price = "$15.99",
                Discount = "5% Off"
            },
            new ProductItemInfo
            {
                Id = 2,
                Image = "/images/grocery/sm/2.png",
                ItemName = "Strawberries",
                Descriptions = "Local Farm Product",
                Price = "$19.99",
                Discount = "5% Off"
            },
            new ProductItemInfo
            {
                Id = 3,
                Image = "/images/grocery/sm/3.png",
                ItemName = "EU Onions",
                Descriptions = "Imported from Europe",
                Price = "$15.50",
                Discount = "15% Off"
            },
            new ProductItemInfo
            {
                Id = 4,
                Image = "/images/grocery/sm/4.png",
                ItemName = "Iceberg Salad",
                Descriptions = "Imported from Asia",
                Price = "$99.99",
                Discount = "5% Off"
            },
            new ProductItemInfo
            {
                Id = 5,
                Image = "/images/grocery/sm/5.png",
                ItemName = "Green Apple",
                Descriptions = "Local Farm Product",
                Price = "$9,99",
                Discount = "10% Off"
            },
            new ProductItemInfo
            {
                Id =6,
                Image = "/images/grocery/sm/6.png",
                ItemName = "Potatoes",
                Descriptions = "Imported from Asia",
                Price = "$11.99",
                Discount = "35% Off"
            }
        };

            _loaded = true;
        }

        /// <summary>
        /// Navigates the application to the product detail page for the specified grocery product.
        /// </summary>
        /// <remarks>This method initiates a client-side navigation to the product detail view. It is
        /// typically used in response to user actions such as selecting a product from a list.</remarks>
        public void NavigateToProductDetail()
        {
            NavigationManager.NavigateTo("/grocery/productdetail-2");
        }

        /// <summary>
        /// Represents a menu with a display name.
        /// </summary>
        public class Menu
        {
            public string Name { get; set; }
        }

        /// <summary>
        /// Represents information about a product item, including its identifier, image, name, description, price, and
        /// discount details.
        /// </summary>
        public class ProductItemInfo
        {
            public int Id { get; set; }
            public string Image { get; set; }
            public string ItemName { get; set; }
            public string Descriptions { get; set; }
            public string Price { get; set; }
            public string Discount { get; set; }
        }
    }
}
