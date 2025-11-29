using ConectOne.Domain.ResultWrappers;
using ConectOne.Infrastructure.Controllers;
using ConectOne.Infrastructure.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

// For Result<T>, PaginatedResult<T> wrappers

// For ILearnerService interface
// For LearnerDto, ParentDto, etc.
// For SaveImportedLearnersRequest

// For LearnerPageParameters

namespace SchoolsModule.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for handling learner-related HTTP requests.
    /// Provides endpoints for fetching learners, paging, filtering, creating, updating, deleting,
    /// managing learner parents, and importing data.
    /// </summary>
    [Route("api/learners"), ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LearnerController : BaseContactInfoApiController<Learner>
    {
        private readonly ILearnerQueryService _learnerQueryService;
        private readonly ILearnerCommandService _learnerCommandService;
        private readonly ILearnerNotificationService _learnerNotificationService;
        private readonly ILearnerExportService _learnerExportService;
        private readonly IActivityGroupQueryService _activityGroupQueryService;
        private readonly IActivityGroupCommandService _activityGroupCommandService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerController"/> class with injected services.
        /// </summary>
        /// <param name="learnerQueryService">
        /// Service for querying learner data, such as retrieval by ID, paginated listings, or email-based lookups.
        /// </param>
        /// <param name="learnerCommandService">
        /// Service for executing commands on learners, including creation, updates, and deletions.
        /// </param>
        /// <param name="learnerNotificationService">
        /// Service responsible for generating learner-based recipient lists for notifications or mailings.
        /// </param>
        /// <param name="learnerExportService">
        /// Service handling learner data imports and batch grade/class assignments from external sources.
        /// </param>
        /// <param name="service">
        /// Generic contact information service, likely responsible for managing phone and email info for <see cref="Learner"/> entities.
        /// </param>
        public LearnerController(ILearnerQueryService learnerQueryService, ILearnerCommandService learnerCommandService, ILearnerNotificationService learnerNotificationService, 
            ILearnerExportService learnerExportService, IContactInfoService<Learner> service, IActivityGroupQueryService activityGroupQueryService, IActivityGroupCommandService activityGroupCommandService) : base(service)
        {
            _learnerQueryService = learnerQueryService;
            _learnerCommandService = learnerCommandService;
            _learnerNotificationService = learnerNotificationService;
            _learnerExportService = learnerExportService;
            _activityGroupQueryService = activityGroupQueryService;
            _activityGroupCommandService=activityGroupCommandService;
        }

        /// <summary>
        /// Returns all learners without paging or filtering.
        /// Converts the returned learners into LearnerDto objects.
        /// </summary>
        [HttpGet] public async Task<IActionResult> AllLearners([FromQuery] LearnerPageParameters pageParameters)
        {
            var result = await _learnerQueryService.AllLearnersAsync(pageParameters);
            if (!result.Succeeded) return Ok(result);

            var dtos = result.Data.ToList();
            return Ok(await Result<IEnumerable<LearnerDto>>.SuccessAsync(dtos));
        }

        /// <summary>
        /// Returns a paged list of learners based on filtering and search criteria passed via LearnerPageParameters.
        /// </summary>
        /// <param name="pageParameters">The paging parameters</param>
        [HttpGet("notificationList")] public async Task<IActionResult> LearnersNotificationList([FromQuery] LearnerPageParameters pageParameters)
        {
            var result = await _learnerNotificationService.LearnersNotificationList(pageParameters);
            if (result.Succeeded)
            {
                return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            }
            return Ok(result);
        }

        /// <summary>
        /// Returns a list of all learners who can receive global notifications.
        /// </summary>
        [HttpGet("notificationList/global")]
        public async Task<IActionResult> GlobalNotificationList()
        {
            var result = await _learnerNotificationService.GlobalMailRecipientList();
            if (result.Succeeded)
            {
                return Ok(await Result<IEnumerable<RecipientDto>>.SuccessAsync(result.Data));
            }
            return Ok(result);
        }

        /// <summary>
        /// Returns a paged list of learners based on filtering and search criteria passed via LearnerPageParameters.
        /// </summary>
        /// <param name="pageParameters">Filtering, paging, and search criteria</param>
        [HttpGet("pagedlearners")]
        public async Task<IActionResult> PagedLearnersAsync([FromQuery] LearnerPageParameters pageParameters)

        {
            var result = await _learnerQueryService.PagedLearnersAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Returns the total count of learners in the system.
        /// </summary>
        [HttpGet("count")]
        public async Task<IActionResult> ParentCount()
        {
            var result = await _learnerQueryService.LearnerCount();
            return Ok(result);
        }

        /// <summary>
        /// Returns detailed info about a single learner by learnerId.
        /// </summary>
        [HttpGet("{learnerId}")]
        public async Task<IActionResult> LearnerAsync(string learnerId)
        {
            var result = await _learnerQueryService.LearnerAsync(learnerId);
            return Ok(result);
        }

        /// <summary>
        /// Returns learner information by a given email address.
        /// Useful if you need to find a learner based on their contact email.
        /// </summary>
        [HttpGet("byemail/{emailAddress}")]
        public async Task<IActionResult> LearnerByEmailAsync(string emailAddress)
        {
            var result = await _learnerQueryService.LearnerByEmailAsync(emailAddress);
            return Ok(result);
        }

        [HttpGet("exist/{emailAddress}"), AllowAnonymous]
        public async Task<IActionResult> LearnerExits(string emailAddress)
        {
            var result = await _learnerQueryService.LearnerExist(emailAddress);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new learner using the data provided in the LearnerDto.
        /// Expects a LearnerDto with necessary fields.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> CreateLearnerAsync([FromBody] LearnerDto learner)
        {
            var result = await _learnerCommandService.CreateAsync(learner);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing learner's information.
        /// Expects a LearnerDto with the updated data, including LearnerId to identify the learner.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateLearnerAsync([FromBody] LearnerDto learner)
        {
            var result = await _learnerCommandService.UpdateAsync(learner);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a learner by learnerId, as well as their associated notifications and messages if any.
        /// </summary>
        [HttpDelete("{learnerId}")]
        public async Task<IActionResult> DeleteAsync(string learnerId)
        {
            var result = await _learnerCommandService.RemoveAsync(learnerId);
            return Ok(result);
        }

        /// <summary>
        /// Returns a paged list of learners associated with a specific activity group.
        /// Requires LearnerPageParameters including ActivityGroupId.
        /// </summary>
        [HttpGet("pagedactivitygrouplearners")]
        public async Task<IActionResult> PagedActivityGroupLearnersAsync([FromQuery] LearnerPageParameters pageParameters)
        {
            var result = await _activityGroupQueryService.PagedActivityGroupTeamMembersAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Adds a learner to a specific activity group team.
        /// Requires activityGroupId and learnerId as route parameters.
        /// </summary>
        [HttpPut("{activityGroupId}/activitygrouplearners/{learnerId}")]
        public async Task<IActionResult> CreateActivityGroupTeamMemberAsync(string activityGroupId, string learnerId)
        {
            var result = await _activityGroupCommandService.CreateActivityGroupTeamMemberAsync(activityGroupId, learnerId);
            return Ok(result);
        }

        /// <summary>
        /// Removes a learner from a specific activity group team.
        /// Requires activityGroupId and learnerId as route parameters.
        /// </summary>
        [HttpDelete("{activityGroupId}/activitygrouplearners/{learnerId}")]
        public async Task<IActionResult> DeleteActivityGroupTeamMemberAsync(string activityGroupId, string learnerId)
        {
            var result = await _activityGroupCommandService.RemoveActivityGroupTeamMemberAsync(activityGroupId, learnerId);
            return Ok(result);
        }

        /// <summary>
        /// Returns a paged list of learners linked to a particular event's activity group.
        /// Requires LearnerPageParameters including EventId and possibly ActivityGroupId.
        /// </summary>
        [HttpGet("pagedeventactivitygrouplearners")]
        public async Task<IActionResult> PagedEventActivityGroupLearnersAsync([FromQuery] LearnerPageParameters pageParameters)
        {
            var result = await _activityGroupQueryService.PagedEventActivityGroupTeamMembersAsync(pageParameters);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all parents associated with a specific learner by learnerId.
        /// Returns a list of ParentDto with parent details.
        /// </summary>
        [HttpGet("learnerparents/{learnerId}")]
        public async Task<IActionResult> LearnerParentsAsync(string learnerId)
        {
            var result = await _learnerQueryService.LearnerParentsAsync(learnerId);
            return Ok(result);
        }

        /// <summary>
        /// Updates the parents of a given learner.
        /// Expects a list of ParentDto objects representing the new set of parents.
        /// Adds or removes parent relationships as needed.
        /// </summary>
        [HttpPost("learnerparents/{learnerId}")]
        public async Task<IActionResult> UpdateLearnerParentsAsync(string learnerId, [FromBody] List<ParentDto> parents)
        {
            var result = await _learnerCommandService.UpdateLearnerParentsAsync(learnerId, parents);
            return Ok(result);
        }

        /// <summary>
        /// Imports new learners and parents from a provided request object.
        /// Used for bulk importing learner data.
        /// </summary>
        [HttpPost("import/new")]
        public async Task<IActionResult> UpdateLearnerParentsAsync([FromBody] SaveImportedLearnersRequest request)
        {
            var result = await _learnerExportService.ImportNewLearnersAndParents(request);
            return Ok(result);
        }

        /// <summary>
        /// Imports new learners with grade-related data from a provided request object.
        /// Currently calls the same import method as "import/new", but could be extended for specific grade logic.
        /// </summary>
        [HttpPost("import/grades/byId")]
        public async Task<IActionResult> UpdateLearnerGradesByIdAsync([FromBody] SaveImportedLearnerGradesRequest request)
        {
            var result = await _learnerExportService.ImportNewLearnersGradesById(request);
            return Ok(result);
        }
    }
}
