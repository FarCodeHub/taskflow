using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Tasks.Application.TaskItems.CreateTask;

namespace TaskFlow.Tasks.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    private readonly ISender _sender;

    public TasksController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Creates a new task.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var taskId = await _sender.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(Create),
            new { id = taskId },
            taskId);
    }
}