using CLS.BackendAPI.Models.DTOs.Packages;

namespace CLS.BackendAPI.Services
{
    public interface IPackageService
    {
        Task<LearningPackageDto> CreatePackageAsync(CreatePackageRequest request);
        Task<IEnumerable<LearningPackageDto>> GetActivePackagesAsync();
        Task<LearnerPackageDto> AssignPackageToLearnerAsync(AssignPackageRequest request);
        Task<LearnerPackageDto> AdjustLearnerPackageBalanceAsync(int learnerPackageId, AdjustBalanceRequest request);
    }
}
