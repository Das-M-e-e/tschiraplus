using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.Mapper;

public class ProjectMapper
{
    private readonly IProjectRepository _projectRepository;

    public ProjectMapper (IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    public ProjectDto ToDto(ProjectModel model)
    {
        var projectDto = new ProjectDto()
        {
            ProjectId = model.ProjectId,
            Name = model.Name,
            Description = model.Description,
            Priority = model.Priority.ToString(),
            Status = model.Status.ToString()
        
        };

        return projectDto;

    }

    public ProjectModel ToModel(ProjectDto dto)
    {
        
        var projectModel = _projectRepository.GetProjectById(dto.ProjectId);
        if (projectModel == null)
        {
            throw new NullReferenceException($"Project {dto.ProjectId} not found");
        }
        
        projectModel.Name = dto.Name;
        projectModel.Description = dto.Description;
        projectModel.Priority = Enum.TryParse(dto.Priority, out ProjectPriority priority) ? priority : ProjectPriority.Low;
        projectModel.Status = Enum.TryParse(dto.Status, out ProjectStatus status) ? status : ProjectStatus.NotStarted;
        
        return projectModel;
    }



}