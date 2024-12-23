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

    public ProjectRepository(Database db, RemoteDatabaseService remoteDb)
    {
        _db = db;
        _remoteDb = remoteDb;
    }

    //****** LOCAL DB ******//
    public void AddProject(ProjectModel project)
    {
        _db.Insert("Projects", "ProjectId", project);
    }

    public ProjectModel GetProjectById(Guid projectId)
    {
        return _db.SingleOrDefault<ProjectModel>("SELECT * FROM Projects WHERE ProjectId=@0", projectId);
    }

    public List<ProjectDTO> GetAllProjects()
    {
        var projects = _db.Fetch<ProjectModel>("SELECT * FROM Projects");

        return projects.Select(project => new ProjectDTO
        {
            ProjectId = project.ProjectId,
            Name = project.Name,
            Description = project.Description ?? "No description provided..."
        }).ToList();
    }

    public void DeleteProject(Guid projectId)
    {
        _db.Execute("DELETE FROM Projects WHERE ProjectId=@0", projectId);
    }
    
    //****** REMOTE DB ******//
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
    
    public async Task<bool> PostProjectAsync(ProjectModel project)
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
        
        return await _remoteDb.PostAsync("Projects", jsonData);
    }

    public async Task<bool> DeleteAsync(Guid projectId)
    {
        return await _remoteDb.DeleteAsync("Projects", projectId);
    }
    
    //****** HELPER ******//
    public async Task SyncProjectsAsync()
    {
        try
        {
            var remoteProjects = await GetAllProjectsAsync();

            foreach (var remoteProject in remoteProjects.Where(remoteProject => !LocalDatabaseContainsProject(remoteProject.ProjectId)))
            {
                AddProject(remoteProject);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private bool LocalDatabaseContainsProject(Guid projectId)
    {
        var existingProject = _db.SingleOrDefault<ProjectModel>("SELECT * FROM Projects WHERE ProjectId=@0", projectId);
        return existingProject != null;
    }
}