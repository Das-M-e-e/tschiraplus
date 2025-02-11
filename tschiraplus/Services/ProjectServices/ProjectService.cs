﻿using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.ProjectServices;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly UserDto _currentUser;

    public ProjectService(IProjectRepository projectRepository, IProjectUserRepository projectUserRepository, UserDto currentUser)
    {
        _projectRepository = projectRepository;
        _projectUserRepository = projectUserRepository;
        _currentUser = currentUser;
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
            OwnerId = _currentUser.UserId,
            Name = projectDto.Name,
            Description = projectDto.Description ?? null,
            Status = ProjectStatus.NotStarted,
            Priority = Enum.TryParse<ProjectPriority>(projectDto.Priority, out var priority) ? priority : ProjectPriority.Low,
            CreationDate = DateTime.Now,
            LastUpdated = DateTime.Now,
        };

        // Create the ProjectUser for the owner of the project
        var ownerProjectUser = new ProjectUserModel
        {
            ProjectUserId = Guid.NewGuid(),
            ProjectId = projectDto.ProjectId,
            UserId = _currentUser.UserId,
            AssignedAt = DateTime.Now
        };
        
        _projectRepository.AddProject(newProject);
        _projectUserRepository.AddProjectUser(ownerProjectUser);
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
    
    public async Task AddUserToProject(string username, Guid projectId)
    {
        await _projectUserRepository.AddProjectUserAsync(username, _currentUser.UserId, projectId);
        
    }
}