using CLS.BackendAPI.Models;
using CLS.BackendAPI.Models.DTOs.MasterData;
using Microsoft.EntityFrameworkCore;

namespace CLS.BackendAPI.Services
{
    public class MasterDataService : IMasterDataService
    {
        private readonly ClsDbContext _context;

        public MasterDataService(ClsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DropdownItemDto>> GetActiveTeachersAsync()
        {
            return await _context.Users
                .Where(u => u.RoleId == 3 && u.IsActive == true) // Assuming RoleId 3 is Teacher, or we can check Role.RoleName == "Teacher"
                .Select(u => new DropdownItemDto
                {
                    Id = u.UserId,
                    Name = $"{u.FirstName} {u.LastName}"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<DropdownItemDto>> GetActiveClassroomsAsync()
        {
            return await _context.Classrooms
                .Where(c => c.IsActive == true)
                .Select(c => new DropdownItemDto
                {
                    Id = c.ClassroomId,
                    Name = c.RoomName
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<DropdownItemDto>> GetActiveSubjectsAsync()
        {
            return await _context.Subjects
                .Where(s => s.IsActive == true)
                .Select(s => new DropdownItemDto
                {
                    Id = s.SubjectId,
                    Name = s.SubjectName
                })
                .ToListAsync();
        }
    }
}
