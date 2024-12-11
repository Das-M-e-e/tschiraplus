using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class ProjectListViewModel
{
    private readonly IProjectService _projectService;
    private readonly MainMenuViewModel _mainMenuViewModel;
    
    public ObservableCollection<ProjectViewModel> Projects { get; set; }
    
    public ICommand CreateNewProjectCommand { get; }
    public ICommand OpenProjectCommand { get; }

    public ProjectListViewModel(IProjectService projectService, MainMenuViewModel mainMenuViewModel)
    {
        _projectService = projectService;
        _mainMenuViewModel = mainMenuViewModel;

        Projects = new ObservableCollection<ProjectViewModel>();
        
        CreateNewProjectCommand = new RelayCommand(CreateNewProject);
        OpenProjectCommand = new RelayCommand<Guid>(OpenProject);
        
        LoadProjects();
    }

    private void LoadProjects()
    {
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
        _projectService.CreateTestProject();
        LoadProjects();
    }

    public void OpenProject(Guid projectId)
    {
        _mainMenuViewModel.OpenProjectCommand.Execute(projectId);
    }
}