using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services.DTOs;
using Services.ProjectServices;

namespace UI.ViewModels;

public class ProjectDetailsViewModel : ViewModelBase
{
    // Services
    private readonly IProjectService _projectService;
    private readonly ProjectListViewModel _projectListViewModel;
    private ProjectDto _projectDto;
    
    // Bindings
    private string _title;
    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    private bool _isEditingTitle;
    public bool IsEditingTitle
    {
        get => _isEditingTitle;
        set => this.RaiseAndSetIfChanged(ref _isEditingTitle, value);
    }

    // Commands
    public ICommand StartEditingTitleCommand { get; set; }
    public ICommand SaveTitleCommand { get; set; }
    public ICommand CloseFlyoutCommand { get; set; }
    
    public ProjectDetailsViewModel(IProjectService projectService, ProjectListViewModel projectListViewModel, Guid projectId)
    {
        _projectService = projectService;
        _projectListViewModel = projectListViewModel;
        
        LoadProject(projectId);
        
        StartEditingTitleCommand = new RelayCommand(StartEditingTitle);
        SaveTitleCommand = new RelayCommand(SaveTitle);
        CloseFlyoutCommand = new RelayCommand(CloseFlyout);
    }

    private void LoadProject(Guid projectId)
    {
        _projectDto = _projectService.GetProjectById(projectId);
        Title = _projectDto.Name;
    }

    private void StartEditingTitle()
    {
        IsEditingTitle = true;
    }

    private void SaveTitle()
    {
        if (_projectDto.Name != Title)
        {
            _projectDto.Name = Title;
            _projectService.UpdateProject(_projectDto);
        }
        IsEditingTitle = false;
    }

    private void CloseFlyout()
    {
        _projectListViewModel.CloseFlyout();
    }
}