namespace PayFast.Extensions
{
    /// <summary>
    /// Provides extension methods for working with <see cref="Dictionary{TKey, TValue}"/> instances that use <see
    /// cref="string"/> keys and values.
    /// </summary>
    /// <remarks>These methods extend <see cref="Dictionary{TKey, TValue}"/> to simplify common operations
    /// when working with dictionaries that have <see cref="string"/> keys and values. All methods require a non-null
    /// dictionary instance.</remarks>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds a new key and value to the dictionary, or updates the value if the key already exists.
        /// </summary>
        /// <remarks>If the specified key does not exist in the dictionary, the method adds the key with
        /// the provided value. If the key exists, its value is updated. When adding a new key, a null or empty value is
        /// stored as an empty string; when updating, the value is set as provided, including null.</remarks>
        /// <param name="dictionary">The dictionary to add the key and value to, or to update if the key already exists. Cannot be null.</param>
        /// <param name="key">The key to add or update in the dictionary.</param>
        /// <param name="value">The value to associate with the specified key. If null or empty, an empty string is used when adding a new
        /// key.</param>
        /// <exception cref="ArgumentNullException">Thrown if the dictionary parameter is null.</exception>
        public static void AddOrUpdate(this Dictionary<string, string> dictionary, string key, string value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary cannot be null");
            }

            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key: key, value: string.IsNullOrEmpty(value) ? string.Empty : value);
            }
            else
            {
                dictionary[key] = value;
            }
        }

        /// <summary>
        /// Retrieves the value associated with the specified key from the dictionary, or returns null if the key does
        /// not exist.
        /// </summary>
        /// <param name="dictionary">The dictionary to search for the specified key. Cannot be null.</param>
        /// <param name="key">The key whose associated value is to be returned.</param>
        /// <returns>The value associated with the specified key, or null if the key is not found in the dictionary.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dictionary"/> is null.</exception>
        public static string ValueAs(this Dictionary<string, string> dictionary, string key)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary cannot be null");
            }

            if (!dictionary.ContainsKey(key))
            {
                return null;
            }
            else
            {
                return dictionary[key];
            }
        }
    }
}
