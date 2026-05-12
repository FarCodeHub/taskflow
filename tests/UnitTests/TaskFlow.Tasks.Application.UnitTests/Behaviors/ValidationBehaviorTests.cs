using FluentValidation;
using MediatR;
using NSubstitute;
using TaskFlow.SharedKernel.Validation;
using TaskFlow.Tasks.Application.Behaviors;
using TaskFlow.Tasks.Application.TaskItems.CreateTask;
using Xunit;

namespace TaskFlow.Tasks.Application.UnitTests.Behaviors;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_Should_Call_Next_When_Request_Is_Valid()
    {
        // Arrange
        // The pipeline should allow valid requests to continue to the actual handler.
        var validators = new List<IValidator<CreateTaskCommand>>
        {
            new CreateTaskCommandValidator()
        };

        var behavior = new ValidationBehavior<CreateTaskCommand, Guid>(validators);

        var request = new CreateTaskCommand(
            Title: "Valid task",
            Description: "Valid description",
            CreatedByUserId: Guid.NewGuid(),
            DueDate: DateTime.UtcNow.AddDays(1)
        );

        var expectedTaskId = Guid.NewGuid();

        RequestHandlerDelegate<Guid> next = (_) => Task.FromResult(expectedTaskId);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.Equal(expectedTaskId, result);
    }

    [Fact]
    public async Task Handle_Should_Throw_ValidationException_When_Request_Is_Invalid()
    {
        // Arrange
        // Invalid requests should be stopped before reaching the handler.
        var validators = new List<IValidator<CreateTaskCommand>>
        {
            new CreateTaskCommandValidator()
        };

        var behavior = new ValidationBehavior<CreateTaskCommand, Guid>(validators);

        var request = new CreateTaskCommand(
            Title: "",
            Description: "Invalid request",
            CreatedByUserId: Guid.Empty,
            DueDate: DateTime.UtcNow.AddDays(1)
        );

        RequestHandlerDelegate<Guid> next = (_) => Task.FromResult(Guid.NewGuid());

        // Act
        var act = () => behavior.Handle(request, next, CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<TaskFlow.SharedKernel.Validation.ValidationException>(act);

        Assert.Contains(nameof(CreateTaskCommand.Title), exception.Errors.Keys);
        Assert.Contains(nameof(CreateTaskCommand.CreatedByUserId), exception.Errors.Keys);
    }

    [Fact]
    public async Task Handle_Should_Not_Call_Next_When_Request_Is_Invalid()
    {
        // Arrange
        var validators = new List<IValidator<CreateTaskCommand>>
        {
            new CreateTaskCommandValidator()
        };

        var behavior = new ValidationBehavior<CreateTaskCommand, Guid>(validators);

        var request = new CreateTaskCommand(
            Title: "",
            Description: "Invalid request",
            CreatedByUserId: Guid.Empty,
            DueDate: DateTime.UtcNow.AddDays(1)
        );

        var nextWasCalled = false;

        RequestHandlerDelegate<Guid> next = (_) =>
        {
            nextWasCalled = true;
            return Task.FromResult(Guid.NewGuid());
        };

        // Act
        var act = () => behavior.Handle(request, next, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<TaskFlow.SharedKernel.Validation.ValidationException>(act);
        Assert.False(nextWasCalled);
    }
}