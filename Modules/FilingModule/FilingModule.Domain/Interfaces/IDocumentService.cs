using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.RequestFeatures;

namespace FilingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a service for managing and retrieving documents with support for pagination and filtering.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that the service operates on. Must implement <see cref="IAuditableEntity"/>.</typeparam>
    public interface IDocumentService<TEntity> where TEntity : IAuditableEntity
    {
        /// <summary>
        /// Retrieves a paginated list of documents based on the specified parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve documents in a paginated manner, which is useful for
        /// scenarios involving large datasets. Ensure that the <paramref name="parameters"/> object is properly
        /// configured to avoid unexpected results.</remarks>
        /// <param name="parameters">The parameters that define the pagination and filtering options for the document query. This includes page
        /// size, page number, and any additional filters.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a collection of <see cref="DocumentDto"/> objects for the
        /// requested page, along with pagination metadata such as total count and page information.</returns>
        Task<PaginatedResult<DocumentDto>> PagedDocumentsAsync(DocumentPageParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of all documents based on the specified parameters.
        /// </summary>
        /// <remarks>The method supports pagination and filtering to efficiently retrieve large sets of
        /// documents.  Ensure that the <parameters> object is properly configured to avoid unexpected
        /// results.</remarks>
        /// <param name="parameters">The pagination and filtering parameters used to control the document retrieval.  This includes page size,
        /// page number, and any applicable filters.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> object
        /// with an enumerable collection of <DocumentDto>  representing the retrieved documents.</returns>
        Task<IBaseResult<IEnumerable<DocumentDto>>> AllDocumentsAsync(DocumentPageParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the document associated with the specified document ID asynchronously.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch a document by its ID. Ensure
        /// that the provided <paramref name="documentId"/> is valid and corresponds to an existing document in the
        /// system.</remarks>
        /// <param name="documentId">The unique identifier of the document to retrieve. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping a <see cref="DocumentDto"/> instance representing the document. If the document is not
        /// found, the result may indicate an error or a null value, depending on the implementation.</returns>
        Task<IBaseResult<DocumentDto>> DocumentAsync(string documentId, CancellationToken cancellationToken = default);
    }
}
