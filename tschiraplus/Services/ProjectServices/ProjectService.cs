using Core.Enums;
using Core.Models;
using Data.DatabaseConfig;
using Services.DatabaseServices;
using Services.DTOs;
using Services.Repositories;

namespace Services.ProjectServices;

public class ProjectService
{
    private readonly DatabaseService _databaseService;
    private readonly ProjectRepository _projectRepository;

    public ProjectService(DatabaseService databaseService, ProjectRepository projectRepository)
    {
        _databaseService = databaseService;
        _projectRepository = projectRepository;
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

        string projectDbPath = $"project_{newProject.Name}.db";
        _databaseService.CreateDatabase(projectDbPath);

        var projectDbConfig = new PetaPocoConfig($"Data Source={projectDbPath}");
        var dbInitializer = new DatabaseInitializer(projectDbConfig);
        dbInitializer.InitializeProjectDatabase();
    }

    public void CreateTestProject()
    {
        Guid projectId = Guid.NewGuid();
        
        var newProject = new ProjectModel
        {
            ProjectId = projectId,
            Name = "Test_Project-" + projectId,
            Description = "No description provided",
            CreationDate = DateTime.Now,
            Status = ProjectStatus.NotStarted,
            LastUpdated = DateTime.Now
        };
        
        _projectRepository.AddProject(newProject);

        string projectDbPath = DatabasePathHelper.GetDatabasePath($"project_{newProject.ProjectId}.db");
        _databaseService.CreateDatabase(projectDbPath);

        var projectDbConfig = new PetaPocoConfig($"Data Source={projectDbPath}");
        var dbInitializer = new DatabaseInitializer(projectDbConfig);
        dbInitializer.InitializeProjectDatabase();
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