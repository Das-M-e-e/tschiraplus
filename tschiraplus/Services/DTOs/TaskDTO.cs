namespace Services.DTOs;

public class TaskDto
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreationDate { get; set; }
}