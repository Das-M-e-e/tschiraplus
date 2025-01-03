using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class CreateNewProjectViewModel
{
    // Services
    private readonly IProjectService _projectService;
    private readonly MainMenuViewModel _mainMenuViewModel;
    
    // Bindings
    public string Name { get; set; }
    public string Description { get; set; }
    public string ProjectPriority { get; set; }
    
    // Commands
    public ICommand CreateProjectCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    
    public CreateNewProjectViewModel(IProjectService projectService, MainMenuViewModel mainMenuViewModel)
    {
        _projectService = projectService;
        _mainMenuViewModel = mainMenuViewModel;
        
        CreateProjectCommand = new RelayCommand(CreateProject);
        CancelCommand = new RelayCommand(Cancel);
    }
    
    /// <summary>
    /// Uses the _projectService to create a new project
    /// </summary>
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
        
        _mainMenuViewModel.OpenProjectCommand.Execute(projectId);
    }

    /// <summary>
    /// Cancels the project creation and switches back to the ProjectListView
    /// </summary>
    private void Cancel()
    {
        _mainMenuViewModel.SelectedTabIndex = 0;
        _mainMenuViewModel.CloseCurrentTab();
    }
}