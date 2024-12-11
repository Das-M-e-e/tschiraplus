using Services.DTOs;

namespace Services.ProjectServices;

public interface IProjectService
{
    public void CreateTestProject();
    public List<ProjectDTO> GetAllProjects();
    public ProjectDTO GetProjectById(Guid projectId);
}