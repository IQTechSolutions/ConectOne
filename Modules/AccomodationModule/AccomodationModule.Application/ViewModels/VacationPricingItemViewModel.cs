using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for vacation pricing items, providing details such as name, description, price, and
    /// ordering.
    /// </summary>
    /// <remarks>This view model is typically used to transfer vacation pricing data between the application
    /// layers. It includes properties for identifying the vacation pricing item, associating it with a vacation, and
    /// specifying its price and display order.</remarks>
    public record VacationPricingItemViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationPricingItemViewModel"/> class.
        /// </summary>
        public VacationPricingItemViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationPricingItemViewModel"/> class using the specified
        /// vacation pricing data.
        /// </summary>
        /// <remarks>If the <paramref name="vacationPrice"/> contains a null or empty <see
        /// cref="VacationPricingItemDto.VacationPriceItemId"/>, a new unique identifier is generated for <see
        /// cref="VacationPriceItemId"/>.</remarks>
        /// <param name="vacationPrice">The vacation pricing data used to initialize the view model. Must not be null.</param>
        public VacationPricingItemViewModel(VacationPricingItemDto vacationPrice)
        {
            VacationPriceItemId = string.IsNullOrEmpty(vacationPrice.VacationPriceItemId) ? Guid.NewGuid().ToString() : vacationPrice.VacationPriceItemId;
            VacationId = vacationPrice.VacationId;
            Name = vacationPrice.Name;
            Description = vacationPrice.Description;
            Price = vacationPrice.Price;
            Selector = vacationPrice.Selector;
            Order = vacationPrice.Order;
        }

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the unique identifier for a vacation price item.
        /// </summary>
        public string VacationPriceItemId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        public string? VacationId { get; set; } 

        /// <summary>
        /// Gets or sets the unique identifier for the vacation extension.
        /// </summary>
        public string? VacationExtensionId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; set; }
        
        /// <summary>
        /// Gets or sets the CSS selector used to identify elements in the DOM.
        /// </summary>
        /// <remarks>The selector must be a valid CSS selector string. For example, it can represent an
        /// element by tag name (e.g., "div"), class name (e.g., ".my-class"), or ID (e.g., "#my-id").</remarks>
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets the order in which the item should be processed or displayed.
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of <see cref="VacationPrice"/> to a  <see
        /// cref="VacationPricingItemDto"/>.
        /// </summary>
        /// <returns>A <see cref="VacationPricingItemDto"/> that contains the data from the current instance.</returns>
        public VacationPricingItemDto ToDto()
        {
            return new VacationPricingItemDto()
            {
                VacationPriceItemId = VacationPriceItemId,
                VacationId = VacationId,
                VacationExtensionId = VacationExtensionId,
                Name = Name,
                Description = Description,
                Price = Price,
                Selector = Selector,
                Order = Order
            };
        }

        #endregion
    }
}
