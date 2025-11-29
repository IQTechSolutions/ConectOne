using System.Globalization;

namespace ConectOne.Domain.CsvGenerator
{
    /// <summary>
    /// Represents the configuration settings for reading and writing CSV files.
    /// </summary>
    public class CsvConfiguration(CultureInfo cultureInfo)
    {
        /// <summary>
        /// Gets or sets the culture information to use for parsing and formatting.
        /// </summary>
        public CultureInfo CultureInfo { get; set; } = cultureInfo;

        /// <summary>
        /// Gets or sets the delimiter character to use for separating fields in the CSV file.
        /// The default value is a comma (",").
        /// </summary>
        public string Delimiter { get; set; } = ",";

        /// <summary>
        /// Gets or sets a value indicating whether the CSV file has a header record.
        /// The default value is true.
        /// </summary>
        public bool HasHeaderRecord { get; set; } = true;
    }
}
