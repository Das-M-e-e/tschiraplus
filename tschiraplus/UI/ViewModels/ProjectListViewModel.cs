using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services;
using Services.DTOs;
using Services.ProjectServices;
using UI.Views;

namespace UI.ViewModels;

public class ProjectListViewModel : ViewModelBase, IActivatableViewModel
{
    // Services
    public ViewModelActivator Activator { get; }
    private readonly IProjectService _projectService;
    private readonly MainMenuViewModel _mainMenuViewModel;
    private readonly ApplicationState _appState;
    
    // Bindings
    public ObservableCollection<ProjectViewModel> Projects { get; set; } = [];

    private bool _isProjectDetailsOpen;
    public bool IsProjectDetailsOpen
    {
        get => _isProjectDetailsOpen;
        set => this.RaiseAndSetIfChanged(ref _isProjectDetailsOpen, value);
    }

    private object? _projectDetailsFlyout;
    public object? ProjectDetailsFlyout
    {
        get => _projectDetailsFlyout;
        set => this.RaiseAndSetIfChanged(ref _projectDetailsFlyout, value);
    }
    
    // Commands
    public ICommand CreateNewProjectCommand { get; }

    public ProjectListViewModel(IProjectService projectService, MainMenuViewModel mainMenuViewModel, ApplicationState appState)
    {
        _projectService = projectService;
        _mainMenuViewModel = mainMenuViewModel;
        _appState = appState;
        
        CreateNewProjectCommand = new RelayCommand(CreateNewProject);

        Activator = new ViewModelActivator();
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            LoadProjects();
        });
    }

    /// <summary>
    /// Gets all projects from the host using the _projectService and updates the Projects list
    /// </summary>
    private void LoadProjects()
    {
        var allProjects = _projectService.GetAllProjects();
        UpdateProjectList(allProjects);
    }

    /// <summary>
    /// Updates the Projects list using the given list of ProjectDtos
    /// </summary>
    /// <param name="projectDtos"></param>
    private void UpdateProjectList(IEnumerable<ProjectDto> projectDtos)
    {
        Projects.Clear();
        foreach (var projectDto in projectDtos)
        {
            Projects.Add(new ProjectViewModel(projectDto, this));
        }
    }

    /// <summary>
    /// Uses the _projectService to create a new project, then updates the Projects list
    /// </summary>
    private void CreateNewProject()
    {
        _mainMenuViewModel.CreateNewProjectCommand.Execute(null);
    }

    /// <summary>
    /// Opens a project via the _mainMenuViewModel
    /// </summary>
    /// <param name="projectId"></param>
    public void OpenProject(Guid projectId)
    {
        _mainMenuViewModel.OpenProjectCommand.Execute(projectId);
    }

    /// <summary>
    /// Deletes a project using the _projectService
    /// </summary>
    /// <param name="projectId"></param>
    public async Task DeleteProject(Guid projectId)
    {
        await _projectService.DeleteProject(projectId, _appState.IsOnline);
        LoadProjects();
    }

    public void OpenProjectDetails(Guid projectId)
    {
        var project = _projectService.GetProjectById(projectId);
        
        ProjectDetailsFlyout = new ProjectDetailsView
        {
            DataContext = new ProjectDetailsViewModel(
                _projectService,
                this,
                project.ProjectId)
        };
    }

    public void CloseFlyout()
    {
        ProjectDetailsFlyout = null;
        LoadProjects();
    }
}