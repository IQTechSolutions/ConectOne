namespace ProductsModule.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for calculating pricing-related values, such as discounts, profits, VAT, and
    /// formatted price strings.
    /// </summary>
    /// <remarks>This static class includes methods for performing common pricing calculations, such as
    /// determining the price with a discount or profit margin,  calculating VAT-inclusive or VAT-exclusive prices, and
    /// formatting prices as currency strings.  These methods are designed to simplify pricing logic and can be used
    /// directly on numeric values.</remarks>
    public static class PricingExtensions
    {        
        /// <summary>
        /// Calculates the discount value based on the given price and discount percentage.
        /// </summary>
        /// <param name="priceExcl">The original price, excluding any discounts. Must be a non-negative value.</param>
        /// <param name="discountPercentage">The discount percentage to apply. Must be a value between 0 and 100, inclusive.</param>
        /// <returns>The calculated discount value. Returns 0 if <paramref name="discountPercentage"/> is 0.</returns>
        public static double PercentageValue(double priceExcl, double discountPercentage)
        {
            if(discountPercentage == 0)
            {
                return 0;
            }
            var result = priceExcl * (discountPercentage/100);
            return result;
        }

        /// <summary>
        /// Calculates the price of an item after applying a specified profit percentage.
        /// </summary>
        /// <param name="value">The base cost of the item.</param>
        /// <param name="profitpercentage">The profit percentage to be applied. Must be greater than or equal to 0.</param>
        /// <returns>The price of the item after adding the specified profit percentage.</returns>
        public static double PriceWithProfit(this double value, double profitpercentage)
        {
            return value / (1-(profitpercentage/100));
        }

        public static double PriceWithDiscount(this double value, double discountPercentage, DateTime? discountEndDate)
        {
            if (discountEndDate > DateTime.Now && discountPercentage > 0)
            {
                return value/ (1 + (discountPercentage/100));
            }
            return value;
        }

        /// <summary>
        /// Rounds the specified value to a precision determined by its magnitude.
        /// </summary>
        /// <param name="value">The value to be rounded.</param>
        /// <returns>The rounded value, where the precision is determined by the magnitude of the input: <list type="bullet">
        /// <item><description>If <paramref name="value"/> is greater than 10,000, it is rounded to the nearest
        /// 100.</description></item> <item><description>If <paramref name="value"/> is greater than 1,000, it is
        /// rounded to the nearest 10.</description></item> <item><description>If <paramref name="value"/> is greater
        /// than 100, it is rounded to the nearest whole number.</description></item> <item><description>If <paramref
        /// name="value"/> is greater than 10, it is rounded to one decimal place.</description></item>
        /// <item><description>Otherwise, it is rounded to two decimal places.</description></item> </list></returns>
        public static double PriceExcl(this double value)
        {
            if (value > 10000)
                return (int)(Math.Ceiling(value/100d) * 100);

            if (value > 1000)
                return (int)(Math.Ceiling(value/10d) * 10);

            if (value > 100)
                return Math.Ceiling(value);

            if (value > 10)
                return Math.Round(value, 1);

            return Math.Round(value, 2);
        }

        /// <summary>
        /// Calculates the VAT amount for a given value based on its VAT status and rate.
        /// </summary>
        /// <param name="value">The base value to calculate VAT for.</param>
        /// <param name="vatable">A value indicating whether VAT applies to the base value. <see langword="true"/> if VAT applies; otherwise,
        /// <see langword="false"/>.</param>
        /// <param name="vatRate">The VAT rate to apply, expressed as a decimal (e.g., 0.2 for 20%).</param>
        /// <returns>The VAT amount for the specified value. Returns 0 if <paramref name="vatable"/> is <see langword="false"/>.</returns>
        public static double PriceVat(this double value, bool vatable, double vatRate)
        {
            if(vatable)
                return value - (value * vatRate) / (100 + vatRate);
            return 0;
        }

        /// <summary>
        /// Calculates the price including VAT (Value-Added Tax) based on the specified VAT rate.
        /// </summary>
        /// <remarks>This method assumes that the input price is exclusive of VAT. If VAT is applicable, 
        /// the method calculates the price by applying the specified VAT rate. If VAT is not  applicable, the method
        /// simply returns the original price rounded to two decimal places.</remarks>
        /// <param name="value">The original price of the item.</param>
        /// <param name="vatable">A value indicating whether the item is subject to VAT.  <see langword="true"/> if VAT should be applied;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="vatRate">The VAT rate as a percentage (e.g., 20 for 20%). Must be a non-negative value.</param>
        /// <returns>The price including VAT if <paramref name="vatable"/> is <see langword="true"/>;  otherwise, the original
        /// price rounded to two decimal places.</returns>
        public static double PriceIncl(this double value, bool vatable, double vatRate)
        {
            if (vatable)
                return PriceExcl(value) * (1 + (vatRate/100));

            return Math.Round(PriceExcl(value), 2);
        }

        /// <summary>
        /// Converts a monetary value to a formatted currency string or returns a "Contact For Price" message.
        /// </summary>
        /// <param name="ammount">The monetary value to format as a currency string.</param>
        /// <param name="contactForPrice">A boolean value indicating whether to return a "Contact For Price" message instead of formatting the
        /// monetary value. If <see langword="true"/>, the method returns "Contact For Price"; otherwise, it formats the
        /// <paramref name="ammount"/> as a currency string.</param>
        /// <returns>A string representing the formatted currency value of <paramref name="ammount"/>, rounded to two decimal
        /// places,  or "Contact For Price" if <paramref name="contactForPrice"/> is <see langword="true"/>.</returns>
        public static string PriceCurrencyString(this double ammount, bool contactForPrice)
        {
            if (contactForPrice)
                return "Contact For Price";
            return Math.Round(ammount, 2).ToString("c");
        }
    }
}
