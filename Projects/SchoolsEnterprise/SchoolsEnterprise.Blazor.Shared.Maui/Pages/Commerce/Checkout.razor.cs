using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using PayFast;
using ShoppingModule.Application.ViewModels;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Enums;
using System.Globalization;
using System.Security.Claims;
using ConectOne.Application.ViewModels;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using ProductsModule.Domain.Enums;
using ShoppingModule.Blazor.Components;
using ShoppingModule.Domain.Interfaces;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Commerce
{
    /// <summary>
    /// Represents the checkout process in the application, including user authentication and shopping cart state.
    /// </summary>
    /// <remarks>This class is a Blazor component that manages the checkout workflow. It relies on cascading
    /// parameters  to access the current shopping cart and authentication state. Override lifecycle methods, such as 
    /// <see cref="OnInitializedAsync"/>, to customize the behavior during initialization.</remarks>
    public partial class Checkout
    {
        private CheckoutViewModel CheckoutModel = new CheckoutViewModel();
        private PayFastSettings? _payFastSettings;
        private readonly Dictionary<string, string> _payFastFormFields = new(StringComparer.OrdinalIgnoreCase);
        private string? _payFastProcessUrl;
        private ElementReference PayFastForm;
        private bool _shouldSubmitPayFastForm;
        private ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity());

        /// <summary>
        /// Gets the collection of key-value pairs representing the form fields required for a PayFast payment request.
        /// </summary>
        protected IReadOnlyDictionary<string, string> PayFastFormFields => _payFastFormFields;

        /// <summary>
        /// Gets the URL used to initiate a payment process with PayFast.
        /// </summary>
        protected string? PayFastProcessUrl => _payFastProcessUrl;

        #region Injections and Parameters

        /// <summary>
        /// Gets or sets the current shopping cart state for the user.
        /// </summary>
        [CascadingParameter] public CartStateProvider ShoppingCart { get; set; }

        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage sales orders.
        /// </summary>
        [Inject] public ISalesOrderService SalesOrderService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// configuration values such as connection strings, application settings, and environment variables.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage sales order detail operations.
        /// </summary>
        [Inject] public ISalesOrderDetailService SalesOrderDetailService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime abstraction used to invoke JavaScript functions from .NET code.
        /// </summary>
        /// <remarks>This property is typically populated by the dependency injection framework in Blazor
        /// applications. Ensure that the property is set before attempting to use JavaScript interop.</remarks>
        [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Processes a payment for the current shopping cart, creating a sales order and handling payment details based
        /// on the selected payment method.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet">
        /// <item><description>Generates a new sales order based on the current user's shopping
        /// cart.</description></item> <item><description>Creates sales order details for each item in the shopping
        /// cart.</description></item> <item><description>If the payment method is <see cref="PaymentMethod.Card"/>,
        /// initiates a payment request with the payment gateway.</description></item> <item><description>Updates the
        /// sales order status and records the payment details.</description></item> </list> If the payment method is
        /// not <see cref="PaymentMethod.Card"/>, the method prepares for alternative payment processing.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ProcessPaymentAsync()
        {
            if (ShoppingCart?.ShoppingCart?.CartItems is null || !ShoppingCart.ShoppingCart.CartItems.Any())
            {
                SnackBar.Add("Your shopping cart is empty.", Severity.Warning);
                return;
            }

            var cart = ShoppingCart.ShoppingCart;
            var orderId = Guid.NewGuid().ToString();

            var order = new SalesOrderDto()
            {
                SalesOrderDate = DateTime.Now,
                SalesOrderId = orderId,
                UserId = _user.GetUserId(),
                Notes = string.Empty,
                DeliveryMethod = DeliveryMethod.Flat,
                ShippingTotal = cart.Shipping,
                Discount = 0,
                CouponDiscount = cart.CouponDiscount,
                ShippingAddress = CheckoutModel.ShippingAddress?.ToDto(),
                BillingAddress = CheckoutModel.ShippingAddress?.ToDto(),
                ItemCount = cart.ItemCount,
                SubTotal = cart.SubTotalExcl,
                Vat = cart.TotalVat,
                TotalIncl = cart.TotalDue,
                Total = cart.TotalDue,
                TotalAmmountPaid = 0
            };

            var result = await SalesOrderService.CreateSalesOrderAsync(order);
            if (!result.Succeeded)
            {
                SnackBar.AddErrors(result.Messages);
                return;
            }

            var vat = Configuration.GetSection("CompanyConfiguration").GetValue<int>("VatRate");

            foreach (var detail in cart.CartItems)
            {
                var neworder = new SalesOrderDetailDto
                {
                    SalesOrderId = orderId,
                    SalesOrderDetailId = Guid.NewGuid().ToString(),
                    Processed = false,
                    Qty = detail.Qty,
                    ProductId = detail.ProductId,
                    ProductName = detail.ProductName,
                    Description = detail.ShortDescription,
                    ThumbnailUrl = detail.ThumbnailUrl,
                    PriceExcl = detail.PriceExcl,
                    PriceVat = detail.PriceExcl * vat / 100,
                    PriceIncl = detail.PriceIncl,
                    MetaData = detail.Metadata.ToList()
                };

                var salesOrderDetailAsync = await SalesOrderDetailService.CreateSalesOrderDetailAsync(neworder);
                if (!salesOrderDetailAsync.Succeeded)
                {
                    SnackBar.AddErrors(salesOrderDetailAsync.Messages);
                }
            }

            if (CheckoutModel.PaymentMethod == PaymentMethod.Card)
            {
                var submitted = await TrySubmitPayFastPaymentAsync(orderId);

                if (submitted)
                {
                    await ShoppingCart.EmptyCartAsync();
                }
            }
            else
            {
                SnackBar.Add("Electronic transfer instructions will be provided separately.", Severity.Info);
                await ShoppingCart.EmptyCartAsync();
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Asynchronously initializes the component and loads user-specific data required for the checkout process.
        /// </summary>
        /// <remarks>This method retrieves PayFast configuration settings and the authenticated user's
        /// information. It populates the <see cref="CheckoutModel"/> with the user's details, including their company
        /// name, contact information, and shipping address. Any errors encountered during the data retrieval process
        /// are displayed using the provided <see cref="SnackBar"/>.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            _payFastSettings = Configuration.GetSection("PayFast").Get<PayFastSettings>();
            _payFastProcessUrl = _payFastSettings?.ProcessUrl;

            var authState = await AuthenticationStateTask;
            _user = authState.User;

            var userInfoResult = await UserService.GetUserInfoAsync(_user.GetUserId());
            userInfoResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                var userInfo = userInfoResult.Data;
                CheckoutModel.CompanyName = $"{userInfo.CompanyName}";
                CheckoutModel.FirstName = $"{userInfo.FirstName}";
                CheckoutModel.LastName = $"{userInfo.LastName}";
                CheckoutModel.PhoneNr = $"{userInfo.PhoneNr}";
                CheckoutModel.Email = userInfo.EmailAddress;
                CheckoutModel.ShippingAddress = new AddressViewModel(userInfo.Address);
            });
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Attempts to submit a payment request to PayFast based on the current shopping cart and checkout details.
        /// </summary>
        /// <remarks>This method validates the PayFast configuration and ensures that the shopping cart
        /// contains items before proceeding. If the validation fails or the cart is empty, the method returns <see
        /// langword="false"/> without submitting the payment. On success, the method prepares the payment request,
        /// populates the necessary fields, and triggers the submission process.</remarks>
        /// <returns><see langword="true"/> if the payment request was successfully prepared and submitted; otherwise, <see
        /// langword="false"/>.</returns>
        private async Task<bool> TrySubmitPayFastPaymentAsync(string salesOrderId)
        {
            if (!ValidatePayFastConfiguration())
            {
                return false;
            }

            if (ShoppingCart?.ShoppingCart?.CartItems is null || !ShoppingCart.ShoppingCart.CartItems.Any())
            {
                SnackBar.Add("Your shopping cart is empty.", Severity.Warning);
                return false;
            }

            var cart = ShoppingCart.ShoppingCart;
            var firstName = CheckoutModel.FirstName ?? string.Empty;
            var lastName = CheckoutModel.LastName ?? string.Empty;
            var email = CheckoutModel.Email ?? string.Empty;
            var phone = CheckoutModel.PhoneNr ?? string.Empty;

            var request = CreatePayFastRequest();

            request.name_first = firstName;
            request.name_last = lastName;
            request.email_address = email;
            request.cell_number = phone;
            request.m_payment_id = !string.IsNullOrWhiteSpace(cart.ShoppingCartId) ? cart.ShoppingCartId : Guid.NewGuid().ToString("N");
            request.amount = Math.Round(cart.TotalDue, 2, MidpointRounding.AwayFromZero);

            var itemCount = cart.CartItems?.Count ?? 0;
            var description = cart.CartItems is null
                ? string.Empty
                : string.Join(", ", cart.CartItems.Select(i => i.ProductName)).TruncateLongString(255);

            request.item_name = itemCount switch
            {
                0 => "Shopping Cart",
                1 => cart.CartItems!.First().ProductName.TruncateLongString(100),
                _ => $"{itemCount} items"
            };
            request.item_description = description;
            request.custom_str1 = salesOrderId;
            request.custom_str2 = cart.ShoppingCartId;

            PopulatePayFastFormFields(request);
            _shouldSubmitPayFastForm = true;
            await InvokeAsync(StateHasChanged);

            return true;
        }

        /// <summary>
        /// Invoked after the component has rendered. Executes additional logic if this is the first render or if a
        /// specific condition is met.
        /// </summary>
        /// <remarks>If the <c>_shouldSubmitPayFastForm</c> flag is set to <see langword="true"/>, this
        /// method attempts to invoke a JavaScript function named <c>submitPayFastForm</c> using the <see
        /// cref="JSRuntime"/> to submit the <c>PayFastForm</c>. Any exceptions encountered during this process are
        /// logged to the console.</remarks>
        /// <param name="firstRender">A boolean value indicating whether this is the first time the component has been rendered.</param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_shouldSubmitPayFastForm)
            {
                _shouldSubmitPayFastForm = false;

                try
                {
                    await JSRuntime.InvokeVoidAsync("submitPayFastForm", PayFastForm);
                }
                catch (Exception exception)
                {
                    Console.Error.WriteLine($"Failed to submit PayFast form: {exception}");
                }
            }
        }

        /// <summary>
        /// Creates and initializes a new <see cref="PayFastRequest"/> instance with merchant and URL configuration
        /// settings.
        /// </summary>
        /// <remarks>The method retrieves configuration values for the PayFast merchant ID, merchant key,
        /// return URL, cancel URL, and notify URL from the application's configuration. If a passphrase is specified in
        /// the PayFast settings, it is used to initialize the <see cref="PayFastRequest"/>; otherwise, a default
        /// instance is created.</remarks>
        /// <returns>A <see cref="PayFastRequest"/> instance populated with the merchant and URL configuration settings.</returns>
        private PayFastRequest CreatePayFastRequest()
        {
            var request = string.IsNullOrWhiteSpace(_payFastSettings?.PassPhrase)
                ? new PayFastRequest()
                : new PayFastRequest(_payFastSettings!.PassPhrase);

            request.merchant_id = Configuration["PayFast:MerchantId"];
            request.merchant_key = Configuration["PayFast:MerchantKey"];
            request.return_url = _payFastSettings?.ReturnUrl ?? Configuration["PayFast:ReturnUrl"];
            request.cancel_url = Configuration["PayFast:CancelUrl"];
            request.notify_url =Configuration["PayFast:NotifyUrl"];
            request.custom_str5 = _payFastSettings?.RedirectUrl ?? _payFastSettings?.ReturnUrl ?? Configuration["PayFast:ReturnUrl"];

            return request;
        }

        /// <summary>
        /// Populates the PayFast form fields with the data provided in the <see cref="PayFastRequest"/> object.
        /// </summary>
        /// <remarks>This method clears any existing form fields before populating them with the values
        /// from the <paramref name="request"/>. Required fields are always added, while optional fields are added only
        /// if they contain non-null and non-whitespace values. The method also ensures that numeric and boolean values
        /// are formatted appropriately for the PayFast API.</remarks>
        /// <param name="request">The <see cref="PayFastRequest"/> containing the data to populate the form fields. This includes required
        /// fields such as merchant details, payment information, and optional fields for additional metadata.</param>
        private void PopulatePayFastFormFields(PayFastRequest request)
        {
            _payFastFormFields.Clear();

            void AddField(string key, string? value)
            {
                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                {
                    _payFastFormFields[key] = value;
                }
            }

            void AddOptionalField(string key, string? value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _payFastFormFields[key] = value;
                }
            }

            AddField("merchant_id", request.merchant_id);
            AddField("merchant_key", request.merchant_key);
            AddField("return_url", request.return_url);
            AddField("cancel_url", request.cancel_url);
            AddField("notify_url", request.notify_url);
            AddField("m_payment_id", request.m_payment_id);
            AddField("amount", request.amount.ToString("F2", CultureInfo.InvariantCulture));
            AddField("item_name", request.item_name);
            AddOptionalField("item_description", request.item_description);
            AddOptionalField("name_first", request.name_first);
            AddOptionalField("name_last", request.name_last);
            AddOptionalField("email_address", request.email_address);
            AddOptionalField("cell_number", request.cell_number);
            AddOptionalField("custom_str1", request.custom_str1);
            AddOptionalField("custom_str2", request.custom_str2);
            AddOptionalField("custom_str3", request.custom_str3);
            AddOptionalField("custom_str4", request.custom_str4);
            AddOptionalField("custom_str5", request.custom_str5);
            AddOptionalField("custom_int1", request.custom_int1?.ToString(CultureInfo.InvariantCulture));
            AddOptionalField("custom_int2", request.custom_int2?.ToString(CultureInfo.InvariantCulture));
            AddOptionalField("custom_int3", request.custom_int3?.ToString(CultureInfo.InvariantCulture));
            AddOptionalField("custom_int4", request.custom_int4?.ToString(CultureInfo.InvariantCulture));
            AddOptionalField("custom_int5", request.custom_int5?.ToString(CultureInfo.InvariantCulture));

            if (request.email_confirmation.HasValue)
            {
                _payFastFormFields["email_confirmation"] = request.email_confirmation.Value ? "1" : "0";
            }

            AddOptionalField("confirmation_address", request.confirmation_address);

            AddField("signature", request.signature);
        }

        /// <summary>
        /// Validates the PayFast configuration settings to ensure all required fields are properly configured.
        /// </summary>
        /// <remarks>This method checks whether the PayFast settings, including the merchant details and
        /// required URLs, are provided and non-empty. If any required field is missing or invalid, an error message is
        /// displayed via the SnackBar, and the method returns <see langword="false"/>.</remarks>
        /// <returns><see langword="true"/> if the PayFast configuration is valid; otherwise, <see langword="false"/>.</returns>
        private bool ValidatePayFastConfiguration()
        {
            if (_payFastSettings is null)
            {
                SnackBar.Add("PayFast configuration is missing.", Severity.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(_payFastSettings.MerchantId) || string.IsNullOrWhiteSpace(_payFastSettings.MerchantKey))
            {
                SnackBar.Add("PayFast merchant details are not configured.", Severity.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(_payFastSettings.ProcessUrl) || string.IsNullOrWhiteSpace(_payFastSettings.ReturnUrl) || string.IsNullOrWhiteSpace(_payFastSettings.CancelUrl) || string.IsNullOrWhiteSpace(_payFastSettings.NotifyUrl))
            {
                SnackBar.Add("PayFast URLs are not fully configured.", Severity.Error);
                return false;
            }

            _payFastProcessUrl = _payFastSettings.ProcessUrl;

            return true;
        }

        #endregion
    }
}
