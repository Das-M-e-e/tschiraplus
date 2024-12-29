using Core.Models;
using Services.Repositories;

namespace Services.DatabaseServices;

public class SyncService : ISyncService
{
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly ApplicationState _appState;

    public SyncService(IProjectUserRepository projectUserRepository, IProjectRepository projectRepository,
        IUserRepository userRepository, ApplicationState appState)
    {
        _projectUserRepository = projectUserRepository;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _appState = appState;
    }

    public async Task SyncAsync()
    {
        var projectUsers = await GetAllProjectUsersForCurrentUser();
        var projects = await GetAllProjectsForCurrentUser(projectUsers);
        if (projects == null) return;
        
        var users = new List<Guid>();
        foreach (var project in projects.Where(project => !users.Contains(project.OwnerId)))
        {
            users.Add(project.OwnerId);
        }
        
        await AddUsersToLocalDbIfNotExists(users);
        AddProjectsToLocalDbIfNotExists(projects);
        AddProjectUsersToLocalDbIfNotExists(projectUsers);
    }

    private async Task<List<ProjectUserModel>?> GetAllProjectUsersForCurrentUser()
    {
        Console.WriteLine(_appState.CurrentUser.UserId);
        var projectUsers = await _projectUserRepository.GetAllProjectUsersByUserIdAsync(_appState.CurrentUser!.UserId);
        
        Console.WriteLine(projectUsers.Count);
        return projectUsers;
    }

    private async Task<List<ProjectModel>?> GetAllProjectsForCurrentUser(List<ProjectUserModel>? projectUsers)
    {
        if (projectUsers == null) return null;

        var projects = new List<ProjectModel>();
        foreach (var projectUser in projectUsers)
        {
            projects.Add(await _projectRepository.GetProjectByIdAsync(projectUser.ProjectId));
        }
        
        return projects;
    }

    private void AddProjectUsersToLocalDbIfNotExists(List<ProjectUserModel>? projectUsers)
    {
        if (projectUsers == null) return;

        foreach (var projectUser in projectUsers.Where(projectUser => !_projectUserRepository.ProjectUserExists(projectUser.ProjectUserId)))
        {
            _projectUserRepository.AddProjectUser(projectUser);
        }
    }

    private void AddProjectsToLocalDbIfNotExists(List<ProjectModel>? projects)
    {
        if (projects == null) return;

        foreach (var project in projects.Where(project => !_projectRepository.ProjectExists(project.ProjectId)))
        {
            _projectRepository.AddProject(project);
        }
    }

    private async Task AddUsersToLocalDbIfNotExists(List<Guid>? userIds)
    {
        if (userIds == null) return;

        foreach (var userId in userIds.Where(userId => !_userRepository.UserExists(userId)))
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            _userRepository.AddUser(user);
        }
    }
}