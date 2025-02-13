namespace Services.DTOs;

public class ProjectDto
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Priority { get; set; }
    public string Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
}