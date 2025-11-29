using System.Text.RegularExpressions;

namespace PayFast.Extensions
{
    /// <summary>
    /// Provides extension methods for working with strings.
    /// </summary>
    /// <remarks>This class contains static methods that extend the functionality of the <see cref="string"/>
    /// type. All members are thread safe and can be used without instantiating the class.</remarks>
    public static class StringExtensions
    {
        /// <summary>
        /// Encodes a URL string by replacing reserved and unsafe characters with their percent-encoded representations.
        /// </summary>
        /// <remarks>This method encodes characters commonly reserved or unsafe in URLs, such as spaces,
        /// punctuation, and special symbols, to ensure the resulting string is safe for use in HTTP requests. Spaces
        /// are encoded as '+'. This method does not encode every possible character that may require encoding in all
        /// contexts; for full compliance with URL encoding standards, consider using System.Net.WebUtility.UrlEncode or
        /// a similar method.</remarks>
        /// <param name="url">The URL string to encode. Cannot be null.</param>
        /// <returns>A URL-encoded string in which reserved and unsafe characters are replaced with their percent-encoded
        /// equivalents. Returns an empty string if the input is empty.</returns>
        public static string UrlEncode(this string url)
        {
            Dictionary<string, string> convertPairs = new Dictionary<string, string>() { { "%", "%25" }, { "!", "%21" }, { "#", "%23" }, { " ", "+" },
            { "$", "%24" }, { "&", "%26" }, { "'", "%27" }, { "(", "%28" }, { ")", "%29" }, { "*", "%2A" }, { "+", "%2B" }, { ",", "%2C" },
            { "/", "%2F" }, { ":", "%3A" }, { ";", "%3B" }, { "=", "%3D" }, { "?", "%3F" }, { "@", "%40" }, { "[", "%5B" }, { "]", "%5D" } };

            var replaceRegex = new Regex(@"[%!# $&'()*+,/:;=?@\[\]]");
            MatchEvaluator matchEval = match => convertPairs[match.Value];
            string encoded = replaceRegex.Replace(url, matchEval);

            return encoded;
        }
    }
}
