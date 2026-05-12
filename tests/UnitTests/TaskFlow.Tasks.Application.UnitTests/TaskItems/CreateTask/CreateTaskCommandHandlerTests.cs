using NSubstitute;
using TaskFlow.SharedKernel.Time;
using TaskFlow.Tasks.Application.Abstractions.Data;
using TaskFlow.Tasks.Application.TaskItems.CreateTask;
using TaskFlow.Tasks.Domain.TaskItems;
using Xunit;

namespace TaskFlow.Tasks.Application.UnitTests.TaskItems.CreateTask;

public class CreateTaskCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Task_And_Return_TaskId()
    {
        // Arrange
        // In application tests, we mock external dependencies such as repositories.
        // This keeps the test focused on the use case behavior, not database details.
        var repository = Substitute.For<ITaskItemRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();

        dateTimeProvider.UtcNow.Returns(new DateTime(2026, 01, 01, 10, 00, 00, DateTimeKind.Utc));
        unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        var handler = new CreateTaskCommandHandler(
            repository,
            unitOfWork,
            dateTimeProvider
        );

        var command = new CreateTaskCommand(
            Title: "Create application layer",
            Description: "Add CreateTask use case",
            CreatedByUserId: Guid.NewGuid(),
            DueDate: dateTimeProvider.UtcNow.AddDays(3)
        );

        // Act
        var taskId = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, taskId);

        await repository.Received(1).AddAsync(
            Arg.Is<TaskItem>(task =>
                task.Id == taskId &&
                task.Title == command.Title &&
                task.CreatedByUserId == command.CreatedByUserId),
            Arg.Any<CancellationToken>());

        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}