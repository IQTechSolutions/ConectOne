using ConectOne.Domain.ResultWrappers;

namespace ConectOne.EntityFrameworkCore.Sql.Extensions
{
	/// <summary>
	/// Provides extension methods for paginating queryable and list data sources.
	/// </summary>
	/// <remarks>The methods in this class enable efficient retrieval of paged results from collections or queryable
	/// sources, returning results in a standardized paginated format. These extensions are intended to simplify pagination
	/// logic in applications that work with large data sets.</remarks>
	public static class QueryableExtensions
	{
		/// <summary>
		/// Asynchronously creates a paginated result from the specified queryable source.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
		/// <param name="source">The queryable data source to paginate. Cannot be null.</param>
		/// <param name="pageNumber">The one-based index of the page to retrieve. If zero or less, defaults to 1.</param>
		/// <param name="pageSize">The number of items per page. If zero or less, defaults to 10.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a PaginatedResult<T> with the items
		/// for the specified page and pagination metadata.</returns>
		/// <exception cref="Exception">Thrown if the source is null.</exception>
		public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
		{
			if (source == null) throw new Exception();
			pageNumber = pageNumber == 0 ? 1 : pageNumber;
			pageSize = pageSize == 0 ? 10 : pageSize;
			int count = source.Count();
			pageNumber = pageNumber <= 0 ? 1 : pageNumber;
			List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
			return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
		}

		/// <summary>
		/// Creates a paginated result from the specified list by returning a subset of items for the given page number and
		/// page size.
		/// </summary>
		/// <typeparam name="T">The type of elements in the source list. Must be a reference type.</typeparam>
		/// <param name="source">The list of items to paginate. Cannot be null.</param>
		/// <param name="pageNumber">The one-based index of the page to retrieve. If less than or equal to zero, defaults to 1.</param>
		/// <param name="pageSize">The number of items per page. If less than or equal to zero, defaults to 10.</param>
		/// <returns>A <see cref="PaginatedResult{T}"/> containing the items for the specified page, along with pagination metadata. If
		/// the source list is empty, the result contains an empty items collection.</returns>
		/// <exception cref="Exception">Thrown if <paramref name="source"/> is null.</exception>
		public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this List<T> source, int pageNumber, int pageSize) where T : class
		{
			if (source == null) throw new Exception();
			pageNumber = pageNumber == 0 ? 1 : pageNumber;
			pageSize = pageSize == 0 ? 10 : pageSize;
			int count = source.Count();
			pageNumber = pageNumber <= 0 ? 1 : pageNumber;
			List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
			return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
		}
	}
}
