using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using PayFast.Extensions;

namespace PayFast.Base
{
    /// <summary>
    /// Provides a base class for PayFast-related objects, offering common functionality for property serialization,
    /// passphrase management, and data formatting required for PayFast integrations.
    /// </summary>
    /// <remarks>This class is intended to be inherited by types that represent PayFast requests or responses.
    /// It includes methods for generating name-value collections of properties, formatting property values according to
    /// PayFast requirements, and handling passphrase logic. Derived classes can override methods such as
    /// GetOrderedProperties to customize property ordering or inclusion. This class is not thread-safe.</remarks>
    public class PayFastBase
    {
        protected string passPhrase;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PayFastBase class with the specified passphrase.
        /// </summary>
        /// <param name="passPhrase">The passphrase used to secure or validate PayFast transactions. Cannot be null.</param>
        public PayFastBase(string passPhrase)
        {
            this.passPhrase = passPhrase;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Sets the passphrase used for authentication or encryption operations.
        /// </summary>
        /// <param name="passPhrase">The passphrase to use. Cannot be null.</param>
        public void SetPassPhrase(string passPhrase)
        {
            this.passPhrase = passPhrase;
        }

        /// <summary>
        /// Creates a new collection containing the object's property names and their corresponding values, excluding
        /// the property named "signature".
        /// </summary>
        /// <remarks>The returned collection includes all properties except those named "signature",
        /// regardless of case. The order of the entries matches the order returned by
        /// <c>GetOrderedProperties()</c>.</remarks>
        /// <returns>A <see cref="NameValueCollection"/> containing property names and values, with the "signature" property
        /// omitted.</returns>
        public NameValueCollection GetNameValueCollection()
        {
            var collection = new NameValueCollection();

            foreach (var property in GetOrderedProperties())
            {
                if (string.Equals(property.Name, "signature", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                collection.Add(property.Name, GetPropertyValue(property));
            }

            return collection;
        }

        /// <summary>
        /// Retrieves the sequence of public properties for the current object type.
        /// </summary>
        /// <remarks>The default implementation returns properties in the order provided by <see
        /// cref="Type.GetProperties()"/>. Derived classes can override this method to customize the property order or
        /// selection.</remarks>
        /// <returns>An enumerable collection of <see cref="PropertyInfo"/> objects representing the public properties of the
        /// object's type.</returns>
        protected virtual IEnumerable<PropertyInfo> GetOrderedProperties()
        {
            return GetType().GetProperties();
        }

        /// <summary>
        /// Retrieves the string representation of the specified property value for the current instance, applying
        /// type-specific formatting and conversions as needed.
        /// </summary>
        /// <remarks>For numeric types (double, decimal, float), the value is formatted with two decimal
        /// places using invariant culture. For boolean values, returns "1" for true and "0" for false. For nullable
        /// types, returns an empty string if the value is null. For enum types BillingFrequency and SubscriptionType,
        /// returns the underlying byte value as a string. For DateTime values, the value is formatted using the
        /// ToPayFastDate extension method. For all other types, the value's ToString() method is used.</remarks>
        /// <param name="propertyInfo">The property metadata describing the property whose value is to be retrieved and formatted. Must not be
        /// null.</param>
        /// <returns>A string representation of the property's value, formatted according to its type. Returns an empty string if
        /// the property value is null.</returns>
        protected string GetPropertyValue(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsAssignableFrom(typeof(PayfastBillingFrequency)))
            {
                var currentEnumValue = propertyInfo.GetValue(this, null);

                if (currentEnumValue == null)
                {
                    return string.Empty;
                }

                var billingFrequency = currentEnumValue.ToEnum<PayfastBillingFrequency>();

                return ((byte)billingFrequency).ToString();
            }

            if (propertyInfo.PropertyType.IsAssignableFrom(typeof(SubscriptionType)))
            {
                var currentEnumValue = propertyInfo.GetValue(this, null);

                if (currentEnumValue == null)
                {
                    return string.Empty;
                }

                var subscriptionType = currentEnumValue.ToEnum<SubscriptionType>();

                return ((byte)subscriptionType).ToString();
            }

            if (propertyInfo.PropertyType.IsAssignableFrom(typeof(double)))
            {
                if (propertyInfo.PropertyType == typeof(double?))
                {
                    var doubleValue2 = (double?)propertyInfo.GetValue(this, null);

                    return doubleValue2.HasValue
                        ? doubleValue2.Value.ToString("F2", CultureInfo.InvariantCulture)
                        : string.Empty;
                }

                var doubleValue = (double)propertyInfo.GetValue(this, null);

                return doubleValue.ToString("F2", CultureInfo.InvariantCulture);
            }

            if (propertyInfo.PropertyType.IsAssignableFrom(typeof(decimal)))
            {
                if (propertyInfo.PropertyType == typeof(decimal?))
                {
                    var decimalValue2 = (decimal?)propertyInfo.GetValue(this, null);

                    return decimalValue2.HasValue
                        ? decimalValue2.Value.ToString("F2", CultureInfo.InvariantCulture)
                        : string.Empty;
                }

                var decimalValue = (decimal)propertyInfo.GetValue(this, null);

                return decimalValue.ToString("F2", CultureInfo.InvariantCulture);
            }

            if (propertyInfo.PropertyType.IsAssignableFrom(typeof(float)))
            {
                if (propertyInfo.PropertyType == typeof(float?))
                {
                    var floatValue2 = (float?)propertyInfo.GetValue(this, null);

                    return floatValue2.HasValue
                        ? floatValue2.Value.ToString("F2", CultureInfo.InvariantCulture)
                        : string.Empty;
                }

                var floatValue = (float)propertyInfo.GetValue(this, null);

                return floatValue.ToString("F2", CultureInfo.InvariantCulture);
            }

            if (propertyInfo.PropertyType.IsAssignableFrom(typeof(bool)))
            {
                if (propertyInfo.PropertyType == typeof(bool?))
                {
                    var booleanValue = (bool?)propertyInfo.GetValue(this, null);

                    return booleanValue.HasValue ? booleanValue.Value ? "1" : "0" : string.Empty;
                }
                else
                {
                    var booleanValue = (bool)propertyInfo.GetValue(this, null);

                    return booleanValue ? "1" : "0";
                }
            }

            if (propertyInfo.PropertyType.IsAssignableFrom(typeof(DateTime)))
            {
                if (propertyInfo.PropertyType == typeof(DateTime?))
                {
                    var dateTimeValue = (DateTime?)propertyInfo.GetValue(this, null);

                    return dateTimeValue.ToPayFastDate();
                }
                else
                {
                    var dateTimeValue = (DateTime)propertyInfo.GetValue(this, null);

                    return dateTimeValue.ToPayFastDate();
                }
            }

            return propertyInfo.GetValue(this, null) == null ? string.Empty : propertyInfo.GetValue(this, null).ToString();
        }

        /// <summary>
        /// Encodes a URL string by replacing reserved and unsafe characters with their percent-encoded representations.
        /// </summary>
        /// <remarks>This method encodes characters commonly reserved or unsafe in URLs, such as spaces,
        /// punctuation, and special symbols. Spaces are encoded as '+'. Use this method to prepare URL components for
        /// safe transmission over HTTP.</remarks>
        /// <param name="url">The URL string to encode. Cannot be null.</param>
        /// <returns>A URL-encoded string in which reserved and unsafe characters are replaced with percent-encoded values.</returns>
        protected string UrlEncode(string url)
        {
            Dictionary<string, string> convertPairs = new Dictionary<string, string>() { { "%", "%25" }, { "!", "%21" }, { "#", "%23" }, { " ", "+" },
            { "$", "%24" }, { "&", "%26" }, { "'", "%27" }, { "(", "%28" }, { ")", "%29" }, { "*", "%2A" }, { "+", "%2B" }, { ",", "%2C" },
            { "/", "%2F" }, { ":", "%3A" }, { ";", "%3B" }, { "=", "%3D" }, { "?", "%3F" }, { "@", "%40" }, { "[", "%5B" }, { "]", "%5D" } };

            var replaceRegex = new Regex(@"[%!# $&'()*+,/:;=?@\[\]]");
            MatchEvaluator matchEval = match => convertPairs[match.Value];
            string encoded = replaceRegex.Replace(url, matchEval);

            return encoded;
        }

        /// <summary>
        /// Generates an MD5 hash string for the specified input, optionally incorporating a passphrase if one is set.
        /// </summary>
        /// <remarks>If a passphrase is set, it is appended to the input before hashing. The method uses
        /// the system's MD5 implementation if available; otherwise, it falls back to a managed MD5 implementation. The
        /// output is always a 32-character hexadecimal string.</remarks>
        /// <param name="input">A <see cref="StringBuilder"/> containing the input data to hash. The content of this builder is used as the
        /// basis for the hash computation.</param>
        /// <returns>A lowercase hexadecimal string representing the MD5 hash of the input data, including the passphrase if
        /// specified.</returns>
        protected string CreateHash(StringBuilder input)
        {
            var inputStringBuilder = new StringBuilder(input.ToString());
            if (!string.IsNullOrWhiteSpace(passPhrase))
            {
                inputStringBuilder.Append($"passphrase={this.UrlEncode(this.passPhrase)}");
            }

            var inputBytes = Encoding.ASCII.GetBytes(inputStringBuilder.ToString());

            byte[] hash;

            try
            {
                using var md5 = MD5.Create();
                hash = md5.ComputeHash(inputBytes);
            }
            catch (CryptographicException)
            {
                hash = ManagedMd5.ComputeHash(inputBytes);
            }
            catch (PlatformNotSupportedException)
            {
                hash = ManagedMd5.ComputeHash(inputBytes);
            }

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        #endregion Methods
    }
}
