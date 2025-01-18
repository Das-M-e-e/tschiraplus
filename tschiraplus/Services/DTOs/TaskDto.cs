namespace Services.DTOs;

public class TaskDto
{
    public Guid TaskId { get; init; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } 
}