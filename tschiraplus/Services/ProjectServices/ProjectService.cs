using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;
using Services.UserServices;

namespace Services.ProjectServices;

public class ProjectService
{
    private readonly ProjectRepository _projectRepository;
    private UserDTO _currentUser;

    public ProjectService(ProjectRepository projectRepository, UserDTO currentUser)
    {
        _projectRepository = projectRepository;
        _currentUser = currentUser;
    }

    public void CreateProject(string name, string description)
    {
        var newProject = new ProjectModel
        {
            ProjectId = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreationDate = DateTime.Now,
            Status = ProjectStatus.NotStarted
        };
        
        _projectRepository.AddProject(newProject);
    }

    public void CreateTestProject()
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
            LastUpdated = DateTime.Now
        };
        
        _projectRepository.AddProject(newProject);
    }

    public List<ProjectDTO> GetAllProjects()
    {
        return _projectRepository.GetAllProjects();
    }

    public ProjectDTO GetProjectById(Guid projectId)
    {
        var projectModel = _projectRepository.GetProjectById(projectId);

        return new ProjectDTO
        {
            ProjectId = projectModel.ProjectId,
            Name = projectModel.Name,
            Description = projectModel.Description ?? "No description provided"
        };
    }
}