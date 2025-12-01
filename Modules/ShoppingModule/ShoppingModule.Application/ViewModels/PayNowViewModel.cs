using System.ComponentModel.DataAnnotations;

namespace ShoppingModule.Application.ViewModels
{
    /// <summary>
    /// Represents the data model for a "Pay Now" checkout process, containing customer, shipping,  and payment details
    /// required to complete a transaction.
    /// </summary>
    /// <remarks>This view model is designed to capture all necessary information for processing a payment, 
    /// including customer details, shipping address, order summary, and payment preferences.  It supports scenarios
    /// where the shipping address differs from the billing address.</remarks>
    public class PayNowViewModel
    {
        private string _shippingAddressLine1;
        private string _shippingSuburb;
        private string _shippingCity;
        private string _shippingPostalCode;
        private string _shippingProvice;
        private string _shippingCountry;

        /// <summary>
        /// Gets or sets the checkout method used for processing transactions.
        /// </summary>
        public string? CheckoutMethod { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        [Required] public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname of the individual. This property is required.
        /// </summary>
        [Required] public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required, EmailAddress] public string Email { get; set; }

        /// <summary>
        /// Gets or sets the cell value, which is a required string.
        /// </summary>
        [Required] public string Cell { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the first line of the address. This is typically used to specify the street address or primary
        /// location information.
        /// </summary>
        [Required] public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the suburb associated with the address.
        /// </summary>
        [Required] public string Suburb { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        [Required] public string City { get; set; }

        /// <summary>
        /// Gets or sets the postal code associated with the address.
        /// </summary>
        [Required] public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the province associated with the entity.
        /// </summary>
        [Required] public string Province { get; set; }

        /// <summary>
        /// Gets or sets the country associated with the entity.
        /// </summary>
        [Required] public string Country { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the shipping address is different from the billing address.
        /// </summary>
        public bool DifferentShippingAddress { get; set; } = false;

        /// <summary>
        /// Gets or sets the first line of the shipping address.
        /// </summary>
        public string ShippingAddressLine1
        {
            get
            {
                if (DifferentShippingAddress)
                    return _shippingAddressLine1;
                return AddressLine1;
            }
            set => _shippingAddressLine1 = value;
        }

        /// <summary>
        /// Gets or sets the suburb associated with the shipping address.
        /// </summary>
        public string ShippingSuburb
        {
            get
            {
                if (DifferentShippingAddress)
                    return _shippingSuburb;
                return Suburb;
            }
            set => _shippingAddressLine1 = value;
        }

        /// <summary>
        /// Gets or sets the city associated with the shipping address.
        /// </summary>
        public string ShippingCity
        {
            get
            {
                if (DifferentShippingAddress)
                    return _shippingCity;
                return City;
            }
            set => _shippingAddressLine1 = value;
        }

        /// <summary>
        /// Gets or sets the postal code for the shipping address.
        /// </summary>
        public string ShippingPostalCode
        {
            get
            {
                if (DifferentShippingAddress)
                    return _shippingPostalCode;
                return PostalCode;
            }
            set => _shippingAddressLine1 = value;
        }

        /// <summary>
        /// Gets or sets the province associated with the shipping address.
        /// </summary>
        public string ShippingProvince
        {
            get
            {
                if (DifferentShippingAddress)
                    return _shippingProvice;
                return Province;
            }
            set => _shippingAddressLine1 = value;
        }

        /// <summary>
        /// Gets or sets the shipping country for the order.
        /// </summary>
        public string ShippingCountry
        {
            get
            {
                if (DifferentShippingAddress)
                    return _shippingCountry;
                return Country;
            }
            set => _shippingAddressLine1 = value;
        }

        //public DeliveryMethod DeliveryMethod { get; set; }

        //public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the voucher code associated with the transaction.
        /// </summary>
        public string? VoucherCode { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the sales order.
        /// </summary>
        public string SalesOrderId { get; set; }

        /// <summary>
        /// Gets or sets the subtotal amount for the current transaction.
        /// </summary>
        public double SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the shipping rate for the current transaction.
        /// </summary>
        public double ShippingRate { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage to be applied to the total price.
        /// </summary>
        public double Discount { get; set; }

        /// <summary>
        /// Gets or sets the value-added tax (VAT) percentage.
        /// </summary>
        public double Vat { get; set; }

        /// <summary>
        /// Gets or sets the total value represented as a double.
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        /// Gets or sets the comments associated with this instance.
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has agreed to the terms and conditions.
        /// </summary>
        public bool AgreeToTerms { get; set; } = true;

        /// <summary>
        /// Gets or sets the name of the payment gateway used for processing transactions.
        /// </summary>
        public string? PaymentGateway { get; set; }
    }
}
