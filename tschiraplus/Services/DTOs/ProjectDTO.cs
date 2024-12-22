namespace Services.DTOs;

public class ProjectDTO
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public string ProjectPriority { get; set; }
}