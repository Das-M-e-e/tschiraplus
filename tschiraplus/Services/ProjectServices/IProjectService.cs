using Services.DTOs;

namespace Services.ProjectServices;

public interface IProjectService
{
    public void CreateTestProject(bool isOnline);
    public List<ProjectDto> GetAllProjects();
    public ProjectDto GetProjectById(Guid projectId);
    public Task DeleteProject(Guid projectId, bool isOnline);
    public Task SyncProjects();
}