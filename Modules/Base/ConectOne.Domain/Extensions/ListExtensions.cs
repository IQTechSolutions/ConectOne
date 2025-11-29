using ConectOne.Domain.Interfaces;

namespace ConectOne.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for common operations on generic list and dictionary collections.
    /// </summary>
    /// <remarks>The ListExtensions class includes utility methods for retrieving default entries, adding
    /// multiple items to a dictionary, splitting lists into smaller segments, and shuffling list elements. These
    /// methods are designed to simplify common collection manipulation tasks and can be used with standard .NET
    /// collection types.</remarks>
    public static class ListExtensions
    {
        /// <summary>
        /// Returns the default entry from the specified collection, or the first entry if no default is found.
        /// </summary>
        /// <remarks>If multiple entries are marked as default, the first such entry is returned. If no
        /// entries are marked as default, the method returns the first entry in the collection. This method is intended
        /// for use with collections where at most one entry is expected to be marked as default.</remarks>
        /// <typeparam name="T">The type of elements in the collection. Must implement <see cref="IDefaultEntity"/>.</typeparam>
        /// <param name="listWithDefaultEntry">The collection of entities to search for a default entry. Cannot be null.</param>
        /// <returns>The default entry in the collection if one exists; otherwise, the first entry. Returns <see
        /// langword="null"/> if the collection is empty.</returns>
        public static T? GetDefaultEntry<T>(this ICollection<T> listWithDefaultEntry) where T : IDefaultEntity
        {
            if (listWithDefaultEntry.Any())
            {
                if (listWithDefaultEntry.Any(c => c.Default))
                {
                    return listWithDefaultEntry.FirstOrDefault(c => c.Default);
                }
                return listWithDefaultEntry.FirstOrDefault();
            }
            return default;
        }

        /// <summary>
        /// Adds the key/value pairs from the specified dictionary to the source dictionary if the key does not already
        /// exist in the source.
        /// </summary>
        /// <remarks>If a key in the collection already exists in the source dictionary, that key/value
        /// pair is skipped and not added. No existing entries in the source dictionary are overwritten.</remarks>
        /// <typeparam name="T">The type of the keys in the dictionaries. This type must not be nullable.</typeparam>
        /// <typeparam name="S">The type of the values in the dictionaries.</typeparam>
        /// <param name="source">The dictionary to which the key/value pairs will be added. Must not be null.</param>
        /// <param name="collection">The dictionary containing the key/value pairs to add to the source dictionary. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection) where T : notnull
        {
            if (collection == null)
            {
                throw new ArgumentNullException($"Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                }
            }
        }

        /// <summary>
        /// Divides the input list into multiple sublists of a specified maximum size.
        /// </summary>
        /// <remarks>The returned sublists are new lists containing references to the elements from the
        /// original list. The method does not modify the input list.</remarks>
        /// <typeparam name="T">The type of elements in the input list.</typeparam>
        /// <param name="list">The list to be split into sublists. Cannot be null.</param>
        /// <param name="nSize">The maximum number of elements in each sublist. Must be greater than 0. The default value is 2.</param>
        /// <returns>An enumerable collection of sublists, each containing up to nSize elements from the original list. The last
        /// sublist may contain fewer elements if the total number of elements is not evenly divisible by nSize.</returns>
        public static IEnumerable<List<T>> SplitList<T>(this List<T> list, int nSize = 2)
        {
            for (int i = 0; i < list.Count; i += nSize)
            {
                yield return list.GetRange(i, Math.Min(nSize, list.Count - i));
            }
        }

        /// <summary>
        /// Randomly reorders the elements in the specified list in place.
        /// </summary>
        /// <remarks>This method modifies the original list. The shuffle is performed using a random
        /// number generator, resulting in a different order each time the method is called. The operation is performed
        /// in-place and does not allocate a new list.</remarks>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list whose elements will be shuffled. Cannot be null.</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
