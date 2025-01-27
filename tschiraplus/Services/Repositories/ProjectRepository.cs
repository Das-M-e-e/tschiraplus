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
    /// Gets all projects a user is part of from the local database
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>A List of all projects the user is part of as ProjectDto</returns>
    public List<ProjectDto>? GetProjectsByUserId(Guid userId)
    {
        var projectUsers = _projectUserRepository.GetAllProjectUsersByUserId(userId);
        return projectUsers?.Select(projectUser => GetProjectById(projectUser.ProjectId))
            .OfType<ProjectModel>()
            .Select(project => new ProjectDto
            {
                ProjectId = project.ProjectId, Name = project.Name, Description = project.Description ?? string.Empty, ProjectPriority = project.Priority.ToString(),
            })
            .ToList();
    }

    /// <summary>
    /// Deletes a project by id from the local database
    /// </summary>
    /// <param name="projectId"></param>
    public void DeleteProject(Guid projectId)
    {
        _db.Execute("DELETE FROM ProjectUsers WHERE ProjectId=@0", projectId);
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
        var jsonParts = new List<string>
        {
            $"\"projectId\":\"{project.ProjectId}\"",
            $"\"ownerId\":\"{project.OwnerId}\"",
            $"\"name\":\"{project.Name}\"",
            $"\"status\":{(int)project.Status}",
            $"\"priority\":{(int)project.Priority}",
            $"\"creationDate\":\"{project.CreationDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"",
            $"\"lastUpdated\":\"{project.LastUpdated:yyyy-MM-ddTHH:mm:ss.fffZ}\""
        };

        if (!string.IsNullOrEmpty(project.Description))
        {
            jsonParts.Add($"\"description\":\"{project.Description}\"");
        }

        if (project.StartDate.HasValue)
        {
            jsonParts.Add($"\"startDate\":\"{project.StartDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"");
        }

        if (project.DueDate.HasValue)
        {
            jsonParts.Add($"\"dueDate\":\"{project.DueDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"");
        }
        
        var jsonData = "{" + string.Join(",", jsonParts) + "}";
        
        Console.WriteLine(jsonData);
        
        await _remoteDb.PostAsync("Projects", jsonData);
        
        // Create the ProjectUser for the owner of the project
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
        await _remoteDb.DeleteAsync("ProjectUsers/DeleteByProjectId", projectId);
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