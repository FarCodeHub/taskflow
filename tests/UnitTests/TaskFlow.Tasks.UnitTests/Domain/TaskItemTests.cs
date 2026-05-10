using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Tasks.Domain.TaskItems;
using TaskFlow.Tasks.Domain.TaskItems.Events;

namespace TaskFlow.Tasks.UnitTests.Domain;

public class TaskItemTests
{

    [Fact]
    public void Create_Should_Create_Task_With_Todo_Status()
    {
        var task = TaskItem.Create(
            title: "Write architecture tests",
            description: "Add tests to enforce clean architecture rules",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(3)
        );

        Assert.Equal("Write architecture tests", task.Title);
        Assert.Equal(TaskItemStatus.Todo, task.Status);
        Assert.NotEqual(Guid.Empty, task.Id);
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_Throw_Exception_When_Title_Is_Empty(string title)
    {
        var act = () => TaskItem.Create(
            title: title,
            description: "Description",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        Assert.Throws<TaskItemDomainException>(act);
    }

    [Fact]
    public void Create_Should_Throw_Exception_When_CreatedByUserId_Is_Empty()
    {
        var act = () => TaskItem.Create(
            title: "Valid title",
            description: "Description",
            createdByUserId: Guid.Empty,
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        Assert.Throws<TaskItemDomainException>(act);
    }



    [Fact]
    public void AssignTo_Should_Assign_Task_To_User()
    {
        // Arrange
        // Create a valid task first. Assignment is a behavior of an existing task.
        var task = TaskItem.Create(
            title: "Implement notifications",
            description: "Add real-time notifications using SignalR",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(2)
        );

        var assignedToUserId = Guid.NewGuid();

        // Act
        // Execute the domain behavior.
        task.AssignTo(assignedToUserId);

        // Assert
        // The task should now be assigned to the selected user.
        Assert.Equal(assignedToUserId, task.AssignedToUserId);
        Assert.NotNull(task.UpdatedAtUtc);
    }

    [Fact]
    public void AssignTo_Should_Throw_Exception_When_UserId_Is_Empty()
    {
        // Arrange
        var task = TaskItem.Create(
            title: "Implement notifications",
            description: "Add real-time notifications using SignalR",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(2)
        );

        // Act
        var act = () => task.AssignTo(Guid.Empty);

        // Assert
        // A task cannot be assigned to an unknown user.
        Assert.Throws<TaskItemDomainException>(act);
    }

    [Fact]
    public void AssignTo_Should_Raise_TaskAssignedDomainEvent()
    {
        // Arrange
        // Domain events allow the aggregate to record important business facts
        // without directly calling infrastructure concerns like message brokers.
        var task = TaskItem.Create(
            title: "Implement notifications",
            description: "Add real-time notifications using SignalR",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(2)
        );

        var assignedToUserId = Guid.NewGuid();

        // Act
        task.AssignTo(assignedToUserId);

        // Assert
        var domainEvent = Assert.Single(task.DomainEvents);

        var taskAssignedEvent = Assert.IsType<TaskAssignedDomainEvent>(domainEvent);

        Assert.Equal(task.Id, taskAssignedEvent.TaskId);
        Assert.Equal(assignedToUserId, taskAssignedEvent.AssignedToUserId);
    }



    [Fact]
    public void Start_Should_Change_Status_From_Todo_To_InProgress()
    {
        // Arrange
        // A newly created task starts with Todo status.
        var task = TaskItem.Create(
            title: "Implement task status transitions",
            description: "Add domain rules for changing task status",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(2)
        );

        // Act
        // Starting a task should move it to InProgress.
        task.Start();

        // Assert
        Assert.Equal(TaskItemStatus.InProgress, task.Status);
        Assert.NotNull(task.UpdatedAtUtc);
    }
    [Fact]
    public void Complete_Should_Change_Status_From_InProgress_To_Done()
    {
        // Arrange
        var task = TaskItem.Create(
            title: "Complete domain behavior",
            description: "Complete task status transition implementation",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(2)
        );

        task.Start();

        // Act
        task.Complete();

        // Assert
        Assert.Equal(TaskItemStatus.Done, task.Status);
        Assert.NotNull(task.UpdatedAtUtc);
    }



    [Fact]
    public void Complete_Should_Throw_Exception_When_Task_Is_Not_InProgress()
    {
        // Arrange
        // A new task is Todo by default.
        // It should not be completed before being started.
        var task = TaskItem.Create(
            title: "Invalid transition",
            description: "Trying to complete a task before starting it",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        // Act
        var act = () => task.Complete();

        // Assert
        Assert.Throws<TaskItemDomainException>(act);
    }


    [Fact]
    public void Cancel_Should_Change_Status_To_Cancelled_When_Task_Is_Not_Done()
    {
        // Arrange
        var task = TaskItem.Create(
            title: "Cancel task",
            description: "Cancel a task that is not completed",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        // Act
        task.Cancel();

        // Assert
        Assert.Equal(TaskItemStatus.Cancelled, task.Status);
        Assert.NotNull(task.UpdatedAtUtc);
    }



    [Fact]
    public void Cancel_Should_Throw_Exception_When_Task_Is_Done()
    {
        // Arrange
        var task = TaskItem.Create(
            title: "Completed task",
            description: "A completed task should not be cancelled",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        task.Start();
        task.Complete();

        // Act
        var act = () => task.Cancel();

        // Assert
        Assert.Throws<TaskItemDomainException>(act);
    }


    [Fact]
    public void AddComment_Should_Add_Comment_To_Task()
    {
        // Arrange
        // A comment belongs to a task and should be added through the aggregate root.
        var task = TaskItem.Create(
            title: "Add task comments",
            description: "Allow users to add comments to a task",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(2)
        );

        var userId = Guid.NewGuid();
        var commentText = "This task needs more details.";

        // Act
        task.AddComment(userId, commentText);

        // Assert
        var comment = Assert.Single(task.Comments);

        Assert.Equal(userId, comment.UserId);
        Assert.Equal(commentText, comment.Text);
        Assert.NotEqual(Guid.Empty, comment.Id);
    }


    [Fact]
    public void AddComment_Should_Throw_Exception_When_UserId_Is_Empty()
    {
        // Arrange
        var task = TaskItem.Create(
            title: "Invalid comment user",
            description: "Comment user id should be required",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        // Act
        var act = () => task.AddComment(Guid.Empty, "Valid comment text");

        // Assert
        Assert.Throws<TaskItemDomainException>(act);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void AddComment_Should_Throw_Exception_When_Text_Is_Empty(string text)
    {
        // Arrange
        // Theory allows the same test logic to run with multiple invalid inputs.
        var task = TaskItem.Create(
            title: "Invalid comment text",
            description: "Comment text should be required",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        // Act
        var act = () => task.AddComment(Guid.NewGuid(), text);

        // Assert
        Assert.Throws<TaskItemDomainException>(act);
    }


    [Fact]
    public void AddComment_Should_Trim_Comment_Text()
    {
        // Arrange
        var task = TaskItem.Create(
            title: "Trim comment",
            description: "Comment text should be normalized",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        // Act
        task.AddComment(Guid.NewGuid(), "  Please review this task.  ");

        // Assert
        var comment = Assert.Single(task.Comments);

        Assert.Equal("Please review this task.", comment.Text);
    }


    [Fact]
    public void Create_Should_Create_Task_With_Medium_Priority_By_Default()
    {
        // Arrange
        // Priority is set to Medium by default to keep task creation simple.
        // Later, the AI service can suggest a different priority.
        var createdByUserId = Guid.NewGuid();

        // Act
        var task = TaskItem.Create(
            title: "Add task priority",
            description: "Add default priority to task aggregate",
            createdByUserId: createdByUserId,
            dueDate: DateTime.UtcNow.AddDays(2)
        );

        // Assert
        Assert.Equal(TaskPriority.Medium, task.Priority);
    }

    [Fact]
    public void ChangePriority_Should_Update_Task_Priority()
    {
        // Arrange
        var task = TaskItem.Create(
            title: "Change priority",
            description: "Allow changing task priority manually",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(2)
        );

        // Act
        task.ChangePriority(TaskPriority.High);

        // Assert
        Assert.Equal(TaskPriority.High, task.Priority);
        Assert.NotNull(task.UpdatedAtUtc);
    }

    [Fact]
    public void Create_Should_Throw_Exception_When_DueDate_Is_In_The_Past()
    {
        // Arrange
        // A due date in the past is invalid because a new task should not be overdue at creation time.
        var pastDueDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var act = () => TaskItem.Create(
            title: "Invalid due date",
            description: "Due date should not be in the past",
            createdByUserId: Guid.NewGuid(),
            dueDate: pastDueDate
        );

        // Assert
        Assert.Throws<TaskItemDomainException>(act);
    }



    [Fact]
    public void ChangeDueDate_Should_Update_DueDate()
    {
        // Arrange
        var task = TaskItem.Create(
            title: "Change due date",
            description: "Allow changing task due date",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        var newDueDate = DateTime.UtcNow.AddDays(5);

        // Act
        task.ChangeDueDate(newDueDate);

        // Assert
        Assert.Equal(newDueDate, task.DueDate);
        Assert.NotNull(task.UpdatedAtUtc);
    }
    [Fact]
    public void ChangeDueDate_Should_Throw_Exception_When_DueDate_Is_In_The_Past()
    {
        // Arrange
        var task = TaskItem.Create(
            title: "Invalid due date update",
            description: "Changing due date to past should fail",
            createdByUserId: Guid.NewGuid(),
            dueDate: DateTime.UtcNow.AddDays(1)
        );

        var pastDueDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var act = () => task.ChangeDueDate(pastDueDate);

        // Assert
        Assert.Throws<TaskItemDomainException>(act);
    }
    [Fact]
    public void Create_Should_Return_Clear_Error_Message_When_Title_Is_Empty()
    {
        // Arrange
        var title = "";

        // Act
        var exception = Assert.Throws<TaskItemDomainException>(() =>
            TaskItem.Create(
                title,
                description: "Description",
                createdByUserId: Guid.NewGuid(),
                dueDate: DateTime.UtcNow.AddDays(1)
            )
        );

        // Assert
        Assert.Equal("Task title cannot be empty.", exception.Message);
    }
}

