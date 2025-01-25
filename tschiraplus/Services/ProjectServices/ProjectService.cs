using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Mapper;
using Services.Repositories;

namespace Services.ProjectServices;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly UserDto _currentUser;
    private ProjectMapper _projectMapper;

    public ProjectService(IProjectRepository projectRepository, UserDto currentUser)
    {
        _projectRepository = projectRepository;
        _currentUser = currentUser;
        _projectMapper = new ProjectMapper(_projectRepository);
    }

    /// <summary>
    /// Creates a new project from a ProjectDto and saves it to the database and host
    /// </summary>
    /// <param name="projectDto"></param>
    public void CreateProject(ProjectDto projectDto)
    {
          _projectRepository.AddProject(_projectMapper.ToModel(projectDto));
          _projectRepository.PostProjectAsync(_projectMapper.ToModel(projectDto));
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
}