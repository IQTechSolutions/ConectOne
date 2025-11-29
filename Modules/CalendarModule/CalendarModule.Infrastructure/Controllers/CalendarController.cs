using CalendarModule.Domain.DataTransferObjects;
using CalendarModule.Domain.Entities;
using CalendarModule.Domain.Interfaces;
using CalendarModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarModule.Infrastructure.Controllers
{
    /// <summary>
    /// API controller for managing calendar entries.
    /// Provides endpoints for retrieving, creating, updating, and deleting calendar entries.
    /// </summary>
    [Route("api/calendar"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class CalendarController(IAppointmentService service) : ControllerBase
    {
        #region Get Operations

        /// <summary>
        /// Retrieves all calendar entries based on the provided request parameters.
        /// </summary>
        /// <param name="requestParameters">The parameters for pagination and filtering.</param>
        /// <returns>A list of calendar entries.</returns>
        [HttpGet] public async Task<IActionResult> GetAllAsync([FromQuery] CalendarPageParameters requestParameters)
        {
            var result = await service.GetAllAsync(requestParameters, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a calendar entry by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the calendar entry.</param>
        /// <returns>The calendar entry with the specified ID.</returns>
        [HttpGet("details/{id}")] public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await service.GetByIdAsync(id, HttpContext.RequestAborted);
            return Ok(result);
        }

        #endregion

        #region Crud Operations

        /// <summary>
        /// Creates a new calendar entry.
        /// </summary>
        /// <param name="appointment">The calendar entry to create.</param>
        /// <returns>The created calendar entry.</returns>
        [HttpPut] public async Task<IActionResult> CreateAsync(CalendarEntryDto appointment)
        {
            var result = await service.AddAsync(appointment, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing calendar entry.
        /// </summary>
        /// <param name="supportTicket">The calendar entry to update.</param>
        /// <returns>The updated calendar entry.</returns>
        [HttpPost] public async Task<IActionResult> UpdateAsync(CalendarEntryDto supportTicket)
        {
            var result = await service.EditAsync(supportTicket, HttpContext.RequestAborted);
            return Ok(result);

            Appointment UpdateAction(Appointment appointment, CalendarEntryDto dto)
            {
                appointment.Heading = dto.Name;
                appointment.StartDate = dto.StartDate.Value;
                appointment.StartTime = dto.StartTime.Value;
                appointment.EndDate = dto.EndDate.Value;
                appointment.EndTime = dto.EndTime.Value;
                appointment.FullDayEvent = dto.FullDayEvent;

                return appointment;
            }
        }

        /// <summary>
        /// Deletes a calendar entry by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the calendar entry to delete.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        [HttpDelete("{id}")] public async Task<IActionResult> RemoveAsync(string id)
        {
            var result = await service.DeleteAsync(id);
            return Ok(result);
        }

        #endregion
    }
}
