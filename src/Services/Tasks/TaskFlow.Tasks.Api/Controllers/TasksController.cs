using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Tasks.Application.TaskItems.CreateTask;

namespace TaskFlow.Tasks.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    // private readonly IMediator _mediator;

    // public TasksController(IMediator mediator)
    // {
    //     _mediator = mediator;
    // }

    // /// <summary>
    // /// Creates a new task.
    // /// </summary>
    // [HttpPost]
    // public async Task<IActionResult> Create(
    //     [FromBody] CreateTaskCommand command,
    //     CancellationToken cancellationToken)
    // {
    //     var taskId = await _mediator.Send(command, cancellationToken);

    //     return CreatedAtAction(
    //         nameof(Create),
    //         new { id = taskId },
    //         taskId);
    // }
}