using Core.Enums;
using Core.Models;
using Services.DTOs;

namespace Services.Mapper;

public class ProjectMapper
{
    public static ProjectDto toDto(ProjectModel model)
    {
        var projectDto = new ProjectDto()
        {
            ProjectId = model.ProjectId,
            Name = model.Name,
            Description = model.Description,
            ProjectPriority = model.Priority.ToString()
        
        };

        return projectDto;

    }

    public static ProjectModel toModel(ProjectDto dto, UserModel user)
    {
        var ProjectModel = new ProjectModel()
        {
            ProjectId = dto.ProjectId,
            OwnerId = user.UserId,
            Name = dto.Name,
            Description = dto.Description,
            Priority = Enum.TryParse(dto.ProjectPriority, out ProjectPriority projectPriority)? projectPriority : ProjectPriority.Low,
            CreationDate = DateTime.Now,
            // TODO soll das noch in die UI eingearbeitet werden?? StartDate = 
            // TODO DueDate noch in UI einarbeiten 
            //TODO Lastupdated wo rausholen
            
        };
        
        return ProjectModel;
    }



}