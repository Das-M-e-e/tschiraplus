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
    private readonly IProjectService _projectService;
    private readonly MainMenuViewModel _mainMenuViewModel;
    private readonly ApplicationState _appState;
    
    public ObservableCollection<ProjectViewModel> Projects { get; set; }
    
    public ICommand CreateNewProjectCommand { get; }
    public ICommand OpenProjectCommand { get; }

    public ProjectListViewModel(IProjectService projectService, MainMenuViewModel mainMenuViewModel, ApplicationState appState)
    {
        _projectService = projectService;
        _mainMenuViewModel = mainMenuViewModel;
        _appState = appState;

        Projects = new ObservableCollection<ProjectViewModel>();
        
        CreateNewProjectCommand = new RelayCommand(CreateNewProject);
        OpenProjectCommand = new RelayCommand<Guid>(OpenProject);

        LoadProjects();
    }

    private async Task LoadProjects()
    {
        if (_appState.IsOnline)
        {
            await _projectService.SyncProjects();
        }
        var allProjects = _projectService.GetAllProjects();
        UpdateProjectList(allProjects);
    }

    private void UpdateProjectList(IEnumerable<ProjectDTO> projectDtos)
    {
        Projects.Clear();
        foreach (var projectDto in projectDtos)
        {
            Projects.Add(new ProjectViewModel(projectDto, this));
        }
    }

    private void CreateNewProject()
    {
        _projectService.CreateTestProject(_appState.IsOnline);
        LoadProjects();
    }

    public void OpenProject(Guid projectId)
    {
        _mainMenuViewModel.OpenProjectCommand.Execute(projectId);
    }

    public async void DeleteProject(Guid projectId)
    {
        await _projectService.DeleteProject(projectId, _appState.IsOnline);
        LoadProjects();
    }
}