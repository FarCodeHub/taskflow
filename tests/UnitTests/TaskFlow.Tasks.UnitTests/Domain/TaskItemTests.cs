using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Tasks.Domain.TaskItems;

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

}

