using Core.Models;
using Core.Enums;
using Services.Repositories;

public class ProjectFactory
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskRepository _taskRepository; //TODO nötig???
    private readonly IUserRepository _userRepository;

    public ProjectFactory(IProjectRepository projectRepository, ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _projectRepository = projectRepository;
        _taskRepository = taskRepository; // TODO nötig??
        _userRepository = userRepository;
    }

    public async Task<ProjectModel> GetProjectFromDatabase(Guid projectId)
    {
        // Lädt das Projekt aus der Datenbank
        var projectEntity = await _projectRepository.GetProjectByIdAsync(projectId);

        // Lädt den Besitzer (UserModel) des Projekts
        var owner = await _userRepository.GetUserByIdAsync(projectEntity.OwnerId);

        

        // Erstellet das ProjectModel
        return new ProjectModel
        {
            ProjectId = projectEntity.ProjectId,
            OwnerId = owner.UserId,
            Name = projectEntity.Name,
            Description = projectEntity.Description,
            Status = projectEntity.Status,
            Priority = projectEntity.Priority,
            CreationDate = projectEntity.CreationDate,
            StartDate = projectEntity.StartDate,
            DueDate = projectEntity.DueDate,
            LastUpdated = projectEntity.LastUpdated,
            //Tasks = tasks
        };
    }
}