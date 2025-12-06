// ReSharper disable MustUseReturnValue

using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AccomodationModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing reviews.
    /// Provides endpoints for CRUD operations on reviews.
    /// </summary>
    [Route("api/vacationContactUsInfos"), ApiController]
    public class VacationContactUsInfoController(IVacationContactUsInfoService service, IConfiguration configuration) : ControllerBase
    {
        #region Get Operations

        /// <summary>
        /// Retrieves a paginated list of vacation hosts based on the specified request parameters.
        /// </summary>
        /// <remarks>This method uses the provided pagination parameters to query and return a subset of
        /// vacation hosts. The result is returned as an HTTP response with a status code of 200 (OK) if
        /// successful.</remarks>
        /// <param name="pageParameters">The parameters used to define pagination settings, such as page number and page size.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of vacation hosts.</returns>
        [HttpGet] public async Task<IActionResult> PagedVacationHostsAsync([FromQuery] RequestParameters pageParameters)
        {
            var result = await service.PagedVacationContactUsInfoListAsync(pageParameters, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all templates.
        /// </summary>
        [HttpGet("all")] public async Task<IActionResult> GetAllAsync()
        {
            var result = await service.VacationContactUsInfoListAsync(HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a template by id.
        /// </summary>
        [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await service.VacationContactUsInfoAsync(id, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Crud Operations

        /// <summary>
        /// Creates a new template.
        /// </summary>
        [HttpPut] public async Task<IActionResult> CreateAsync(VacationContactUsInfoDto dto)
        {
            var result = await service.CreateVacationContactUsInfoAsync(dto, HttpContext.RequestAborted);

            if (result.Succeeded)
            {
                
            }


            return Ok(result);
        }

        /// <summary>
        /// Updates an existing template.
        /// </summary>
        [HttpPost] public async Task<IActionResult> UpdateAsync(VacationContactUsInfoDto dto)
        {
            var result = await service.UpdateVacationContactUsInfoAsync(dto, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Removes a template.
        /// </summary>
        [HttpDelete("{id}")] public async Task<IActionResult> RemoveAsync(string id)
        {
            var result = await service.RemoveVacationContactUsInfoAsync(id, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion
    }
}
