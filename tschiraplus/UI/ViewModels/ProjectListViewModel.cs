using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
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
    private readonly MainViewModel _mainViewModel;
    private readonly ApplicationState _appState;
    
    // Bindings
    public ObservableCollection<ProjectViewModel> Projects { get; set; } = [];

    private UserControl _createProjectFlyout;
    public UserControl CreateProjectFlyout
    {
        get => _createProjectFlyout;
        set => this.RaiseAndSetIfChanged(ref _createProjectFlyout, value);
    }
    
    // Commands
    public ICommand CreateNewProjectCommand { get; }

    public ProjectListViewModel(IProjectService projectService, MainViewModel mainViewModel, ApplicationState appState)
    {
        _projectService = projectService;
        _mainViewModel = mainViewModel;
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
    private async Task LoadProjects()
    {
        var allProjects = await _projectService.GetAllProjects();
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
        CreateProjectFlyout = new CreateNewProjectView
        {
            DataContext = new CreateNewProjectViewModel(_projectService, this)
        };
    }

    /// <summary>
    /// Opens a project via the _mainMenuViewModel
    /// </summary>
    /// <param name="projectId"></param>
    public void OpenProject(Guid projectId)
    {
        _mainViewModel.OpenProject(projectId);
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
        CreateProjectFlyout = new ProjectDetailsView
        {
            DataContext = new ProjectDetailsViewModel(_projectService, this, projectId)
        };
    }

    public void CloseFlyout()
    {
        LoadProjects();
    }
}