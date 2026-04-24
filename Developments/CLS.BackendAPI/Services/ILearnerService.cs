using CLS.BackendAPI.Models.DTOs.Learners;
using CLS.BackendAPI.Models.DTOs.Common;

namespace CLS.BackendAPI.Services
{
    public interface ILearnerService
    {
        Task<PagedResponse<LearnerDto>> GetAllLearnersAsync(int pageNumber = 1, int pageSize = 10);
        Task<LearnerDto> GetLearnerByIdAsync(int id);
        Task<LearnerDto> CreateLearnerAsync(CreateLearnerRequest request);
        Task<LearnerDto> UpdateLearnerAsync(int id, UpdateLearnerRequest request);
        Task DeactivateLearnerAsync(int id);
    }
}
