using MediatR;
using TaskFlow.SharedKernel.Time;
using TaskFlow.Tasks.Application.Abstractions.Data;
using TaskFlow.Tasks.Domain.TaskItems;

namespace TaskFlow.Tasks.Application.TaskItems.CreateTask;

/// <summary>
/// Handles the create task use case.
/// The handler coordinates domain behavior and persistence abstractions.
/// </summary>
public sealed class CreateTaskCommandHandler
    : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateTaskCommandHandler(
        ITaskItemRepository taskItemRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _taskItemRepository = taskItemRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Guid> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var taskItem = TaskItem.Create(
            request.Title,
            request.Description,
            request.CreatedByUserId,
            request.DueDate,
            _dateTimeProvider);

        await _taskItemRepository.AddAsync(taskItem, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return taskItem.Id;
    }
}