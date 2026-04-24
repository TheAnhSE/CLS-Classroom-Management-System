using CLS.BackendAPI.Exceptions;
using CLS.BackendAPI.Models;
using CLS.BackendAPI.Models.DTOs.Common;
using CLS.BackendAPI.Models.DTOs.Learners;
using CLS.BackendAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CLS.BackendAPI.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly ClsDbContext _context;

        public LearnerService(ClsDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<LearnerDto>> GetAllLearnersAsync(int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Learners
                .Include(l => l.Parent)
                .AsNoTracking();

            var totalRecords = await query.CountAsync();

            var learners = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = learners.Select(MapToDto).ToList();

            return new PagedResponse<LearnerDto>(dtos, pageNumber, pageSize, totalRecords);
        }

        public async Task<LearnerDto> GetLearnerByIdAsync(int id)
        {
            var learner = await _context.Learners
                .Include(l => l.Parent)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.LearnerId == id);

            if (learner == null)
            {
                throw new NotFoundException($"Học viên không tồn tại (ID: {id})");
            }

            return MapToDto(learner);
        }

        public async Task<LearnerDto> CreateLearnerAsync(CreateLearnerRequest request)
        {
            // Check if Parent Email already exists
            var parent = await _context.Parents.FirstOrDefaultAsync(p => p.Email == request.ParentEmail);

            if (parent == null)
            {
                parent = new Parent
                {
                    FullName = request.ParentFullName,
                    Email = request.ParentEmail,
                    PhoneNumber = request.ParentPhoneNumber,
                    Address = request.ParentAddress,
                    Relationship = request.RelationshipToLearner,
                    CreatedTime = DateTime.UtcNow
                };
            }

            var learner = new Learner
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                EnrollmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Status = "Active",
                Notes = request.Notes,
                CreatedTime = DateTime.UtcNow,
                CreatedBy = 1, // System or Admin ID
                Parent = parent // Entity Framework will handle attaching or inserting
            };

            _context.Learners.Add(learner);
            await _context.SaveChangesAsync();

            return await GetLearnerByIdAsync(learner.LearnerId);
        }

        public async Task<LearnerDto> UpdateLearnerAsync(int id, UpdateLearnerRequest request)
        {
            var learner = await _context.Learners.FindAsync(id);
            if (learner == null)
            {
                throw new NotFoundException($"Học viên không tồn tại (ID: {id})");
            }

            learner.FirstName = request.FirstName;
            learner.LastName = request.LastName;
            learner.DateOfBirth = request.DateOfBirth;
            learner.Gender = request.Gender ?? learner.Gender;
            learner.Notes = request.Notes;
            learner.UpdatedTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetLearnerByIdAsync(id);
        }

        public async Task DeactivateLearnerAsync(int id)
        {
            var learner = await _context.Learners
                .Include(l => l.LearnerPackages)
                .FirstOrDefaultAsync(l => l.LearnerId == id);

            if (learner == null)
            {
                throw new NotFoundException($"Học viên không tồn tại (ID: {id})");
            }

            // Check BR-LRN-04: Cannot deactivate if active package has remaining sessions
            var hasActivePackage = learner.LearnerPackages.Any(lp => lp.Status == "Active" && lp.RemainingSessions > 0);
            if (hasActivePackage)
            {
                throw new ConflictException("Cần tất toán gói học phí trước khi vô hiệu hóa.");
            }

            learner.Status = "Inactive";
            learner.UpdatedTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        private static LearnerDto MapToDto(Learner learner)
        {
            return new LearnerDto
            {
                LearnerId = learner.LearnerId,
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                DateOfBirth = learner.DateOfBirth,
                Gender = learner.Gender,
                EnrollmentDate = learner.EnrollmentDate,
                Status = learner.Status ?? string.Empty,
                Notes = learner.Notes,
                ParentId = learner.ParentId,
                Parent = learner.Parent != null ? new ParentDto
                {
                    ParentId = learner.Parent.ParentId,
                    FullName = learner.Parent.FullName,
                    Email = learner.Parent.Email,
                    PhoneNumber = learner.Parent.PhoneNumber,
                    Address = learner.Parent.Address,
                    Relationship = learner.Parent.Relationship
                } : null
            };
        }
    }
}
