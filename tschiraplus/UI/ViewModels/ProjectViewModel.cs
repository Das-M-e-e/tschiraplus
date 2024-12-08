using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;

namespace UI.ViewModels;

public class ProjectViewModel
{
    public Guid ProjectId { get; }
    public string Name { get; }
    public string Description { get; }

    public ICommand OpenProjectCommand { get; }
    private readonly ProjectListViewModel _projectListViewModel;

    public ProjectViewModel(ProjectDTO projectDto, ProjectListViewModel projectListViewModel)
    {
        ProjectId = projectDto.ProjectId;
        Name = projectDto.Name;
        Description = projectDto.Description;

        OpenProjectCommand = new RelayCommand(OpenProject);
        _projectListViewModel = projectListViewModel;
    }

    private void OpenProject()
    {
        _projectListViewModel.OpenProject(ProjectId);
    }
}