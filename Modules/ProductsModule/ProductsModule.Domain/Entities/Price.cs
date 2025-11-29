using ConectOne.Domain.Entities;

namespace ProductsModule.Domain.Entities
{
    /// <summary>
    /// Represents pricing information for a product, including cost, discounts, profit margins, VAT, and reward points.
    /// </summary>
    /// <remarks>This class provides methods to calculate various price-related values, such as the price with
    /// profit,  price with discounts, VAT, and the final inclusive price. It also includes properties for managing 
    /// discount periods, reward points, and VAT applicability.</remarks>
    public class Price : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the date and time when the discount period ends.
        /// </summary>
        public DateTime? DiscountEndDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the cost excluding any applicable taxes or additional charges.
        /// </summary>
        public double CostExcl { get; set; }

        /// <summary>
        /// Gets or sets the shipping amount for an order.
        /// </summary>
        public double ShippingAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the price is available only upon contacting the seller.
        /// </summary>
        public bool ContactForPrice { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the item is subject to value-added tax (VAT).
        /// </summary>
        public bool Vatable { get; set; } = true;

        /// <summary>
        /// Gets or sets the discount percentage to be applied to the total price.
        /// </summary>
        public double DiscountPercentage { get; set; }     
        
        /// <summary>
        /// Gets or sets the profit percentage as a double value.
        /// </summary>
        public double SellingPrice{ get; set; }

        /// <summary>
        /// Gets or sets the number of reward points associated with the user.
        /// </summary>
        public int RewardPoints { get; set; }

        /// <summary>
        /// Gets the monetary value of the reward points.
        /// </summary>
        public double RewardPointsValue => RewardPoints * 10;
    }
}
