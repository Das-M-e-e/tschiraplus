using Core.Models;
using Services.DTOs;

namespace Services.Repositories;

public interface IProjectRepository
{
    void AddProject(ProjectModel project);
    ProjectModel GetProjectById(Guid projectId);
    List<ProjectDto> GetAllProjects();
    void DeleteProject(Guid projectId);
    Task<bool> PostProjectAsync(ProjectModel project);
    Task<List<ProjectModel>> GetAllProjectsAsync();
    Task<bool> DeleteAsync(Guid projectId);
    Task SyncProjectsAsync();
}