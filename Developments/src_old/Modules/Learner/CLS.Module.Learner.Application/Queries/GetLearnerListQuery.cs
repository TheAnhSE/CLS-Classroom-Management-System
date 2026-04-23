using CLS.Common.Pagination;
using CLS.Module.Learner.Application.DTOs;
using CLS.Module.Learner.Application.Interfaces;
using Mapster;
using MediatR;

namespace CLS.Module.Learner.Application.Queries;

public record GetLearnerListQuery(int PageIndex, int PageSize) : IRequest<PaginatedList<LearnerResponseDto>>;

public class GetLearnerListQueryHandler : IRequestHandler<GetLearnerListQuery, PaginatedList<LearnerResponseDto>>
{
    private readonly ILearnerRepository _repository;

    public GetLearnerListQueryHandler(ILearnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedList<LearnerResponseDto>> Handle(GetLearnerListQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetListAsync(request.PageIndex, request.PageSize, cancellationToken);
        var totalCount = await _repository.GetTotalCountAsync(cancellationToken);

        var dtos = entities.Adapt<List<LearnerResponseDto>>();
        
        return new PaginatedList<LearnerResponseDto>(dtos, totalCount, request.PageIndex, request.PageSize);
    }
}
