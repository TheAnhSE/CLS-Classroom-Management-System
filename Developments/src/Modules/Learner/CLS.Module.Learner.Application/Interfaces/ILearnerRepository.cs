using CLS.Module.Learner.Domain.Entities;

namespace CLS.Module.Learner.Application.Interfaces;

public interface ILearnerRepository
{
    Task<Domain.Entities.Learner> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Domain.Entities.Learner>> GetListAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    Task AddAsync(Domain.Entities.Learner learner, CancellationToken cancellationToken);
    Task UpdateAsync(Domain.Entities.Learner learner, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
