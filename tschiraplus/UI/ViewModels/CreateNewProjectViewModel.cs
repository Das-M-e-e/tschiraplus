using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class CreateNewProjectViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ProjectPriority { get; set; }
    
    public ICommand CreateProjectCommand { get; set; }
    
    private readonly ProjectService _projectService;


    public CreateNewProjectViewModel(ProjectService projectService)
    {
        _projectService = projectService;
        CreateProjectCommand = new RelayCommand(CreateProject);
    }
    
    

    public void CreateProject()
    {
        Guid projectId = Guid.NewGuid();
        var newProjectDto = new ProjectDTO
        {
            ProjectId = projectId,
            Name = Name,
            Description = Description,
            ProjectPriority = ProjectPriority
        };
        
        _projectService.CreateProject(newProjectDto);
        
        

    }
    
    
    

    /*public ProjectDTO CreateProjectDto()
    {
        Guid projectId = Guid.NewGuid();
        var newProjectDto = new ProjectDTO
        {
            ProjectId = projectId,
            Name = Name,
            Description = Description
        };
        return newProjectDto;
    }
    */
    
    
    
}