using System.Net;
using System.Net.Http.Json;
using Xunit;
using TaskFlow.Tasks.Infrastructure.IntegrationTests.Common;

namespace TaskFlow.Tasks.Infrastructure.IntegrationTests.Middleware;

public class ExceptionHandlingMiddlewareTests
    : IClassFixture<TasksApiFactory>
{
    private readonly HttpClient _httpClient;

    public ExceptionHandlingMiddlewareTests(TasksApiFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTask_Should_Return_ValidationProblemDetails_When_Request_Is_Invalid()
    {
        // Arrange
        // Send an invalid request to trigger FluentValidation.
        var request = new
        {
            Title = "",
            Description = "Invalid request",
            CreatedByUserId = Guid.Empty,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync(
            "/api/tasks",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("validation_error", content);
        Assert.Contains("Validation Error", content);
    }
}