using CLS.BackendAPI.Models.DTOs.Common;
using CLS.BackendAPI.Models.DTOs.Learners;
using CLS.BackendAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLS.BackendAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class LearnersController : ControllerBase
    {
        private readonly ILearnerService _learnerService;

        public LearnersController(ILearnerService learnerService)
        {
            _learnerService = learnerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLearners([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var pagedResponse = await _learnerService.GetAllLearnersAsync(pageNumber, pageSize);
            return Ok(pagedResponse); // PagedResponse inherits from ApiResponse
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLearnerById(int id)
        {
            var learner = await _learnerService.GetLearnerByIdAsync(id);
            return Ok(ApiResponse<LearnerDto>.Success(learner));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLearner([FromBody] CreateLearnerRequest request)
        {
            var learner = await _learnerService.CreateLearnerAsync(request);
            return CreatedAtAction(nameof(GetLearnerById), new { id = learner.LearnerId }, ApiResponse<LearnerDto>.Created(learner));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLearner(int id, [FromBody] UpdateLearnerRequest request)
        {
            var learner = await _learnerService.UpdateLearnerAsync(id, request);
            return Ok(ApiResponse<LearnerDto>.Success(learner, "Cập nhật học viên thành công"));
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateLearner(int id)
        {
            await _learnerService.DeactivateLearnerAsync(id);
            return Ok(ApiResponse<object>.Success(null, "Thao tác thành công")); // MSG-LRN-101
        }
    }
}
