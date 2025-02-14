using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;

namespace UI.ViewModels;

public class ProjectViewModel
{
    // Services
    private readonly ProjectListViewModel _projectListViewModel;
    
    // Bindings
    public Guid ProjectId { get; }
    public string Name { get; }
    public string? Description { get; }

    // Commands
    public ICommand OpenProjectCommand { get; }
    public ICommand DeleteProjectCommand { get; }
    public ICommand OpenProjectDetailsCommand { get; }

    public ProjectViewModel(ProjectDto projectDto, ProjectListViewModel projectListViewModel)
    {
        _projectListViewModel = projectListViewModel;
        
        ProjectId = projectDto.ProjectId;
        Name = projectDto.Name;
        Description = projectDto.Description;

        OpenProjectCommand = new RelayCommand(OpenProject);
        DeleteProjectCommand = new AsyncRelayCommand(DeleteProject);
        _projectListViewModel = projectListViewModel;
        OpenProjectDetailsCommand = new RelayCommand(OpenProjectDetails);
    }

    /// <summary>
    /// Uses the _projectListViewModel to open the project
    /// </summary>
    private void OpenProject()
    {
        _projectListViewModel.OpenProject(ProjectId);
    }

    /// <summary>
    /// Uses the _projectListViewModel to delete the project
    /// </summary>
    private async Task DeleteProject()
    {
        await _projectListViewModel.DeleteProject(ProjectId);
    }

    private void OpenProjectDetails()
    {
        _projectListViewModel.OpenProjectDetails(ProjectId);
    }
}