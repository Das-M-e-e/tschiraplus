using Core.Models;
using Newtonsoft.Json;
using PetaPoco;
using Services.DatabaseServices;
using Services.DTOs;

namespace Services.Repositories;

public class ProjectUserRepository : IProjectUserRepository
{
    private readonly Database _db;
    private readonly RemoteDatabaseService _remoteDb;

    public ProjectUserRepository(Database db, RemoteDatabaseService remoteDb)
    {
        _db = db;
        _remoteDb = remoteDb;
    }
    
    //****** LOCAL DB ******//
    /// <summary>
    /// Saves a ProjectUser to the local database
    /// </summary>
    /// <param name="projectUserModel"></param>
    public void AddProjectUser(ProjectUserModel projectUserModel)
    {
        _db.Insert("ProjectUsers", "ProjectUserId", projectUserModel);
    }

    /// <summary>
    /// Gets a ProjectUser by id from the local database
    /// </summary>
    /// <param name="projectUserId"></param>
    /// <returns>The ProjectUserModel of the wanted projectUser</returns>
    public ProjectUserModel? GetProjectUserById(Guid projectUserId)
    {
        return _db.SingleOrDefault<ProjectUserModel>("SELECT * FROM ProjectUsers WHERE ProjectUserId = @0", projectUserId);
    }

    /// <summary>
    /// Gets a List of all ProjectUsers by userId from the local database
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>A List of all ProjectUsers where the UserId = userId</returns>
    public List<ProjectUserModel>? GetProjectUserByUserId(Guid userId)
    {
        return _db.Fetch<ProjectUserModel>("SELECT * FROM ProjectUsers WHERE UserId = @0", userId);
    }

    /// <summary>
    /// Gets a List of all ProjectUsers by projectId from the local database
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns>A List of all ProjectUsers where the ProjectId = projectId</returns>
    public List<ProjectUserModel>? GetProjectUserByProjectId(Guid projectId)
    {
        return _db.Fetch<ProjectUserModel>("SELECT * FROM ProjectUsers WHERE ProjectId = @0", projectId);
    }

    /// <summary>
    /// Gets a List of all ProjectUsers by userId from the local database
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>A List of all ProjectUsers where the UserId = userId</returns>
    public List<ProjectUserModel>? GetAllProjectUsersByUserId(Guid userId)
    {
        return _db.Fetch<ProjectUserModel>("SELECT * FROM ProjectUsers WHERE UserId = @0", userId);
    }

    /// <summary>
    /// Deletes a ProjectUser by id from the local database
    /// </summary>
    /// <param name="projectUserId"></param>
    public void DeleteProjectUser(Guid projectUserId)
    {
        _db.Delete("ProjectUsers", "ProjectUserId", projectUserId);
    }

    //****** REMOTE DB ******//
    /// <summary>
    /// Saves a ProjectUser to the remote database
    /// </summary>
    /// <param name="projectUserModel"></param>
    /// <returns>true or false</returns>
    public async Task<bool> PostProjectUserAsync(ProjectUserModel projectUserModel)
    {
        var jsonData = $"{{\"ProjectUserId\":\"{projectUserModel.ProjectUserId}\"," +
                       $"\"ProjectId\":\"{projectUserModel.ProjectId}\"," +
                       $"\"UserId\":\"{projectUserModel.UserId}\"," +
                       $"\"AssignedAt\":\"{projectUserModel.AssignedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\"}}";
        
        return await _remoteDb.PostAsync("ProjectUsers", jsonData);
    }

    /// <summary>
    /// Gets all ProjectUsers by userId from the remote database
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>A List of all ProjectUsers where the UserId = userId</returns>
    public async Task<List<ProjectUserModel>?> GetAllProjectUsersByUserIdAsync(Guid userId)
    {
        var jsonString = await _remoteDb.GetByIdAsync("ProjectUsers/ByUserId", userId);
        var projectUsers = JsonConvert.DeserializeObject<List<ProjectUserModel>>(jsonString);

        if (projectUsers == null)
        {
            throw new InvalidOperationException("Failed to deserialize JSON to List<ProjectUserModel>.");
        }
        
        return projectUsers;
    }

    /// <summary>
    /// Deletes a ProjectUser from the remote database
    /// </summary>
    /// <param name="projectUserId"></param>
    /// <returns>true or false</returns>
    public async Task<bool> DeleteProjectUserAsync(Guid projectUserId)
    {
        return await _remoteDb.DeleteAsync("ProjectUsers", projectUserId);
    }

    public async Task<bool> AddProjectUserAsync(string username, Guid inviterId, Guid projectId)
    {
        var data = new InvitationDto()
        {
            InviterId = inviterId,
            Username = username,
            ProjectId = projectId
        };
        
        return await _remoteDb.PostAsync("ProjectUsers/AddProjectUser", JsonConvert.SerializeObject(data));
    }

    //****** HELPER ******//
    /// <summary>
    /// Checks if a ProjectUser exists in the local database
    /// </summary>
    /// <param name="projectUserId"></param>
    /// <returns>true or false</returns>
    public bool ProjectUserExists(Guid projectUserId)
    {
        var projectUser = GetProjectUserById(projectUserId);
        
        return projectUser != null;
    }
}