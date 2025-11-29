using System.Collections.Specialized;
using System.Text;
using PayFast.Base;
using PayFast.Extensions;

namespace PayFast
{
    public class PayFastNotify : PayFastBase
    {
        #region Fields

        private readonly Dictionary<string, string> properties;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// This constructor does not allow for a argument
        /// because it is intended to be called by the mvc model binder.
        /// If a passphrase is being used, make a call to SetPassPhrase(string passPhrase)
        /// on this class after it has been passed in by the model binder
        /// See <a href="https://www.payfast.co.za/documentation/return-variables/">PayFast Return Variable Page Documentation</a> for more information.
        /// See <a href="https://www.payfast.co.za/documentation/itn/">PayFast ITN Page Documentation</a> for more information.
        /// </summary>
        public PayFastNotify() : base(string.Empty)
        {
            properties = new Dictionary<string, string>();
        }

        #endregion Constructor

        #region Properties

        // Transaction details

        /// <summary>
        /// Unique payment ID on the merchant’s system.
        /// </summary>
        public string m_payment_id
        {
            get
            {
                return properties.ValueAs(nameof(m_payment_id));
            }
            set
            {
                properties.AddOrUpdate(nameof(m_payment_id), value);
            }
        }

        /// <summary>
        /// Unique transaction ID on PayFast.
        /// </summary>
        public string pf_payment_id
        {
            get
            {
                return properties.ValueAs(nameof(pf_payment_id));
            }
            set
            {
                properties.AddOrUpdate(nameof(pf_payment_id), value);
            }
        }

        /// <summary>
        /// The status of the payment.
        /// </summary>
        public string payment_status
        {
            get
            {
                return properties.ValueAs(nameof(payment_status));
            }
            set
            {
                properties.AddOrUpdate(nameof(payment_status), value);
            }
        }

        /// <summary>
        /// The name of the item being charged for.
        /// </summary>
        public string item_name
        {
            get
            {
                return properties.ValueAs(nameof(item_name));
            }
            set
            {
                properties.AddOrUpdate(nameof(item_name), value);
            }
        }

        /// <summary>
        /// The description of the item being charged for.
        /// </summary>
        public string item_description
        {
            get
            {
                return properties.ValueAs(nameof(item_description));
            }
            set
            {
                properties.AddOrUpdate(nameof(item_description), value);
            }
        }

        /// <summary>
        /// The total amount which the payer paid.
        /// </summary>
        public string amount_gross
        {
            get
            {
                return properties.ValueAs(nameof(amount_gross));
            }
            set
            {
                properties.AddOrUpdate(nameof(amount_gross), value);
            }
        }

        /// <summary>
        /// The total in fees which was deducated from the amount.
        /// </summary>
        public string amount_fee
        {
            get
            {
                return properties.ValueAs(nameof(amount_fee));
            }
            set
            {
                properties.AddOrUpdate(nameof(amount_fee), value);
            }
        }

        /// <summary>
        /// The net amount credited to the receiver’s account.
        /// </summary>
        public string amount_net
        {
            get
            {
                return properties.ValueAs(nameof(amount_net));
            }
            set
            {
                properties.AddOrUpdate(nameof(amount_net), value);
            }
        }

        /// <summary>
        /// Number 1 in a series of 5 custom string variables (custom_str1, custom_str2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_str1
        {
            get
            {
                return properties.ValueAs(nameof(custom_str1));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_str1), value);
            }
        }

        /// <summary>
        /// Number 2 in a series of 5 custom string variables (custom_str1, custom_str2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_str2
        {
            get
            {
                return properties.ValueAs(nameof(custom_str2));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_str2), value);
            }
        }

        /// <summary>
        /// Number 3 in a series of 5 custom string variables (custom_str1, custom_str2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_str3
        {
            get
            {
                return properties.ValueAs(nameof(custom_str3));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_str3), value);
            }
        }

        /// <summary>
        /// Number 4 in a series of 5 custom string variables (custom_str1, custom_str2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_str4
        {
            get
            {
                return properties.ValueAs(nameof(custom_str4));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_str4), value);
            }
        }

        /// <summary>
        /// Number 5 in a series of 5 custom string variables (custom_str1, custom_str2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_str5
        {
            get
            {
                return properties.ValueAs(nameof(custom_str5));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_str5), value);
            }
        }

        /// <summary>
        /// Number 1 in a series of 5 custom integer variables (custom_int1, custom_int2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_int1
        {
            get
            {
                return properties.ValueAs(nameof(custom_int1));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_int1), value);
            }
        }

        /// <summary>
        /// Number 2 in a series of 5 custom integer variables (custom_int1, custom_int2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_int2
        {
            get
            {
                return properties.ValueAs(nameof(custom_int2));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_int2), value);
            }
        }

        /// <summary>
        /// Number 3 in a series of 5 custom integer variables (custom_int1, custom_int2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_int3
        {
            get
            {
                return properties.ValueAs(nameof(custom_int3));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_int3), value);
            }
        }

        /// <summary>
        /// Number 4 in a series of 5 custom integer variables (custom_int1, custom_int2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_int4
        {
            get
            {
                return properties.ValueAs(nameof(custom_int4));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_int4), value);
            }
        }

        /// <summary>
        /// Number 5 in a series of 5 custom integer variables (custom_int1, custom_int2…) 
        /// which can be used by the merchant as pass-through variables. 
        /// They will be posted back to the merchant at the completion of the transaction.
        /// </summary>
        public string custom_int5
        {
            get
            {
                return properties.ValueAs(nameof(custom_int5));
            }
            set
            {
                properties.AddOrUpdate(nameof(custom_int5), value);
            }
        }

        // Buyer details

        /// <summary>
        /// The buyer’s first name.
        /// </summary>
        public string name_first
        {
            get
            {
                return properties.ValueAs(nameof(name_first));
            }
            set
            {
                properties.AddOrUpdate(nameof(name_first), value);
            }
        }

        /// <summary>
        /// The buyer’s last name.
        /// </summary>
        public string name_last
        {
            get
            {
                return properties.ValueAs(nameof(name_last));
            }
            set
            {
                properties.AddOrUpdate(nameof(name_last), value);
            }
        }

        /// <summary>
        /// The buyer’s email address
        /// </summary>
        public string cell_number
        {
            get
            {
                return properties.ValueAs(nameof(cell_number));
            }
            set
            {
                properties.AddOrUpdate(nameof(cell_number), value);
            }
        }

        /// <summary>
        /// Gets or sets the email address associated with the entity.
        /// </summary>
        public string email_address
        {
            get
            {
                return properties.ValueAs(nameof(email_address));
            }
            set
            {
                properties.AddOrUpdate(nameof(email_address), value);
            }
        }

        // Merchant details

        /// <summary>
        /// The Merchant ID as given by the PayFast system. 
        /// Used to uniquely identify the receiver’s account.
        /// </summary>
        public string merchant_id
        {
            get
            {
                return properties.ValueAs(nameof(merchant_id));
            }
            set
            {
                properties.AddOrUpdate(nameof(merchant_id), value);
            }
        }

        // Recurring billing details

        /// <summary>
        /// Unique ID on PayFast that represents the subscription
        /// </summary>
        public string token
        {
            get
            {
                return properties.ValueAs(nameof(token));
            }
            set
            {
                properties.AddOrUpdate(nameof(token), value);
            }
        }

        /// <summary>
        /// The date from which future subscription payments will be made.
        /// </summary>
        public string billing_date
        {
            get
            {
                return properties.ValueAs(nameof(billing_date));
            }
            set
            {
                properties.AddOrUpdate(nameof(billing_date), value);
            }
        }

        // Security information

        /// <summary>
        /// A security signature of the transmitted data taking 
        /// the form of an MD5 hash of the submitted variables. 
        /// The string from which the hash is created, 
        /// is the concatenation of the name value pairs of 
        /// all the variables with ‘&’ used as a separator.
        /// </summary>
        public string signature { get; set; }

        #endregion #region Properties

        #region Methods

        /// <summary>
        /// Populates the object's properties from the specified collection of name/value pairs.
        /// </summary>
        /// <remarks>If the collection is null or empty, no properties are updated. Keys that match
        /// specific property names may be handled specially. Existing property values are overwritten by values from
        /// the collection.</remarks>
        /// <param name="nameValueCollection">A collection of key/value pairs representing form data to import. Each key corresponds to a property name,
        /// and each value is assigned to the corresponding property.</param>
        public void FromFormCollection(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (nameValueCollection == null || nameValueCollection.Count() < 1)
            {
                return;
            }

            foreach (KeyValuePair<string, string> keyValuePair in nameValueCollection)
            {
                if (keyValuePair.Key == nameof(signature))
                {
                    signature = keyValuePair.Value;

                    continue;
                }

                properties.AddOrUpdate(key: keyValuePair.Key, value: keyValuePair.Value);
            }
        }

        /// <summary>
        /// Calculates and returns the signature hash based on the current property values, excluding the signature
        /// itself and any empty optional fields.
        /// </summary>
        /// <remarks>The calculated signature excludes the 'signature' property and any optional
        /// properties such as 'billing_date' or 'token' if their values are null or empty. The result is suitable for
        /// use in authentication or verification scenarios where a consistent hash of the property values is
        /// required.</remarks>
        /// <returns>A string containing the calculated signature hash. The value is based on the current state of the properties
        /// and may differ if property values change.</returns>
        public string GetCalculatedSignature()
        {
            var stringBuilder = new StringBuilder();

            var lastEntryKey = properties.Last();

            foreach (var property in properties)
            {
                if (property.Key == nameof(signature))
                {
                    continue;
                }

                var value = property.Value;

                if (property.Key == nameof(billing_date) && string.IsNullOrEmpty(property.Value))
                {
                    continue;
                }
                if (property.Key == nameof(token) && string.IsNullOrEmpty(property.Value))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(passPhrase) && property.Key == lastEntryKey.Key)
                {
                    stringBuilder.Append($"{property.Key}={UrlEncode(property.Value)}");
                }
                else
                {
                    stringBuilder.Append($"{property.Key}={UrlEncode(property.Value)}&");
                }
            }

            return CreateHash(stringBuilder.Replace("%2C", "."));
        }

        /// <summary>
        /// Gets a dictionary containing the underlying property names and their corresponding values.
        /// </summary>
        /// <returns>A dictionary where each key is a property name and each value is the associated property value. The
        /// dictionary may be empty if no properties are defined.</returns>
        public Dictionary<string, string> GetUnderlyingProperties()
        {
            return properties;
        }

        /// <summary>
        /// Verifies whether the current signature matches the calculated security hash.
        /// </summary>
        /// <returns>true if the signature is valid and matches the calculated value; otherwise, false.</returns>
        public bool CheckSignature()
        {
            var securityHash = GetCalculatedSignature();

            return signature == securityHash;
        }

        /// <summary>
        /// Determines the last key in the specified collection that meets defined criteria for inclusion.
        /// </summary>
        /// <remarks>Entries with the key "signature" are always excluded. Entries with the key
        /// "billing_date" or "token" are excluded if their value is null or empty. All other entries are excluded if
        /// their value is null, empty, or consists only of white-space characters.</remarks>
        /// <param name="nameValueCollection">The collection of name/value pairs to evaluate. Cannot be null.</param>
        /// <returns>The key of the last entry in the collection that is not excluded by the method's criteria. Returns null if
        /// no such key is found.</returns>
        private string DetermineLast(NameValueCollection nameValueCollection)
        {
            string lastKey = nameValueCollection.GetKey(nameValueCollection.Count - 2);

            foreach (string key in nameValueCollection)
            {
                if (key == nameof(signature))
                {
                    continue;
                }

                var value = nameValueCollection[key];

                if (key == nameof(billing_date) && string.IsNullOrEmpty(value))
                {
                    continue;
                }

                if (key == nameof(token) && string.IsNullOrEmpty(value))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                lastKey = key;
            }

            return lastKey;
        }

        #endregion Methods
    }
}
