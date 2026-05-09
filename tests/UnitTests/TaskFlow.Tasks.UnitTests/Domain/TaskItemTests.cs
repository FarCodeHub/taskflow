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

        Assert.Throws<ArgumentException>(act);
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

        Assert.Throws<ArgumentException>(act);
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
        Assert.Throws<ArgumentException>(act);
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







}

