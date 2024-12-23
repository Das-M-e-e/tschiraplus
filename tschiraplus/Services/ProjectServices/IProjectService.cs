using Services.DTOs;

namespace Services.ProjectServices;

public interface IProjectService
{
    public void CreateTestProject(bool isOnline);
    public List<ProjectDTO> GetAllProjects();
    public ProjectDTO GetProjectById(Guid projectId);
    public Task DeleteProject(Guid projectId);
    public Task SyncProjects();
}