using FluentValidation;

namespace TaskFlow.Tasks.Application.TaskItems.CreateTask;

/// <summary>
/// Validates input data before the command reaches the domain model.
/// Domain rules still remain inside the aggregate.
/// </summary>
public sealed class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.CreatedByUserId)
            .NotEmpty();
    }
}