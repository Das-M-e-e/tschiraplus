using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class MainMenuViewModel
{
    // Services
    private readonly MainViewModel _mainViewModel;
    private readonly IProjectService _projectService;
    
    // Bindings
    public ObservableCollection<MenuProjectViewModel> Projects { get; set; } = [];
    
    // Commands
    public ICommand OpenProjectListCommand { get; set; }
    public ICommand OpenUserDetailsCommand { get; set; }
    public ICommand OpenSettingsCommand { get; set; }

    public MainMenuViewModel(MainViewModel mainViewModel, IProjectService projectService)
    {
        _mainViewModel = mainViewModel;
        _projectService = projectService;

        OpenProjectListCommand = new RelayCommand(OpenProjectList);
        OpenUserDetailsCommand = new RelayCommand(OpenUserDetails);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        
        LoadProjects();
    }

    /// <summary>
    /// Uses the ProjectService to load all projects
    /// </summary>
    private async Task LoadProjects()
    {
        var allProjects = await _projectService.GetAllProjects();
        UpdateProjectList(allProjects);
    }
    
    /// <summary>
    /// Updates the Projects List with the newly loaded List of projects
    /// </summary>
    /// <param name="projectDtos"></param>
    private void UpdateProjectList(IEnumerable<ProjectDto> projectDtos)
    {
        Projects.Clear();
        foreach (var projectDto in projectDtos)
        {
            Projects.Add(new MenuProjectViewModel(projectDto.ProjectId, projectDto.Name, this));
        }
    }

    /// <summary>
    /// Opens the ProjectListView
    /// </summary>
    private void OpenProjectList()
    {
        _mainViewModel.OpenProjectList();
    }

    /// <summary>
    /// Opens a certain project by id
    /// </summary>
    /// <param name="projectId"></param>
    public void OpenProject(Guid projectId)
    {
        _mainViewModel.OpenProject(projectId);
    }

    /// <summary>
    /// Opens the UserProfileView
    /// </summary>
    private void OpenUserDetails()
    {
        _mainViewModel.OpenUserDetails();
    }

    /// <summary>
    /// Opens the SettingsView
    /// </summary>
    private void OpenSettings()
    {
        _mainViewModel.OpenSettings();
    }
}