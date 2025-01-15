namespace Services.DTOs;

public class TaskDto
{
    public Guid TaskId { get; init; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreationDate { get; init; } 
}