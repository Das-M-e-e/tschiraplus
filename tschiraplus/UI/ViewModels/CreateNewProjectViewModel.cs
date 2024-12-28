using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class CreateNewProjectViewModel
{
    private readonly IProjectService _projectService;
    
    public string Name { get; set; }
    public string Description { get; set; }
    public string ProjectPriority { get; set; }
    
    public ICommand CreateProjectCommand { get; set; }
    
    public CreateNewProjectViewModel(IProjectService projectService)
    {
        _projectService = projectService;
        CreateProjectCommand = new RelayCommand(CreateProject);
    }
    
    private void CreateProject()
    {
        var projectId = Guid.NewGuid();
        var newProjectDto = new ProjectDto
        {
            ProjectId = projectId,
            Name = Name,
            Description = Description,
            ProjectPriority = ProjectPriority
        };
        
        _projectService.CreateProject(newProjectDto);
    }
}