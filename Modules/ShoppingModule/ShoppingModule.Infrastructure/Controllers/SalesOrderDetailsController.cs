using Microsoft.AspNetCore.Mvc;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Interfaces;

namespace ShoppingModule.Infrastructure.Controllers;

/// <summary>
/// The api controller for sales order details
/// </summary>
[Route("api/salesorders/details")]
[ApiController]
public class SalesOrderDetailsController(ISalesOrderDetailService salesOrderDetailService) : ControllerBase
{

    /// <summary>
    /// Gets the sales orders from the data source 
    /// </summary>
    /// <param name="salesOrderId">The identity of the sales order contianing the sales order details</param>
    /// <returns>A http ok status code with the serialized sales order detail data transfer object list</returns>
    [HttpGet("{salesOrderId}", Name = "GetSalesOrderDetailsAsync")]
    public async Task<IActionResult> GetSalesOrderDetailsAsync(string salesOrderId)
    {
        var salesOrderDetails = await salesOrderDetailService.GetSalesOrderDetailsAsync(salesOrderId);
        return Ok(salesOrderDetails);
    }

    /// <summary>
    /// Adds a new sales order detail to a specific sales order
    /// </summary>
    /// <param name="salesOrderDetail">The data transfer object that contains the information for creating the sales order detail</param>
    /// <returns>A http ok status code with the serialized sales order detail data transfer object</returns>
    [HttpPost]
    public async Task<IActionResult> CreateSalesOrderDetail([FromBody] SalesOrderDetailDto salesOrderDetail)
    {
        if (salesOrderDetail is null)
            return BadRequest("SalesOrderDetailDto object is null");

        var detail = await salesOrderDetailService.CreateSalesOrderDetailAsync(salesOrderDetail);

        return Ok(detail);
    }
}
