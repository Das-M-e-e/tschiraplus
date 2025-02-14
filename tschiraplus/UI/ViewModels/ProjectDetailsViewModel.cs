using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    private ProjectDto _project;
    
    // Bindings
    private string _title;
    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    private SelectionItemViewModel _status;
    public SelectionItemViewModel Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }
    
    private SelectionItemViewModel _priority;
    public SelectionItemViewModel Priority
    {
        get => _priority;
        set => this.RaiseAndSetIfChanged(ref _priority, value);
    }
    
    private DateTimeOffset? _startDate;
    public DateTimeOffset? StartDate
    {
        get => _startDate;
        set => this.RaiseAndSetIfChanged(ref _startDate, value);
    }
    
    private DateTimeOffset? _dueDate;
    public DateTimeOffset? DueDate
    {
        get => _dueDate;
        set => this.RaiseAndSetIfChanged(ref _dueDate, value);
    }

    private string? _description;
    public string? Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    
    public string Username { get; set; }
    
    private bool _isEditingTitle;
    public bool IsEditingTitle
    {
        get => _isEditingTitle;
        set => this.RaiseAndSetIfChanged(ref _isEditingTitle, value);
    }

    private bool _isEditingStartDate;
    public bool IsEditingStartDate
    {
        get => _isEditingStartDate;
        set => this.RaiseAndSetIfChanged(ref _isEditingStartDate, value);
    }
    
    private bool _isEditingDueDate;
    public bool IsEditingDueDate
    {
        get => _isEditingDueDate;
        set => this.RaiseAndSetIfChanged(ref _isEditingDueDate, value);
    }
    
    private bool _isEditingDescription;
    public bool IsEditingDescription
    {
        get => _isEditingDescription;
        set => this.RaiseAndSetIfChanged(ref _isEditingDescription, value);
    }

    public ObservableCollection<SelectionItemViewModel> StatusList { get; set; } = [];
    public ObservableCollection<SelectionItemViewModel> PriorityList { get; set; } = [];

    // Commands
    public ICommand StartEditingTitleCommand { get; set; }
    public ICommand SaveTitleCommand { get; set; }
    public ICommand StartEditingStartDateCommand { get; set; }
    public ICommand StartEditingDueDateCommand { get; set; }
    public ICommand CloseFlyoutCommand { get; set; }
    public ICommand AddProjectUserCommand { get; set; }
    
    public ProjectDetailsViewModel(IProjectService projectService, ProjectListViewModel projectListViewModel, Guid projectId)
    {
        _projectService = projectService;
        _projectListViewModel = projectListViewModel;
        LoadStatusList();
        LoadPriorityList();
        LoadProject(projectId);
        
        StartEditingTitleCommand = new RelayCommand(() => IsEditingTitle = true);
        SaveTitleCommand = new RelayCommand(SaveTitle);
        StartEditingStartDateCommand = new RelayCommand(() => IsEditingStartDate = true);
        StartEditingDueDateCommand = new RelayCommand(() => IsEditingDueDate = true);
        CloseFlyoutCommand = new RelayCommand(CloseFlyout);
        AddProjectUserCommand = new RelayCommand(AddProjectUser);
    }

    /// <summary>
    /// Fills the StatusList with hardcoded status.
    /// These fit to the Enum ProjectStatus
    /// </summary>
    private void LoadStatusList()
    {
        StatusList.Add(new SelectionItemViewModel{Name = "Not Started", Tag = "NotStarted", ColorCode = "#d3d3d3"});
        StatusList.Add(new SelectionItemViewModel{Name = "In Progress", Tag = "InProgress", ColorCode = "#ffa500"});
        StatusList.Add(new SelectionItemViewModel{Name = "On Hold", Tag = "OnHold", ColorCode = "#9370db"});
        StatusList.Add(new SelectionItemViewModel{Name = "Completed", Tag = "Completed", ColorCode = "#32cd32"});
    }

    /// <summary>
    /// Fills the PriorityList with hardcoded priorities.
    /// These fit with the Enum ProjectPriority
    /// </summary>
    private void LoadPriorityList()
    {
        PriorityList.Add(new SelectionItemViewModel{Name = "Not Set", Tag = "NotSet", ColorCode = "#d3d3d3"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Low", Tag = "Low", ColorCode = "#95d8a1"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Medium", Tag = "Medium", ColorCode = "#8fcdf3"});
        PriorityList.Add(new SelectionItemViewModel{Name = "High", Tag = "High", ColorCode = "#eba29a"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Critical", Tag = "Critical", ColorCode = "#dd5550"});
    }

    /// <summary>
    /// Loads the Project to show
    /// </summary>
    /// <param name="projectId"></param>
    private void LoadProject(Guid projectId)
    {
        _project = _projectService.GetProjectById(projectId);
        Title = _project.Name;
        Status = StatusList.FirstOrDefault(s => (string)s.Tag! == _project.Status)
            ?? StatusList.First();
        Priority = PriorityList.FirstOrDefault(s => (string)s.Tag! == _project.Priority)
            ?? PriorityList.First();
        StartDate = _project.StartDate.HasValue ? new DateTimeOffset(_project.StartDate.Value) : null;
        DueDate = _project.DueDate.HasValue ? new DateTimeOffset(_project.DueDate.Value) : null;
        Description = _project.Description;
    }

    /// <summary>
    /// Save the title of the project
    /// </summary>
    private void SaveTitle()
    {
        if (_project.Name != Title)
        {
            _project.Name = Title;
            _projectService.UpdateProject(_project);
        }
        IsEditingTitle = false;
    }

    /// <summary>
    /// Save the new status of the project
    /// </summary>
    /// <param name="status"></param>
    public void SetStatus(string? status)
    {
        Status = StatusList.First(s => s.Name == status);
        _project.Status = Status.Name.Replace(" ", "");
        _projectService.UpdateProject(_project);
    }

    /// <summary>
    /// save the new priority of the project
    /// </summary>
    /// <param name="priority"></param>
    public void SetPriority(string? priority)
    {
        Priority = PriorityList.First(s => s.Name == priority);
        _project.Priority = Priority.Name.Replace(" ", "");
        _projectService.UpdateProject(_project);
    }

    /// <summary>
    /// save the new start date of the project
    /// </summary>
    /// <param name="newDate"></param>
    public void EditStartDate(DateTime newDate)
    {
        _project.StartDate = newDate;
        _projectService.UpdateProject(_project);
        IsEditingStartDate = false;
    }

    /// <summary>
    /// save the new due date of the project
    /// </summary>
    /// <param name="newDate"></param>
    public void EditDueDate(DateTime newDate)
    {
        _project.DueDate = newDate;
        _projectService.UpdateProject(_project);
        IsEditingDueDate = false;
    }

    /// <summary>
    /// Close all date pickers
    /// </summary>
    public void CloseDatePicker()
    {
        IsEditingStartDate = false;
        IsEditingDueDate = false;
    }

    /// <summary>
    /// Show or hide the "Save Description" button
    /// </summary>
    public void ToggleDescriptionButton()
    {
        IsEditingDescription = !IsEditingDescription;
        SaveDescription();
    }

    /// <summary>
    /// save the new description of the project
    /// </summary>
    private void SaveDescription()
    {
        if (_project.Description != Description)
        {
            _project.Description = Description;
            _projectService.UpdateProject(_project);
        }
    }
    
    /// <summary>
    /// Add a user to the project (not yet fully implemented)
    /// </summary>
    private void AddProjectUser()
    {
        Console.WriteLine($"Adding project user {Username}");
        _projectService.AddUserToProject(Username, _project.ProjectId);   
    }

    /// <summary>
    /// Close the flyout
    /// </summary>
    private void CloseFlyout()
    {
        _projectListViewModel.CloseFlyout();
    }
}