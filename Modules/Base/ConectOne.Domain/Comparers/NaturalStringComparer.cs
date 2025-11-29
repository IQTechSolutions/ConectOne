using System.Text.RegularExpressions;

namespace ConectOne.Domain.Comparers
{
    /// <summary>
    /// Provides a string comparer that performs natural (numeric-aware) ordering, comparing embedded numbers within
    /// strings as numeric values rather than as text.
    /// </summary>
    /// <remarks>Use this comparer to sort strings in a way that accounts for numeric values within the text,
    /// such as ordering "file2" before "file10". This is useful for scenarios like sorting file names, version numbers,
    /// or other strings containing digits where standard lexicographical comparison would produce unintuitive results.
    /// The comparison is case-sensitive and only considers the first sequence of digits found in each string for
    /// numeric comparison; if no digits are found, a standard string comparison is used.</remarks>
    public class NaturalStringComparer : IComparer<string>
    {
        /// <summary>
        /// Compares two strings by locating and comparing the first integer value found in each string. If neither
        /// string contains a number, performs a standard string comparison.
        /// </summary>
        /// <remarks>If both strings contain at least one integer, the comparison is based on the first
        /// integer found in each string. If only one string contains a number, it is considered greater. If neither
        /// string contains a number, the method falls back to standard string comparison. Null values are considered
        /// less than non-null values.</remarks>
        /// <param name="left">The first string to compare. Can be null.</param>
        /// <param name="right">The second string to compare. Can be null.</param>
        /// <returns>A signed integer that indicates the relative values of the strings: less than zero if left is less than
        /// right; zero if left equals right; greater than zero if left is greater than right.</returns>
        public int Compare(string left, string right)
        {
            if (left == right) return 0;
            if (left is null) return -1;
            if (right is null) return 1;

            var reg = new Regex(@"\d+");

            // Find the first number in each string
            var leftMatch = reg.Match(left);
            var rightMatch = reg.Match(right);

            // Try and find numbers in both strings
            if (leftMatch.Success || rightMatch.Success)
            {
                // Numbers were found for both. Compare those
                return int.Parse(leftMatch.Captures[0].Value).CompareTo(int.Parse(rightMatch.Captures[0].Value));
            }

            // Use the default string comparison
            return string.Compare(left, right);
        }
    }
}
