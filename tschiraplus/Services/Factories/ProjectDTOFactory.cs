using Services.DTOs;

namespace Services.Factories;

public class ProjectDTOFactory
{
    public ProjectDto CreateProjectDTO(
        Guid projectId,
        string name,
        string description,
        string projectPriority
        )
    {
        
        var ProjectDto = new ProjectDto
        { 
         ProjectId = projectId,
         Name = name,
         Description = description,
         ProjectPriority = projectPriority
        };

        return ProjectDto;

    }
}
