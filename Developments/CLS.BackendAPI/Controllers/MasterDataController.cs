using CLS.BackendAPI.Models.DTOs.Common;
using CLS.BackendAPI.Models.DTOs.MasterData;
using CLS.BackendAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLS.BackendAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;

        public MasterDataController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var data = await _masterDataService.GetActiveTeachersAsync();
            return Ok(ApiResponse<IEnumerable<DropdownItemDto>>.Success(data));
        }

        [HttpGet("classrooms")]
        public async Task<IActionResult> GetClassrooms()
        {
            var data = await _masterDataService.GetActiveClassroomsAsync();
            return Ok(ApiResponse<IEnumerable<DropdownItemDto>>.Success(data));
        }

        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var data = await _masterDataService.GetActiveSubjectsAsync();
            return Ok(ApiResponse<IEnumerable<DropdownItemDto>>.Success(data));
        }
    }
}
