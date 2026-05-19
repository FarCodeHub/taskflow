using FluentValidation.TestHelper;
using TaskFlow.Tasks.Application.TaskItems.CreateTask;
using Xunit;

namespace TaskFlow.Tasks.Application.UnitTests.TaskItems.CreateTask;

public class CreateTaskCommandValidatorTests
{
    private readonly CreateTaskCommandValidator _validator = new();

    [Fact]
    public void Validate_Should_Have_Error_When_Title_Is_Empty()
    {
        // Arrange
        var command = new CreateTaskCommand(
            Title: "",
            Description: "Description",
            CreatedByUserId: Guid.NewGuid(),
            DueDate: DateTime.UtcNow.AddDays(1)
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Title);
    }

    [Fact]
    public void Validate_Should_Have_Error_When_CreatedByUserId_Is_Empty()
    {
        // Arrange
        var command = new CreateTaskCommand(
            Title: "Valid title",
            Description: "Description",
            CreatedByUserId: Guid.Empty,
            DueDate: DateTime.UtcNow.AddDays(1)
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.CreatedByUserId);
    }
}