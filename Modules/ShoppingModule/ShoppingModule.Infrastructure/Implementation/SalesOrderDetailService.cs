using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Entities;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides operations for managing sales order details, including retrieving and creating sales order details.
    /// </summary>
    /// <remarks>This service is designed to interact with a repository to perform operations on sales order
    /// details. It supports asynchronous operations and handles the conversion between domain entities and
    /// DTOs.</remarks>
    /// <param name="repository"></param>
    public class SalesOrderDetailService(IRepository<SalesOrderDetail, string> repository) : ISalesOrderDetailService
    {
        /// <summary>
        /// Retrieves the details of a sales order based on the specified sales order ID.
        /// </summary>
        /// <remarks>This method queries the repository for sales order details matching the specified
        /// sales order ID.  The result includes a collection of <see cref="SalesOrderDetailDto"/> objects representing
        /// the details  of the sales order. If the operation fails, the result will include error messages describing
        /// the failure.</remarks>
        /// <param name="salesOrderId">The unique identifier of the sales order whose details are to be retrieved.  This parameter cannot be <see
        /// langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of  <see cref="SalesOrderDetailDto"/> objects. If the
        /// operation succeeds, the result contains the  sales order details; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<SalesOrderDetailDto>>> GetSalesOrderDetailsAsync(string salesOrderId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<SalesOrderDetail>(c => c.SalesOrderId == salesOrderId);
            var result = await repository.ListAsync(spec, false, cancellationToken);
            if(!result.Succeeded) return await Result<IEnumerable<SalesOrderDetailDto>>.FailAsync(result.Messages);
            return await Result<IEnumerable<SalesOrderDetailDto>>.SuccessAsync(result.Data.Select(c => new SalesOrderDetailDto(c)));
        }

        /// <summary>
        /// Creates a new sales order detail asynchronously.
        /// </summary>
        /// <remarks>This method performs the following steps: 1. Creates a new sales order detail using
        /// the provided data. 2. Saves the changes to the repository. If any step fails, the method returns a failure
        /// result with the corresponding error messages.</remarks>
        /// <param name="salesOrderDetail">The sales order detail to create. This parameter must contain valid data for the sales order detail.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is <see cref="SalesOrderDetailDto"/>. If the operation succeeds, the result contains the
        /// created sales order detail. If the operation fails, the result contains error messages.</returns>
        public async Task<IBaseResult> CreateSalesOrderDetailAsync(SalesOrderDetailDto salesOrderDetail, CancellationToken cancellationToken = default)
        {
            var result = await repository.CreateAsync(new SalesOrderDetail(salesOrderDetail), cancellationToken);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);
            
            var saveResult = await repository.SaveAsync(cancellationToken);
            if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
            
            return await Result.SuccessAsync("Detail successfully added");
        }
    }
}
