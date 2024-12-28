using Services.DTOs;

namespace Services.ProjectServices;

public interface IProjectService
{
    void CreateTestProject(bool isOnline);
    List<ProjectDto> GetAllProjects();
    ProjectDto GetProjectById(Guid projectId);
    Task DeleteProject(Guid projectId, bool isOnline);
    Task SyncProjects();
    void CreateProject(ProjectDto projectDto);
}