using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ConectOne.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for common string manipulation, formatting, and encoding operations.
    /// </summary>
    /// <remarks>The methods in this class extend the functionality of the string type, offering utilities for
    /// truncation, casing, extracting initials, Base64 encoding and decoding, and HTML-to-plain-text conversion. These
    /// methods are designed to simplify common string-related tasks and can be used as extension methods on string
    /// instances for more fluent and readable code.</remarks>
    public static class StringExtensions
    {
        /// <summary>
        /// Truncates a string to a specified maximum length and appends " ..." if truncated.
        /// </summary>
        /// <param name="str">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the string.</param>
        /// <returns>The truncated string with " ..." appended if it exceeds the maximum length.</returns>
        public static string TruncateLongString(this string? str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > maxLength)
                {
                    return str.Substring(0, Math.Min(str.Length, maxLength)) + "...";
                }
                return str;
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts the first letter of a string to uppercase.
        /// </summary>
        /// <param name="text">The string to convert.</param>
        /// <returns>The string with the first letter in uppercase.</returns>
        public static string UppercaseFirstLetter(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            char[] a = text.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Gets all the first letters of each word in a string.
        /// </summary>
        /// <param name="text">The string to process.</param>
        /// <returns>A list of first letters of each word in the string.</returns>
        public static List<char> GetAllFirstLetters(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<char>();

            var list = new List<char>();
            string[] output = text.Split(' ');
            foreach (string s in output)
            {
                if (s.Length > 0)
                {
                    list.Add(char.ToUpper(s[0]));
                }
            }
            return list;
        }

        /// <summary>
        /// Gets the initials from a full name (e.g., "John Doe" => "JD").
        /// </summary>
        /// <param name="name">The name the initials should be retrieved from</param>
        /// <returns>The initials of the name given</returns>
        public static string GetInitials(this string name)
        {
            var parts = name.Split(' ');
            return string.Concat(parts.Select(p => p.FirstOrDefault())).ToUpperInvariant();
        }

        /// <summary>
        /// Creates a string from a list of characters.
        /// </summary>
        /// <param name="characters">The list of characters.</param>
        /// <returns>A string created from the list of characters.</returns>
        public static string CreateStringFromCharacters(this List<char> characters)
        {
            return string.Concat(characters);
        }

        /// <summary>
        /// Converts HTML content to plain text.
        /// </summary>
        /// <param name="html">The HTML content to convert.</param>
        /// <returns>The plain text representation of the HTML content.</returns>
        public static string? HtmlToPlainText(this string? html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<"; // Matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)"; // Matches any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>"; // Matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />

            if (!string.IsNullOrEmpty(html))
            {
                var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
                var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
                var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

                var text = html;
                // Decode HTML specific characters
                text = System.Net.WebUtility.HtmlDecode(text);
                // Remove tag whitespace/line breaks
                text = tagWhiteSpaceRegex.Replace(text, "><");
                // Replace <br /> with line breaks
                text = lineBreakRegex.Replace(text, Environment.NewLine);
                // Strip formatting
                text = stripFormattingRegex.Replace(text, string.Empty);

                return text;
            }
            return html;
        }

        /// <summary>
        /// Encodes a string to Base64.
        /// </summary>
        /// <param name="plainText">The plain text to encode.</param>
        /// <param name="encoding">The encoding to use.</param>
        /// <param name="encoderShouldEmitIdentifier">Indicates whether the encoder should emit an identifier.</param>
        /// <returns>The Base64 encoded string.</returns>
        public static string ToBase64Encode(this string plainText, Encoding encoding, bool encoderShouldEmitIdentifier)
        {
            if (encoderShouldEmitIdentifier)
            {
                return Convert.ToBase64String(encoding.GetPreamble().Concat(encoding.GetBytes(plainText)).ToArray());
            }
            else
            {
                return Convert.ToBase64String(encoding.GetBytes(plainText));
            }
        }

        /// <summary>
        /// Checks if a string is a valid Base64 encoded string.
        /// </summary>
        /// <param name="base64">The string to check.</param>
        /// <returns>True if the string is a valid Base64 encoded string, otherwise false.</returns>
        public static bool IsBase64String(this string base64)
        {
            if (base64.Contains(","))
            {
                base64 = base64.Remove(0, base64.LastIndexOf(",")).TrimStart(',');

            }

            var buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }

        /// <summary>
        /// Converts a Base64-encoded string into its corresponding byte array representation.
        /// </summary>
        /// <remarks>If the input string contains a comma, the method extracts the substring following the
        /// last comma before decoding. This behavior is useful for handling Base64 strings that may include metadata or
        /// prefixes.</remarks>
        /// <param name="base64">The Base64-encoded string to convert. If the string contains a comma, the portion after the last comma is
        /// used.</param>
        /// <returns>A byte array containing the decoded data from the Base64 string.</returns>
        public static byte[] GetBase64String(this string base64)
        {
            if (base64.Contains(","))
            {
                base64 = base64.Remove(0, base64.LastIndexOf(",")).TrimStart(',');

            }
            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// Converts the specified string to title case, capitalizing the first letter of each word.
        /// </summary>
        /// <param name="source">The string to convert to title case. Can be null or empty.</param>
        /// <returns>A new string in title case. If the input is null or empty, returns the original value.</returns>
        public static string ToTitleCase2(this string source) => ToTitleCase2(source, null);

        /// <summary>
        /// Converts the specified string to title case using the casing rules of the specified culture.
        /// </summary>
        /// <param name="source">The string to convert to title case.</param>
        /// <param name="culture">The culture whose casing rules are used to convert the string. If null, the current UI culture is used.</param>
        /// <returns>A new string in which the first letter of each word is capitalized according to the specified culture's
        /// rules.</returns>
        public static string ToTitleCase2(this string source, CultureInfo culture)
        {
            culture = culture ?? CultureInfo.CurrentUICulture;
            return culture.TextInfo.ToTitleCase(source.ToLower());
        }

    }
}
