using Core.Models;
using Services.DTOs;
using Services.Mapper;
using Services.Repositories;

namespace Services.ProjectServices;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly UserDto _currentUser;
    private readonly ProjectMapper _projectMapper;

    public ProjectService(IProjectRepository projectRepository, IProjectUserRepository projectUserRepository, ApplicationState appState)
    {
        _projectRepository = projectRepository;
        _projectUserRepository = projectUserRepository;
        _currentUser = appState.CurrentUser!;
        _projectMapper = new ProjectMapper(_projectRepository, appState);
    }

    /// <summary>
    /// Creates a new project from a ProjectDto and saves it to the database and host
    /// </summary>
    /// <param name="projectDto"></param>
    public void CreateProject(ProjectDto projectDto)
    {
          _projectRepository.AddProject(_projectMapper.ToModel(projectDto));
          _projectRepository.PostProjectAsync(_projectMapper.ToModel(projectDto));
          
          // Create the ProjectUser for the owner of the project
          var ownerProjectUser = new ProjectUserModel
          {
              ProjectUserId = Guid.NewGuid(),
              ProjectId = projectDto.ProjectId,
              UserId = _currentUser.UserId,
              AssignedAt = DateTime.Now
          };
          
          _projectUserRepository.AddProjectUser(ownerProjectUser);
    }
    
    
    /// <summary>
    /// Gets a list of all projects in the database as DTOs
    /// </summary>
    /// <returns>A List of ProjectDTOs</returns>
    public List<ProjectDto> GetAllProjects()
    {
        return _projectRepository.GetProjectsByUserId(_currentUser.UserId) ?? [];
    }

    /// <summary>
    /// Gets a single Project as DTO via its id (projectId)
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns>A ProjectDTO</returns>
    public ProjectDto GetProjectById(Guid projectId)
    {
        var projectModel = _projectRepository.GetProjectById(projectId);

        return _projectMapper.ToDto(projectModel);
    }

    /// <summary>
    /// Deletes a specific project (projectId) from both the local and remote database
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="isOnline"></param>
    public async Task DeleteProject(Guid projectId, bool isOnline)
    {
        if (isOnline)
        {
            await _projectRepository.DeleteAsync(projectId);
            _projectRepository.DeleteProject(projectId);
        }
    }

    /// <summary>
    /// Updates a specific project in both the local and remote database
    /// </summary>
    /// <param name="projectDto"></param>
    public void UpdateProject(ProjectDto projectDto)
    {
        _projectRepository.UpdateProject(_projectMapper.ToModel(projectDto));
    }

    /// <summary>
    /// Creates a ProjectUserModel to add a user to a project
    /// </summary>
    /// <param name="username"></param>
    /// <param name="projectId"></param>
    public async Task AddUserToProject(string username, Guid projectId)
    {
        await _projectUserRepository.AddProjectUserAsync(username, _currentUser.UserId, projectId);
    }
}