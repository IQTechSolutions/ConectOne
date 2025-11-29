using System.Security.Cryptography;
using System.Text;

namespace PayFast.Extensions
{
    /// <summary>
    /// Provides extension methods for the StringBuilder class.
    /// </summary>
    /// <remarks>This static class contains methods that extend the functionality of the StringBuilder type,
    /// enabling additional operations that are not available in the standard StringBuilder API.</remarks>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Computes the MD5 hash of the string represented by the specified StringBuilder and returns the hash as a
        /// hexadecimal string.
        /// </summary>
        /// <remarks>The returned hash is always 32 characters long, corresponding to the 128-bit MD5
        /// hash. The input is encoded using ASCII before hashing. This method does not modify the original
        /// StringBuilder.</remarks>
        /// <param name="input">The StringBuilder whose contents are used to compute the hash. Cannot be null.</param>
        /// <returns>A 32-character, lowercase hexadecimal string representing the MD5 hash of the input.</returns>
        public static string CreateHash(this StringBuilder input)
        {
            var inputStringBuilder = new StringBuilder(input.ToString());

            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(inputStringBuilder.ToString());

            var hash = md5.ComputeHash(inputBytes);

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
