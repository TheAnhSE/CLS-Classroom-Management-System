using CLS.BackendAPI.Data;
using CLS.BackendAPI.Models;
using CLS.BackendAPI.Models.DTOs;
using CLS.BackendAPI.Models.Entities;
using CLS.BackendAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CLS.BackendAPI.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly ApplicationDbContext _dbContext;

        public LearnerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<LearnerDto>> GetAllLearnersAsync()
        {
            var learners = await _dbContext.Learners.ToListAsync();
            return learners.Select(MapToDto);
        }

        public async Task<LearnerDto> GetLearnerByIdAsync(Guid id)
        {
            var learner = await _dbContext.Learners.FindAsync(id);
            if (learner == null)
            {
                throw new NotFoundException($"Learner with ID {id} was not found.");
            }
            return MapToDto(learner);
        }

        public async Task<LearnerDto> CreateLearnerAsync(CreateLearnerRequest request)
        {
            var existing = await _dbContext.Learners.AnyAsync(l => l.Email == request.Email);
            if (existing)
            {
                throw new ConflictException("Email already exists.");
            }

            var newLearner = new Learner
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Learners.Add(newLearner);
            await _dbContext.SaveChangesAsync();

            return MapToDto(newLearner);
        }

        public async Task<LearnerDto> UpdateLearnerAsync(Guid id, UpdateLearnerRequest request)
        {
            var learner = await _dbContext.Learners.FindAsync(id);
            if (learner == null)
            {
                throw new NotFoundException($"Learner with ID {id} was not found.");
            }

            learner.FullName = request.FullName;
            learner.PhoneNumber = request.PhoneNumber;
            learner.Status = request.Status;
            learner.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return MapToDto(learner);
        }

        public async Task DeleteLearnerAsync(Guid id)
        {
            var learner = await _dbContext.Learners.FindAsync(id);
            if (learner == null)
            {
                throw new NotFoundException($"Learner with ID {id} was not found.");
            }

            _dbContext.Learners.Remove(learner);
            await _dbContext.SaveChangesAsync();
        }

        private static LearnerDto MapToDto(Learner learner)
        {
            return new LearnerDto
            {
                Id = learner.Id,
                FullName = learner.FullName,
                Email = learner.Email,
                PhoneNumber = learner.PhoneNumber,
                Status = learner.Status.ToString()
            };
        }
    }
}
