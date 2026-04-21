using System.Windows.Input;
using CLS.Module.Learner.Application.DTOs;
using CLS.Module.Learner.Application.Interfaces;
using CLS.Module.Learner.Domain.Entities;
using MediatR;

namespace CLS.Module.Learner.Application.Commands;

public record EnrollNewLearnerCommand(EnrollLearnerRequestDto Request) : IRequest<Guid>;

public class EnrollNewLearnerCommandHandler : IRequestHandler<EnrollNewLearnerCommand, Guid>
{
    private readonly ILearnerRepository _repository;

    public EnrollNewLearnerCommandHandler(ILearnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(EnrollNewLearnerCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Request;

        // Domain validation would go here

        var learner = new Domain.Entities.Learner(
            dto.FullName,
            dto.ParentEmail,
            dto.ParentPhone,
            dto.InitialLearningPackageId,
            dto.InitialSessions
        );

        await _repository.AddAsync(learner, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        // Here we could publish a domain event: new LearnerCreatedEvent(learner.Id)
        // for Module.Notification to send an email to the Parent (UC-03 requirement)

        return learner.Id;
    }
}
