using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    private void LoadProjects()
    {
        var allProjects = _projectService.GetAllProjects();
        UpdateProjectList(allProjects);
    }
    
    private void UpdateProjectList(IEnumerable<ProjectDto> projectDtos)
    {
        Projects.Clear();
        foreach (var projectDto in projectDtos)
        {
            Projects.Add(new MenuProjectViewModel(projectDto.ProjectId, projectDto.Name, this));
        }
    }

    private void OpenProjectList()
    {
        _mainViewModel.OpenProjectList();
    }

    public void OpenProject(Guid projectId)
    {
        _mainViewModel.OpenProject(projectId);
    }

    public void OpenUserDetails()
    {
        _mainViewModel.OpenUserDetails();
    }

    public void OpenSettings()
    {
        _mainViewModel.OpenSettings();
    }
}