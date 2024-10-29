using Core.Enums;

namespace Core.Models;

public class SprintModel
{
    public Guid SprintId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public SprintStatus Status { get; set; }
    public Guid ProjectId { get; set; }
    public List<TaskModel> Tasks { get; set; } = [];
    public DateTime CreationDate { get; set; }
    public DateTime? LastUpdated { get; set; }
}