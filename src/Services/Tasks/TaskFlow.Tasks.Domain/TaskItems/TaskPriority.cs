namespace TaskFlow.Tasks.Domain.TaskItems;

/// <summary>
/// Represents the business priority of a task.
/// In future phases, this value can be suggested by the AI service.
/// </summary>
public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3
}