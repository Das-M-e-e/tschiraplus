using Core.Models;
using Services.DTOs;

namespace Services.Repositories;

public interface IProjectRepository
{
    void AddProject(ProjectModel project);
    ProjectModel? GetProjectById(Guid projectId);
    List<ProjectDto> GetAllProjects();
    List<ProjectDto>? GetProjectsByUserId(Guid userId);
    void DeleteProject(Guid projectId);
    Task PostProjectAsync(ProjectModel project);
    Task<List<ProjectModel>> GetAllProjectsAsync();
    Task<ProjectModel> GetProjectByIdAsync(Guid projectId);
    Task<bool> DeleteAsync(Guid projectId);
    bool ProjectExists(Guid projectId);
}