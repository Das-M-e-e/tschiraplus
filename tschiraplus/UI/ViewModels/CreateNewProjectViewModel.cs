using System;
using ReactiveUI;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class CreateNewProjectViewModel : ViewModelBase
{
    // Services
    private readonly IProjectService _projectService;
    private readonly ProjectListViewModel _projectListViewModel;
    
    // Bindings
    public string Name { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    private string _titleErrorMessage;
    public string TitleErrorMessage
    {
        get => _titleErrorMessage;
        set => this.RaiseAndSetIfChanged(ref _titleErrorMessage, value);
    }
    private bool _isTitleErrorMessageVisible;
    public bool IsTitleErrorMessageVisible
    {
        get => _isTitleErrorMessageVisible;
        set => this.RaiseAndSetIfChanged(ref _isTitleErrorMessageVisible, value);
    }
    
    public CreateNewProjectViewModel(IProjectService projectService, ProjectListViewModel projectListViewModel)
    {
        _projectService = projectService;
        _projectListViewModel = projectListViewModel;
    }
    
    /// <summary>
    /// Uses the _projectService to create a new project
    /// </summary>
    public bool CreateProject()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            TitleErrorMessage = "Name can't be empty!";
            IsTitleErrorMessageVisible = true;
            return false;
        }
        
        var projectId = Guid.NewGuid();
        var newProjectDto = new ProjectDto
        {
            ProjectId = projectId,
            Name = Name,
            Description = Description,
            Priority = Priority
        };

        IsTitleErrorMessageVisible = false;
        _projectService.CreateProject(newProjectDto);
        CloseFlyout();

        return true;
    }

    /// <summary>
    /// Closes the opened flyout
    /// </summary>
    private void CloseFlyout()
    {
        _projectListViewModel.CloseFlyout();
    }
}