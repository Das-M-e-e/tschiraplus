using Core.Models;
using Newtonsoft.Json;
using PetaPoco;
using Services.DatabaseServices;
using Services.DTOs;

namespace Services.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly Database _db;
    private readonly RemoteDatabaseService _remoteDb;
    private readonly IProjectUserRepository _projectUserRepository;

    public ProjectRepository(Database db, RemoteDatabaseService remoteDb)
    {
        _db = db;
        _remoteDb = remoteDb;
        _projectUserRepository = new ProjectUserRepository(_db, _remoteDb);
    }

    //****** LOCAL DB ******//
    /// <summary>
    /// Saves a project to the local database
    /// </summary>
    /// <param name="project"></param>
    public void AddProject(ProjectModel project)
    {
        _db.Insert("Projects", "ProjectId", project);

        var projectUser = new ProjectUserModel
        {
            ProjectUserId = Guid.NewGuid(),
            ProjectId = project.ProjectId,
            UserId = project.OwnerId,
            AssignedAt = DateTime.Now
        };
        _projectUserRepository.AddProjectUser(projectUser);
    }

    /// <summary>
    /// Gets a project by id from the local database
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns>The ProjectModel of the wanted project</returns>
    public ProjectModel? GetProjectById(Guid projectId)
    {
        return _db.SingleOrDefault<ProjectModel>("SELECT * FROM Projects WHERE ProjectId=@0", projectId);
    }

    /// <summary>
    /// Gets all projects saved in the local database
    /// </summary>
    /// <returns>A List of all projects as ProjectDto</returns>
    public List<ProjectDto> GetAllProjects()
    {
        var projects = _db.Fetch<ProjectModel>("SELECT * FROM Projects");

        return projects.Select(project => new ProjectDto
        {
            ProjectId = project.ProjectId,
            Name = project.Name,
            Description = project.Description ?? "No description provided..."
        }).ToList();
    }

    /// <summary>
    /// Deletes a project by id from the local database
    /// </summary>
    /// <param name="projectId"></param>
    public void DeleteProject(Guid projectId)
    {
        _db.Execute("DELETE FROM Projects WHERE ProjectId=@0", projectId);
    }
    
    //****** REMOTE DB ******//
    /// <summary>
    /// Gets all saved projects from the remote database
    /// </summary>
    /// <returns>A List of all projects as ProjectModel</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<List<ProjectModel>> GetAllProjectsAsync()
    {
        var jsonString = await _remoteDb.GetAllAsync("Projects");
        var projectList = JsonConvert.DeserializeObject<List<ProjectModel>>(jsonString);

        if (projectList == null)
        {
            throw new InvalidOperationException("Failed to deserialize JSON to List<ProjectModel>.");
        }

        return projectList;
    }

    /// <summary>
    /// Gets a project by id from the remote database
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns>The ProjectModel of the wanted project</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<ProjectModel> GetProjectByIdAsync(Guid projectId)
    {
        var jsonString = await _remoteDb.GetByIdAsync("Projects", projectId);
        var project = JsonConvert.DeserializeObject<ProjectModel>(jsonString);
        
        if (project == null)
        {
            throw new InvalidOperationException("Failed to deserialize JSON to ProjectModel.");
        }
        
        return project;
    }
    
    /// <summary>
    /// Saves a project to the remote database
    /// </summary>
    /// <param name="project"></param>
    /// <returns>true or false</returns>
    public async Task PostProjectAsync(ProjectModel project)
    {
        var jsonData = $"{{\"projectId\":\"{project.ProjectId}\"," +
                       $"\"ownerId\":\"{project.OwnerId}\"," +
                       $"\"name\":\"{project.Name}\"," +
                       $"\"description\":\"{project.Description}\"," +
                       $"\"status\":{(int)project.Status}," +
                       $"\"priority\":{(int)project.Priority}," +
                       $"\"creationDate\":\"{project.CreationDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"," +
                       $"\"startDate\":\"{project.StartDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"," +
                       $"\"dueDate\":\"{project.DueDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"," +
                       $"\"lastUpdated\":\"{project.LastUpdated:yyyy-MM-ddTHH:mm:ss.fffZ}\"}}";
        
        await _remoteDb.PostAsync("Projects", jsonData);
        
        var projectUser = new ProjectUserModel
        {
            ProjectUserId = Guid.NewGuid(),
            ProjectId = project.ProjectId,
            UserId = project.OwnerId,
            AssignedAt = DateTime.Now
        };

        var projectUserJsonData = $"{{\"ProjectUserId\":\"{projectUser.ProjectUserId}\"," +
                                  $"\"ProjectId\":\"{projectUser.ProjectId}\"," +
                                  $"\"UserId\":\"{projectUser.UserId}\"," +
                                  $"\"AssignedAt\":\"{projectUser.AssignedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\"}}";
        
        await _remoteDb.PostAsync("ProjectUsers", projectUserJsonData);
    }

    /// <summary>
    /// Deletes a project by id from the remote database
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns>true or false</returns>
    public async Task<bool> DeleteAsync(Guid projectId)
    {
        return await _remoteDb.DeleteAsync("Projects", projectId);
    }
    
    //****** HELPER ******//
    /// <summary>
    /// Checks if the local database contains a project by id
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns>true or false</returns>
    public bool ProjectExists(Guid projectId)
    {
        var existingProject = GetProjectById(projectId);
        return existingProject != null;
    }
}