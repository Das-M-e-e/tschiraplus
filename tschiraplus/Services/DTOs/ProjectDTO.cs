namespace Services.DTOs;

public class ProjectDto
{
    public Guid ProjectId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}