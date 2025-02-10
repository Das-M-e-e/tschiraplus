using Core.Enums;

namespace Core.Models;

public class ProjectModel
{
    public Guid ProjectId { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ProjectStatus Status { get; set; }
    public ProjectPriority Priority { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime LastUpdated { get; set; }
}