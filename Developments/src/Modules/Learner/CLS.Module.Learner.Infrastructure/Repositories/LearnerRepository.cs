using CLS.Module.Learner.Application.Interfaces;
using CLS.Module.Learner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CLS.Module.Learner.Infrastructure.Repositories;

public class LearnerRepository : ILearnerRepository
{
    private readonly LearnerDbContext _dbContext;

    public LearnerRepository(LearnerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Domain.Entities.Learner> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Learners.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Domain.Entities.Learner>> GetListAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        return await _dbContext.Learners
            .AsNoTracking()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Learners.CountAsync(cancellationToken);
    }

    public async Task AddAsync(Domain.Entities.Learner learner, CancellationToken cancellationToken)
    {
        await _dbContext.Learners.AddAsync(learner, cancellationToken);
    }

    public async Task UpdateAsync(Domain.Entities.Learner learner, CancellationToken cancellationToken)
    {
        _dbContext.Learners.Update(learner);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
