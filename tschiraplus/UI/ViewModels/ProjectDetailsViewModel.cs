using System;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class ProjectDetailsViewModel : ViewModelBase
{
    // Services
    private readonly IProjectService _projectService;
    
    // Bindings
    public Guid ProjectId { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }

    public ProjectDetailsViewModel(ProjectDto project, IProjectService projectService)
    {
        _projectService = projectService;
        
        ProjectId = project.ProjectId;
        Name = project.Name;
        Description = project.Description ?? string.Empty;
        Status = project.Status;
        Priority = project.Priority;
    }
}