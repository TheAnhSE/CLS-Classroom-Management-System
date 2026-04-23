using CLS.BackendAPI.Models.DTOs;

namespace CLS.BackendAPI.Services
{
    public interface ILearnerService
    {
        Task<IEnumerable<LearnerDto>> GetAllLearnersAsync();
        Task<LearnerDto> GetLearnerByIdAsync(Guid id);
        Task<LearnerDto> CreateLearnerAsync(CreateLearnerRequest request);
        Task<LearnerDto> UpdateLearnerAsync(Guid id, UpdateLearnerRequest request);
        Task DeleteLearnerAsync(Guid id);
    }
}
