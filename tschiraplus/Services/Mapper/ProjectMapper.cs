using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.Mapper;

public class ProjectMapper
{
    private readonly IProjectRepository _projectRepository;
    private readonly ApplicationState _appState;

    public ProjectMapper (IProjectRepository projectRepository, ApplicationState appState)
    {
        _projectRepository = projectRepository;
        _appState = appState;
    }
    
    public ProjectDto ToDto(ProjectModel model)
    {
        var projectDto = new ProjectDto()
        {
            ProjectId = model.ProjectId,
            Name = model.Name,
            Description = model.Description,
            Priority = model.Priority.ToString(),
            Status = model.Status.ToString(),
            StartDate= model.StartDate,
            DueDate = model.DueDate
        };

        return projectDto;

    }

    public ProjectModel ToModel(ProjectDto dto)
    {
        
        var projectModel = _projectRepository.GetProjectById(dto.ProjectId);
        if (projectModel == null)
        {
            projectModel = new ProjectModel
            {
                ProjectId = dto.ProjectId,
                OwnerId = _appState.CurrentUser!.UserId,
                Name = dto.Name,
                Description = dto.Description,
                Status = Enum.TryParse(dto.Status, out ProjectStatus stat) ? stat : ProjectStatus.NotStarted,
                Priority = Enum.TryParse(dto.Priority, out ProjectPriority prio) ? prio : ProjectPriority.Low,
                CreationDate = DateTime.Now,
                StartDate = dto.StartDate,
                DueDate = dto.DueDate,
                LastUpdated = DateTime.Now
            };
        }
        else
        {
            projectModel.Name = dto.Name;
            projectModel.Description = dto.Description;
            projectModel.Priority = Enum.TryParse(dto.Priority, out ProjectPriority priority) ? priority : ProjectPriority.Low;
            projectModel.Status = Enum.TryParse(dto.Status, out ProjectStatus status) ? status : ProjectStatus.NotStarted;
            projectModel.StartDate = dto.StartDate;
            projectModel.DueDate = dto.DueDate;
        }
        
        return projectModel;
    }



}