using Core.Enums;

namespace Core.Models;

public class ProjectModel
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? DueDate { get; set; }
    public ProjectStatus Status { get; set; }
    public ProjectPriority Priority { get; set; }
    public List<SprintModel>? Sprints { get; set; }
    public List<Guid> Members { get; set; } = [];
    public List<TaskModel> Tasks { get; set; } = [];
    public List<AttachmentModel>? Attachments { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime? LastUpdated { get; set; }
}