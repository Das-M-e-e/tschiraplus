using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class ProjectDetailsViewModel
{
    private readonly IProjectService _projectService;
    private ProjectDto _project;
    
    //Bindings
    public string Username { get; set; }
    public ICommand AddProjectUserCommand { get; set; }

    public ProjectDetailsViewModel(IProjectService projectService, Guid projectId)
    {
        _projectService = projectService;
        AddProjectUserCommand = new RelayCommand(AddProjectUser);
        LoadProject(projectId);
    }

    private void AddProjectUser()
    {
        Console.WriteLine($"Adding project user {Username}");
        _projectService.AddUserToProject(Username, _project.ProjectId);   
    }

    private void LoadProject(Guid projectId)
    {
        _project = _projectService.GetProjectById(projectId);
    }
    
    
    
    
}