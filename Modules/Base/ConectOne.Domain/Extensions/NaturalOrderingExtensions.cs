using System.Text.RegularExpressions;

namespace ConectOne.Domain.Extensions;

/// <summary>
/// Provides helpers for ordering sequences of text that contain embedded numeric values.
/// </summary>
public static partial class NaturalOrderingExtensions
{
    private static readonly Regex NumericTokenRegex = NumericTokenRegexFactory();

    /// <summary>
    /// Orders the <paramref name="source"/> sequence by extracting the first numeric value from the
    /// text returned by <paramref name="selector"/> and then sorting alphabetically when numeric
    /// portions are equal.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">The sequence to order.</param>
    /// <param name="selector">Projects an element to the text that contains a numeric component.</param>
    /// <returns>The ordered sequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
    public static IEnumerable<TSource> OrderByNumericText<TSource>(this IEnumerable<TSource> source, Func<TSource, string?> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        return source
            .Select(item =>
            {
                var text = selector(item);
                var projection = CreateOrderingProjection(item, text);
                return projection;
            })
            .OrderBy(entry => entry.NumericValue)
            .ThenBy(entry => entry.AlphabeticSegment, StringComparer.OrdinalIgnoreCase)
            .ThenBy(entry => entry.Text ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            .Select(entry => entry.Item);
    }

    /// <summary>
    /// Orders a sequence of strings based on the numeric values contained within each string.
    /// </summary>
    /// <param name="source">The sequence of strings to order.</param>
    /// <returns>The ordered sequence of strings.</returns>
    public static IEnumerable<string?> OrderByNumericText(this IEnumerable<string?> source)
        => source.OrderByNumericText(static value => value);

    /// <summary>
    /// Creates an ordering projection for the specified item based on the provided text, extracting numeric and
    /// alphabetic segments for ordering purposes.
    /// </summary>
    /// <remarks>If the text is null or consists only of whitespace, the ordering projection will use the
    /// maximum possible numeric value and an empty alphabetic segment. If the text contains a leading numeric segment,
    /// it is extracted and used for ordering; otherwise, the entire text is treated as the alphabetic
    /// segment.</remarks>
    /// <typeparam name="T">The type of the item to associate with the ordering projection.</typeparam>
    /// <param name="item">The item to associate with the ordering projection.</param>
    /// <param name="text">The text used to extract ordering information. May contain numeric and alphabetic segments. Can be null or
    /// whitespace.</param>
    /// <returns>An OrderingProjection<T> that encapsulates the item and its associated ordering information derived from the
    /// text.</returns>
    private static OrderingProjection<T> CreateOrderingProjection<T>(T item, string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return new OrderingProjection<T>(item, text, long.MaxValue, string.Empty);
        }

        var match = NumericTokenRegex.Match(text);
        if (match.Success && long.TryParse(match.Value, out var value))
        {
            var alphabeticSegment = ExtractAlphabeticSegment(text, match.Index + match.Length);
            return new OrderingProjection<T>(item, text, value, alphabeticSegment);
        }

        return new OrderingProjection<T>(item, text, long.MaxValue, text);
    }

    /// <summary>
    /// Extracts a substring from the specified position in the input text, skipping any leading non-alphanumeric
    /// characters.
    /// </summary>
    /// <remarks>This method is intended to ignore leading spaces, punctuation, or other non-alphanumeric
    /// characters when extracting a segment for purposes such as alphabetical ordering or searching.</remarks>
    /// <param name="text">The input string from which to extract the segment. Cannot be null.</param>
    /// <param name="startIndex">The zero-based index in the input string at which to begin extraction.</param>
    /// <returns>A substring of the input text starting from the first alphanumeric character at or after the specified index.
    /// Returns an empty string if the start index is greater than or equal to the length of the input text.</returns>
    private static string ExtractAlphabeticSegment(string text, int startIndex)
    {
        if (startIndex >= text.Length)
        {
            return string.Empty;
        }

        var span = text.AsSpan(startIndex);

        // Trim leading non-alphanumeric characters (spaces, punctuation, etc.) to avoid influencing alphabetical ordering.
        var trimmedStart = 0;
        while (trimmedStart < span.Length && !char.IsLetterOrDigit(span[trimmedStart]))
        {
            trimmedStart++;
        }

        return span[trimmedStart..].ToString();
    }

    /// <summary>
    /// Represents a projection of an item with associated ordering information, including numeric and alphabetic
    /// segments.
    /// </summary>
    /// <typeparam name="T">The type of the item being projected.</typeparam>
    /// <param name="Item">The item to be projected.</param>
    /// <param name="Text">An optional textual representation associated with the item. Can be null.</param>
    /// <param name="NumericValue">A numeric value used for ordering or comparison purposes.</param>
    /// <param name="AlphabeticSegment">An alphabetic segment used for ordering or grouping the item.</param>
    private readonly record struct OrderingProjection<T>(T Item, string? Text, long NumericValue, string AlphabeticSegment);

    /// <summary>
    /// Creates a regular expression that matches one or more consecutive digits in a string.
    /// </summary>
    /// <returns>A <see cref="Regex"/> instance configured to match numeric tokens consisting of one or more digits.</returns>
    [GeneratedRegex("(\\d+)")] private static partial Regex NumericTokenRegexFactory();
}
