namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Represents a base class providing functionality to convert objects and their properties 
    /// to and from CSV (Comma-Separated Values) format. Derived classes can override 
    /// methods to customize CSV processing.
    /// </summary>
    public abstract record CsvableBase
    {
        /// <summary>
        /// Gets the column headers (property names) of this object, separated by commas.
        /// If a property is itself a CsvableBase, it recursively gets the headers from that object.
        /// </summary>
        /// <returns>A comma-separated string of property names.</returns>
        public virtual string GetColumnHeaders()
        {
            string output = "";

            var properties = GetType().GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                // If the property is a nested CsvableBase, get its headers via reflection
                if (properties[i].PropertyType.IsSubclassOf(typeof(CsvableBase)))
                {
                    var m = properties[i].PropertyType.GetMethod("GetColumnHeaders", new Type[0]);
                    output += m.Invoke(properties[i].GetValue(this), new object[0]);
                }
                else
                {
                    // Use the property name as the header
                    output += properties[i].Name;
                }

                if (i != properties.Length - 1)
                {
                    output += ",";
                }
            }

            return output;
        }

        /// <summary>
        /// Converts the current object's properties into a single CSV line.
        /// Nested CsvableBase objects are also converted by calling their ToCsv methods.
        /// </summary>
        /// <returns>A comma-separated string of property values.</returns>
        public virtual string ToCsv()
        {
            string output = "";
            var properties = GetType().GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                if (properties[i].PropertyType.IsSubclassOf(typeof(CsvableBase)))
                {
                    // Nested CsvableBase object - call ToCsv on that instance
                    var m = properties[i].PropertyType.GetMethod("ToCsv", new Type[0]);
                    output += m.Invoke(properties[i].GetValue(this), new object[0]);
                }
                else
                {
                    // Convert the property to a string and preprocess for CSV formatting
                    output += PreProcess(properties[i].GetValue(this)?.ToString());
                }

                if (i != properties.Length - 1)
                {
                    output += ",";
                }
            }

            return output;
        }

        /// <summary>
        /// Populates the object's properties from an array of CSV values.
        /// Matches each value in the array to a property, converting the string to the correct type.
        /// If a property is a nested CsvableBase, it consumes multiple values and invokes FromCsv on that instance.
        /// </summary>
        /// <param name="propertyValues">An array of string values representing CSV fields.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if not enough values are provided to populate all properties.</exception>
        /// <exception cref="FormatException">Thrown if a value cannot be converted to the property type.</exception>
        public virtual void FromCsv(string[] propertyValues)
        {
            var properties = GetType().GetProperties();
            int valueIndex = 0; // Tracks which CSV value we are currently assigning

            for (var i = 0; i < properties.Length; i++)
            {
                if (valueIndex >= propertyValues.Length)
                {
                    throw new IndexOutOfRangeException($"Not enough values provided. Expected {properties.Length}, got {valueIndex}.");
                }

                var property = properties[i];
                if (property.PropertyType.IsSubclassOf(typeof(CsvableBase)))
                {
                    // Handle nested CsvableBase object
                    var instance = Activator.CreateInstance(property.PropertyType);
                    var instanceProperties = instance.GetType().GetProperties();

                    if (valueIndex + instanceProperties.Length > propertyValues.Length)
                    {
                        throw new IndexOutOfRangeException($"Not enough values for nested object. Expected {instanceProperties.Length}, got {propertyValues.Length - valueIndex}.");
                    }

                    var nestedValues = propertyValues.Skip(valueIndex).Take(instanceProperties.Length).ToArray();
                    var fromCsvMethod = instance.GetType().GetMethod("FromCsv", new Type[] { typeof(string[]) });

                    if (fromCsvMethod != null)
                    {
                        fromCsvMethod.Invoke(instance, new object[] { nestedValues });
                    }
                    property.SetValue(this, instance);

                    // Advance valueIndex by the number of properties in the nested object
                    valueIndex += instanceProperties.Length;
                }
                else
                {
                    // Handle a simple property
                    var propertyType = property.PropertyType;
                    object value;

                    try
                    {
                        value = Convert.ChangeType(propertyValues[valueIndex], propertyType);
                    }
                    catch
                    {
                        throw new FormatException($"Failed to convert '{propertyValues[valueIndex]}' to {propertyType.Name}.");
                    }

                    property.SetValue(this, value);
                    valueIndex++;
                }
            }
        }

        /// <summary>
        /// Converts the object's properties to CSV, but either includes or excludes certain property names based on a list.
        /// If isIgnore is true, then the specified propertyNames are excluded. If false, only the specified propertyNames are included.
        /// </summary>
        /// <param name="propertyNames">An array of property names to include or exclude.</param>
        /// <param name="isIgnore">If true, ignores these properties. If false, only these properties are included.</param>
        /// <returns>A CSV string filtered by the property names.</returns>
        public virtual string ToCsv(string[] propertyNames, bool isIgnore)
        {
            string output = "";
            bool isFirstPropertyWritten = false;
            var properties = GetType().GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                bool shouldWrite;
                if (isIgnore)
                {
                    // Write if property is not in the ignore list
                    shouldWrite = !propertyNames.Contains(properties[i].Name);
                }
                else
                {
                    // Write if property is in the include list
                    shouldWrite = propertyNames.Contains(properties[i].Name);
                }

                if (shouldWrite)
                {
                    if (isFirstPropertyWritten)
                    {
                        output += ",";
                    }

                    if (properties[i].PropertyType.IsSubclassOf(typeof(CsvableBase)))
                    {
                        var m = properties[i].PropertyType.GetMethod("ToCsv", new Type[0]);
                        output += m.Invoke(properties[i].GetValue(this), new object[0]);
                    }
                    else
                    {
                        output += PreProcess(properties[i].GetValue(this)?.ToString());
                    }

                    if (!isFirstPropertyWritten)
                    {
                        isFirstPropertyWritten = true;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Similar to the previous ToCsv method, but uses indexes instead of property names.
        /// propertyIndexes represent the indices of properties to include or exclude.
        /// If isIgnore is true, excludes those indexes; if false, includes only those indexes.
        /// </summary>
        /// <param name="propertyIndexes">An array of property indexes.</param>
        /// <param name="isIgnore">Determines whether to ignore or include the indexed properties.</param>
        /// <returns>A CSV string filtered by the specified indexes.</returns>
        public virtual string ToCsv(int[] propertyIndexes, bool isIgnore)
        {
            string output = "";
            bool isFirstPropertyWritten = false;
            var properties = GetType().GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                bool shouldWrite;
                if (isIgnore)
                {
                    // Write if this index is not in the ignore list
                    shouldWrite = !propertyIndexes.Contains(i);
                }
                else
                {
                    // Write if this index is in the include list
                    shouldWrite = propertyIndexes.Contains(i);
                }

                if (shouldWrite)
                {
                    if (isFirstPropertyWritten)
                    {
                        output += ",";
                    }

                    if (properties[i].PropertyType.IsSubclassOf(typeof(CsvableBase)))
                    {
                        var m = properties[i].PropertyType.GetMethod("ToCsv", new Type[0]);
                        output += m.Invoke(properties[i].GetValue(this), new object[0]);
                    }
                    else
                    {
                        output += PreProcess(properties[i].GetValue(this)?.ToString());
                    }

                    if (!isFirstPropertyWritten)
                    {
                        isFirstPropertyWritten = true;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Pre-processes a string for CSV output:
        /// - Replaces certain non-English characters with English equivalents.
        /// - Escapes double quotes by doubling them.
        /// - Trims whitespace.
        /// - If the string contains a comma, encloses it in quotes.
        /// </summary>
        /// <param name="input">The input string to process.</param>
        /// <returns>The processed string safe for CSV output.</returns>
        private string PreProcess(string? input)
        {
            if (input == null)
                return "";

            input = input.Replace('ı', 'i')
                .Replace('ç', 'c')
                .Replace('ö', 'o')
                .Replace('ş', 's')
                .Replace('ü', 'u')
                .Replace('ğ', 'g')
                .Replace('İ', 'I')
                .Replace('Ç', 'C')
                .Replace('Ö', 'O')
                .Replace('Ş', 'S')
                .Replace('Ü', 'U')
                .Replace('Ğ', 'G')
                .Replace("\ufffd", "è") 
                .Replace("\"", "\"\"")
                .Trim();

            if (input.Contains(","))
            {
                input = "\"" + input + "\"";
            }

            return input;
        }
    }
}
