using CLS.BackendAPI.Models.DTOs.MasterData;

namespace CLS.BackendAPI.Services
{
    public interface IMasterDataService
    {
        Task<IEnumerable<DropdownItemDto>> GetActiveTeachersAsync();
        Task<IEnumerable<DropdownItemDto>> GetActiveClassroomsAsync();
        Task<IEnumerable<DropdownItemDto>> GetActiveSubjectsAsync();
    }
}
