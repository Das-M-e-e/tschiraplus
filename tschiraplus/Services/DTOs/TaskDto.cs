namespace Services.DTOs;

public class TaskDto
{
    public Guid TaskId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public DateTime? StartDate { get; init; } 
    public DateTime? DueDate { get; init; }
    
}