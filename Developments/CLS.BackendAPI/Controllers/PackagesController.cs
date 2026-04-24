using CLS.BackendAPI.Models.DTOs.Common;
using CLS.BackendAPI.Models.DTOs.Packages;
using CLS.BackendAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLS.BackendAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromBody] CreatePackageRequest request)
        {
            var package = await _packageService.CreatePackageAsync(request);
            return Ok(ApiResponse<LearningPackageDto>.Created(package, "Tạo gói học thành công")); // MSG-PKG-100
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePackages()
        {
            var packages = await _packageService.GetActivePackagesAsync();
            return Ok(ApiResponse<IEnumerable<LearningPackageDto>>.Success(packages));
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignPackageToLearner([FromBody] AssignPackageRequest request)
        {
            var learnerPackage = await _packageService.AssignPackageToLearnerAsync(request);
            return Ok(ApiResponse<LearnerPackageDto>.Success(learnerPackage, "Gán lượt học thành công")); // MSG-PKG-101
        }

        [HttpPut("learner-packages/{id}/adjust")]
        public async Task<IActionResult> AdjustLearnerPackageBalance(int id, [FromBody] AdjustBalanceRequest request)
        {
            var learnerPackage = await _packageService.AdjustLearnerPackageBalanceAsync(id, request);
            return Ok(ApiResponse<LearnerPackageDto>.Success(learnerPackage, "Điều chỉnh thành công")); // MSG-PKG-102
        }
    }
}
