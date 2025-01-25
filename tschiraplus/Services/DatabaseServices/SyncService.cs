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

    /// <summary>
    /// Synchronizes the local db to the remote db
    /// </summary>
    public async Task SyncAsync()
    {
        var projectUsers = await GetAllProjectUsersForCurrentUser();
        var projects = await GetAllProjectsForCurrentUser(projectUsers);
        if (projects == null) return;
        
        var users = new List<Guid>();
        foreach (var project in projects.Where(project => !users.Contains(project.Owner.UserId)))
        {
            users.Add(project.Owner.UserId);
        }
        
        await AddUsersToLocalDbIfNotExists(users);
        AddProjectsToLocalDbIfNotExists(projects);
        AddProjectUsersToLocalDbIfNotExists(projectUsers);
    }

    /// <summary>
    /// Gets all ProjectUsers for the logged-in user from the remote database
    /// </summary>
    /// <returns>A List of all ProjectUsers where the UserId = CurrentUser.UserId</returns>
    private async Task<List<ProjectUserModel>?> GetAllProjectUsersForCurrentUser()
    {
        return await _projectUserRepository.GetAllProjectUsersByUserIdAsync(_appState.CurrentUser!.UserId);
    }

    /// <summary>
    /// Gets all Projects the logged-in user is part of
    /// </summary>
    /// <param name="projectUsers"></param>
    /// <returns>A List of all Projects the user is part of</returns>
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

    /// <summary>
    /// Adds all the given ProjectUsers to the local database if they aren't already in there
    /// </summary>
    /// <param name="projectUsers"></param>
    private void AddProjectUsersToLocalDbIfNotExists(List<ProjectUserModel>? projectUsers)
    {
        if (projectUsers == null) return;

        foreach (var projectUser in projectUsers.Where(projectUser => !_projectUserRepository.ProjectUserExists(projectUser.ProjectUserId)))
        {
            _projectUserRepository.AddProjectUser(projectUser);
        }
    }

    /// <summary>
    /// Adds all the given Projects to the local database if they aren't already in there
    /// </summary>
    /// <param name="projects"></param>
    private void AddProjectsToLocalDbIfNotExists(List<ProjectModel>? projects)
    {
        if (projects == null) return;

        foreach (var project in projects.Where(project => !_projectRepository.ProjectExists(project.ProjectId)))
        {
            _projectRepository.AddProject(project);
        }
    }

    /// <summary>
    /// Adds all the given Users to the local database if they aren't already in there
    /// </summary>
    /// <param name="userIds"></param>
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