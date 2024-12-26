using Core.Models;
using Services.DTOs;

namespace Services.Repositories;

public interface IProjectRepository
{
    public void AddProject(ProjectModel project);
    public ProjectModel GetProjectById(Guid projectId);
    public List<ProjectDto> GetAllProjects();
    public void DeleteProject(Guid projectId);
    public Task<bool> PostProjectAsync(ProjectModel project);
    public Task<List<ProjectModel>> GetAllProjectsAsync();
    public Task<bool> DeleteAsync(Guid projectId);
    public Task SyncProjectsAsync();
}