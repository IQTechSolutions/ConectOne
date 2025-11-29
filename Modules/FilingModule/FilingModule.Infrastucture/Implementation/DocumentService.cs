using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Extensions;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.DataTransferObjects;
using FilingModule.Domain.Entities;
using FilingModule.Domain.Interfaces;
using FilingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace FilingModule.Infrastucture.Implementation
{
    /// <summary>
    /// Provides functionality for managing and retrieving document-related data for a specified entity type.
    /// </summary>
    /// <remarks>This service is designed to handle operations such as retrieving paginated document data,
    /// fetching all documents for a specific entity,  and retrieving individual document details. It relies on a
    /// repository pattern for data access and supports filtering by entity ID.</remarks>
    /// <typeparam name="TEntity">The type of the entity associated with the documents. Must implement <see cref="IAuditableEntity"/> with a
    /// key of type <see cref="string"/>.</typeparam>
    public class DocumentService<TEntity>(IRepository<EntityDocument<TEntity, string>, string> entityDocumentRepository, IRepository<Document, string> documentRepository) : IDocumentService<TEntity> where TEntity : IAuditableEntity<string>
    {
        /// <summary>
        /// Retrieves a paginated list of documents based on the specified parameters.
        /// </summary>
        /// <remarks>This method filters documents by the specified company ID and includes related
        /// document type information. The result is paginated based on the provided page number and page
        /// size.</remarks>
        /// <param name="parameters">The pagination and filtering parameters, including the company ID, page number, and page size.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing a list of <see cref="DocumentDto"/> objects that match the
        /// specified criteria. If no documents are found, the result will contain an empty list.</returns>
        public async Task<PaginatedResult<DocumentDto>> PagedDocumentsAsync(DocumentPageParameters parameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<EntityDocument<TEntity, string>>(c => c.EntityId == parameters.CompanyId);
            spec.AddInclude(c => c.Include(i => i.Document));
            spec.AddInclude(c => (IIncludableQueryable<EntityDocument<TEntity, string>, object>)c.Include(i => i.Entity));

            var result = await entityDocumentRepository.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<DocumentDto>.Failure(result.Messages);

            var response = result.Data.Select(DocumentDto.ToDto).ToList();

            return await response.ToPaginatedListAsync(parameters.PageNr, parameters.PageSize);
        }

        /// <summary>
        /// Retrieves a collection of documents associated with the specified company,  including their metadata and
        /// document type details.
        /// </summary>
        /// <remarks>Each document in the result includes its metadata, such as title, description,
        /// creation details,  and a reference to its document type. The method filters documents by the specified
        /// company ID.</remarks>
        /// <param name="parameters">The parameters for filtering and paginating the documents.  The <see
        /// cref="DocumentPageParameters.CompanyId"/> property must be specified to identify the company.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// where T is an <see cref="IEnumerable{T}"/> of <see cref="DocumentDto"/> objects.  If successful, the result
        /// contains the list of documents; otherwise, it contains an error message.</returns>
        public async Task<IBaseResult<IEnumerable<DocumentDto>>> AllDocumentsAsync(DocumentPageParameters parameters, CancellationToken cancellationToken = default)
        {
            try
            {
                var spec = new LambdaSpec<EntityDocument<TEntity, string>>(c => c.EntityId == parameters.CompanyId);
                spec.AddInclude(c => c.Include(i => i.Document));
                spec.AddInclude(c => (IIncludableQueryable<EntityDocument<TEntity, string>, object>)c.Include(i => i.Entity));

                var result = await entityDocumentRepository.ListAsync(spec, false, cancellationToken);

                var response = result.Data.Select(DocumentDto.ToDto).ToList();

                return await Result<IEnumerable<DocumentDto>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<DocumentDto>>.FailAsync(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a document by its unique identifier and returns its details as a data transfer object (DTO).
        /// </summary>
        /// <remarks>This method queries the database for a document matching the specified <paramref
        /// name="documentId"/>. If a matching document is found, its details are mapped to a <see cref="DocumentDto"/>
        /// object and returned. If no matching document is found, the result indicates failure with an appropriate
        /// error message.</remarks>
        /// <param name="documentId">The unique identifier of the document to retrieve. This value cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="DocumentDto"/>. If the operation succeeds, the result contains the document
        /// details; otherwise, it contains error messages indicating the failure reason.</returns>
        public async Task<IBaseResult<DocumentDto>> DocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<EntityDocument<TEntity, string>>(c => c.EntityId == documentId);
            spec.AddInclude(c => c.Include(i => i.Document));
            spec.AddInclude(c => (IIncludableQueryable<EntityDocument<TEntity, string>, object>)c.Include(i => i.Entity));


            var documentResult = await entityDocumentRepository.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!documentResult.Succeeded) return await Result<DocumentDto>.FailAsync(documentResult.Messages);

            if (documentResult.Data is null) return await Result<DocumentDto>.FailAsync($"No document with id matching : '{documentId}' was found in the database");

            var response = DocumentDto.ToDto(documentResult.Data);

            return await Result<DocumentDto>.SuccessAsync(response);
        }        
    }
}
