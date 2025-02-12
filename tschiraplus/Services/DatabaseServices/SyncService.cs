using System.Text.Json;
using Services.DTOs;
using Services.Repositories;

namespace Services.DatabaseServices;

public class SyncService : ISyncService
{
    private readonly RemoteDatabaseService _remoteDatabaseService;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectUserRepository _projectUserRepository;

    public SyncService(RemoteDatabaseService remoteDatabaseService, IUserRepository userRepository,
        IProjectRepository projectRepository, IProjectUserRepository projectUserRepository)
    {
        _remoteDatabaseService = remoteDatabaseService;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _projectUserRepository = projectUserRepository;
    }

    public async Task SyncProjectsAsync(Guid userId)
    {
        try
        {
            var response = await _remoteDatabaseService.GetByIdAsync("sync/projects", userId);
            if (string.IsNullOrEmpty(response))
            {
                Console.WriteLine("No data received from the remote database.");
                return;
            }

            var syncData = JsonSerializer.Deserialize<SyncProjectsResponse>(response);
            if (syncData?.Projects != null)
            {
                foreach (var project in syncData.Projects)
                {
                    var localProject = _projectRepository.GetProjectById(project.ProjectId);
                    if (localProject == null)
                    {
                        _projectRepository.AddProject(project);
                    }
                    else if (project.LastUpdated > localProject.LastUpdated)
                    {
                        _projectRepository.DeleteProject(localProject.ProjectId);
                        _projectRepository.AddProject(project);
                    }
                }
            }

            if (syncData?.ProjectUsers != null)
            {
                foreach (var projectUser in syncData.ProjectUsers)
                {
                    var localProjectUser = _projectUserRepository.GetProjectUserById(projectUser.ProjectUserId);
                    if (localProjectUser == null)
                    {
                        _projectUserRepository.AddProjectUser(projectUser);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during sync: {e.Message}");
        }
    }
}