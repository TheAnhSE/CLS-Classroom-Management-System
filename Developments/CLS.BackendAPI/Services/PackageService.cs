using CLS.BackendAPI.Exceptions;
using CLS.BackendAPI.Models;
using CLS.BackendAPI.Models.DTOs.Packages;
using CLS.BackendAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CLS.BackendAPI.Services
{
    public class PackageService : IPackageService
    {
        private readonly ClsDbContext _context;

        public PackageService(ClsDbContext context)
        {
            _context = context;
        }

        public async Task<LearningPackageDto> CreatePackageAsync(CreatePackageRequest request)
        {
            var subjectExists = await _context.Subjects.AnyAsync(s => s.SubjectId == request.SubjectId);
            if (!subjectExists)
            {
                throw new ValidationException($"SubjectId {request.SubjectId} không tồn tại.");
            }

            var package = new LearningPackage
            {
                PackageName = request.PackageName,
                SubjectId = request.SubjectId,
                TotalSessions = request.TotalSessions,
                DurationMonths = request.DurationMonths,
                TuitionFee = request.TuitionFee,
                Description = request.Description,
                IsActive = true,
                CreatedTime = DateTime.UtcNow,
                CreatedBy = 1 // System/Admin User ID
            };

            _context.LearningPackages.Add(package);
            await _context.SaveChangesAsync();

            return MapToDto(package);
        }

        public async Task<IEnumerable<LearningPackageDto>> GetActivePackagesAsync()
        {
            var packages = await _context.LearningPackages
                .Where(p => p.IsActive == true)
                .AsNoTracking()
                .ToListAsync();

            return packages.Select(MapToDto);
        }

        public async Task<LearnerPackageDto> AssignPackageToLearnerAsync(AssignPackageRequest request)
        {
            // 1. Check Learner exists
            var learner = await _context.Learners.FindAsync(request.LearnerId);
            if (learner == null)
            {
                throw new NotFoundException($"Học viên (ID: {request.LearnerId}) không tồn tại.");
            }

            // 2. Check Package exists and is active
            var package = await _context.LearningPackages
                .Include(p => p.Subject)
                .FirstOrDefaultAsync(p => p.PackageId == request.PackageId && p.IsActive == true);

            if (package == null)
            {
                throw new NotFoundException($"Gói học phí (ID: {request.PackageId}) không tồn tại hoặc đã bị vô hiệu hóa.");
            }

            // 3. BR-PKG-03 & MSG-PKG-400: Check overlap
            // Prevent assigning if the learner already has an active package for the same Subject
            var existingOverlap = await _context.LearnerPackages
                .Include(lp => lp.Package)
                .Where(lp => lp.LearnerId == request.LearnerId && 
                             lp.Status == "Active" && 
                             lp.RemainingSessions > 0 &&
                             lp.Package.SubjectId == package.SubjectId)
                .AnyAsync();

            if (existingOverlap)
            {
                throw new ConflictException("Học viên đang có gói học cùng môn học này chưa dùng hết.");
            }

            var assignedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var expiryDate = assignedDate.AddMonths(package.DurationMonths > 0 ? package.DurationMonths : 12); // Default to 12 months if not specified

            var learnerPackage = new LearnerPackage
            {
                LearnerId = request.LearnerId,
                PackageId = request.PackageId,
                AssignedDate = assignedDate,
                ExpiryDate = expiryDate,
                TotalSessions = package.TotalSessions,
                RemainingSessions = package.TotalSessions,
                Status = "Active",
                AssignedBy = 1,
                CreatedTime = DateTime.UtcNow
            };

            _context.LearnerPackages.Add(learnerPackage);
            await _context.SaveChangesAsync();

            learnerPackage.Package = package; // For DTO Mapping
            
            return MapToLearnerPackageDto(learnerPackage);
        }

        public async Task<LearnerPackageDto> AdjustLearnerPackageBalanceAsync(int learnerPackageId, AdjustBalanceRequest request)
        {
            var learnerPackage = await _context.LearnerPackages
                .Include(lp => lp.Package)
                .FirstOrDefaultAsync(lp => lp.LearnerPackageId == learnerPackageId);

            if (learnerPackage == null)
            {
                throw new NotFoundException($"Gói của học viên (ID: {learnerPackageId}) không tồn tại.");
            }

            learnerPackage.RemainingSessions += request.DeltaSessions;
            
            if (learnerPackage.RemainingSessions < 0)
            {
                learnerPackage.RemainingSessions = 0;
            }

            learnerPackage.UpdatedTime = DateTime.UtcNow;

            // TODO: Log the Reason to ActivityLog

            await _context.SaveChangesAsync();

            return MapToLearnerPackageDto(learnerPackage);
        }

        private static LearningPackageDto MapToDto(LearningPackage package)
        {
            return new LearningPackageDto
            {
                PackageId = package.PackageId,
                PackageName = package.PackageName,
                SubjectId = package.SubjectId,
                TotalSessions = package.TotalSessions,
                DurationMonths = package.DurationMonths,
                TuitionFee = package.TuitionFee,
                Description = package.Description,
                IsActive = package.IsActive ?? false
            };
        }

        private static LearnerPackageDto MapToLearnerPackageDto(LearnerPackage lp)
        {
            return new LearnerPackageDto
            {
                LearnerPackageId = lp.LearnerPackageId,
                LearnerId = lp.LearnerId,
                PackageId = lp.PackageId,
                PackageName = lp.Package != null ? lp.Package.PackageName : string.Empty,
                SubjectName = lp.Package != null && lp.Package.Subject != null ? lp.Package.Subject.SubjectName : string.Empty,
                AssignedDate = lp.AssignedDate,
                ExpiryDate = lp.ExpiryDate,
                TotalSessions = lp.TotalSessions,
                RemainingSessions = lp.RemainingSessions,
                Status = lp.Status ?? string.Empty
            };
        }
    }
}
