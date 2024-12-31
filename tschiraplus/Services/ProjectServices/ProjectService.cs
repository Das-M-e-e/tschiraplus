using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.ProjectServices;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly UserDto _currentUser;

    public ProjectService(IProjectRepository projectRepository, UserDto currentUser)
    {
        _projectRepository = projectRepository;
        _currentUser = currentUser;
    }

    // Todo: Temporary, will remove when project creation is implemented
    public void CreateTestProject(bool isOnline)
    {
        Guid projectId = Guid.NewGuid();
        
        var newProject = new ProjectModel
        {
            ProjectId = projectId,
            OwnerId = _currentUser.UserId,
            Name = "Test_Project-" + projectId,
            Description = "No description provided",
            CreationDate = DateTime.Now,
            Status = ProjectStatus.NotStarted,
            Priority = ProjectPriority.Low,
            LastUpdated = DateTime.Now,
            DueDate = DateTime.MaxValue,
            StartDate = DateTime.Today
        };

        if (!isOnline) return;
        _projectRepository.AddProject(newProject);
        _projectRepository.PostProjectAsync(newProject);
    }

    /// <summary>
    /// Creates a new project from a ProjectDto and saves it to the database and host
    /// </summary>
    /// <param name="projectDto"></param>
    public void CreateProject(ProjectDto projectDto)
    {
        var newProject = new ProjectModel
        {
            ProjectId = projectDto.ProjectId,
            Name = projectDto.Name,
            Description = projectDto.Description,
            CreationDate = DateTime.Now,
            Status = ProjectStatus.NotStarted,
            LastUpdated = DateTime.Now
        };
        
          _projectRepository.AddProject(newProject);
          _projectRepository.PostProjectAsync(newProject);
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

        return new ProjectDto
        {
            ProjectId = projectModel.ProjectId,
            Name = projectModel.Name,
            Description = projectModel.Description ?? "No description provided"
        };
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