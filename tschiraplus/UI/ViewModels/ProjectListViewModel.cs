using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class ProjectListViewModel
{
    // Services
    private readonly IProjectService _projectService;
    private readonly MainMenuViewModel _mainMenuViewModel;
    private readonly ApplicationState _appState;
    
    // Bindings
    public ObservableCollection<ProjectViewModel> Projects { get; set; }
    
    // Commands
    public ICommand CreateNewProjectCommand { get; }
    public ICommand OpenProjectCommand { get; }

    public ProjectListViewModel(IProjectService projectService, MainMenuViewModel mainMenuViewModel, ApplicationState appState)
    {
        _projectService = projectService;
        _mainMenuViewModel = mainMenuViewModel;
        _appState = appState;

        Projects = new ObservableCollection<ProjectViewModel>();
        
        CreateNewProjectCommand = new AsyncRelayCommand(CreateNewProject);
        OpenProjectCommand = new RelayCommand<Guid>(OpenProject);

        LoadProjects();
    }

    /// <summary>
    /// Gets all projects from the host using the _projectService and updates the Projects list
    /// </summary>
    private async Task LoadProjects()
    {
        if (_appState.IsOnline)
        {
            await _projectService.SyncProjects();
        }
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
    private async Task CreateNewProject()
    {
        _projectService.CreateTestProject(_appState.IsOnline);
        await LoadProjects();
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
        await LoadProjects();
    }
}