using Core.Models;

namespace Services.Repositories;

public interface IProjectUserRepository
{
    void AddProjectUser(ProjectUserModel projectUserModel);
    ProjectUserModel? GetProjectUserById(Guid projectUserId);
    List<ProjectUserModel>? GetProjectUserByUserId(Guid userId);
    List<ProjectUserModel>? GetProjectUserByProjectId(Guid projectId);
    List<ProjectUserModel>? GetAllProjectUsersByUserId(Guid userId);
    void DeleteProjectUser(Guid projectUserId);
    Task<bool> PostProjectUserAsync(ProjectUserModel projectUserModel);
    Task<List<ProjectUserModel>?> GetAllProjectUsersByUserIdAsync(Guid userId);
    Task<bool> DeleteProjectUserAsync(Guid projectUserId);
    bool ProjectUserExists(Guid projectUserId);
}