using Services.DTOs;

namespace Services.ProjectServices;

public interface IProjectService
{
    List<ProjectDto> GetAllProjects();
    ProjectDto GetProjectById(Guid projectId);
    Task DeleteProject(Guid projectId, bool isOnline);
    void CreateProject(ProjectDto projectDto);
    Task AddUserToProject(string username, Guid projectId);
}