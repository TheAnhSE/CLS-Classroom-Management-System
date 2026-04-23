using CLS.BackendAPI.Models.DTOs;
using CLS.BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLS.BackendAPI.Controllers
{
    [ApiController]
    [Route("api/v1/learners")]
    public class LearnerController : ControllerBase
    {
        private readonly ILearnerService _learnerService;

        public LearnerController(ILearnerService learnerService)
        {
            _learnerService = learnerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var learners = await _learnerService.GetAllLearnersAsync();
            return Ok(learners);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var learner = await _learnerService.GetLearnerByIdAsync(id);
            return Ok(learner);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLearnerRequest request)
        {
            var learner = await _learnerService.CreateLearnerAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = learner.Id }, learner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLearnerRequest request)
        {
            var learner = await _learnerService.UpdateLearnerAsync(id, request);
            return Ok(learner);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _learnerService.DeleteLearnerAsync(id);
            return NoContent();
        }
    }
}
