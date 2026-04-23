using CLS.Common;
using CLS.Module.Learner.Application.Commands;
using CLS.Module.Learner.Application.DTOs;
using CLS.Module.Learner.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CLS.Module.Learner.Presentation.Controllers;

[ApiController]
[Route("api/v1/learners")]
public class LearnersController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearnersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetLearnerListQuery(pageIndex, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(BaseResponse<object>.Success(result, "Lấy danh sách học viên thành công", 200));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnrollLearnerRequestDto request, CancellationToken cancellationToken)
    {
        var command = new EnrollNewLearnerCommand(request);
        var learnerId = await _mediator.Send(command, cancellationToken);
        
        var locationUri = $"/api/v1/learners/{learnerId}";
        return Created(locationUri, BaseResponse<object>.Success(new { Id = learnerId }, "Đăng ký học viên thành công", 201));
    }
}
